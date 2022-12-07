﻿using Crypto_Wallet.Classes;
using Crypto_Wallet.Classes.Wallets;
using Crypto_Wallet.Classes.Assets;
using Crypto_Wallet.Enums;
using ConsoleTables;
using Crypto_Wallet.Interfaces;
using Crypto_Wallet.Global.Constants;
using Crypto_Wallet.Global.Data;

var globalData = new GlobalData();

//------ MAIN MENU ------
var mainMenuChoice = 0;

do
{
    mainMenuChoice = GetMenuChoiceFromUser(MenuConstants.MAIN_MENU_TEXT, "Main Menu");

    switch (mainMenuChoice)
    {
        case 1:
            CreateNewCryptoWallet();
            break;
        case 2:
            Console.Clear();
            PrintAllWallets();
            var targetWallet = GetWalletByAddress(MessageConstants.ACCESS_WALLET_REQUEST_MESSAGE, MessageConstants.ACESS_WALLET_FAILED_MESSAGE);
            if (targetWallet == null)
                break;
            Console.Clear();
            WalletAccessSubmenu(targetWallet);
            break;
        case 0:
            return;
        default:
            Console.WriteLine("\nThere is no action for provided input!");
            break;
    }

} while (mainMenuChoice != 0);

//------ NEW CRYPTO WALLET ------
void CreateNewCryptoWallet()
{
    Console.Clear();

    PrintGeneralSectionSeparator("Create Wallet");
    Console.Write("Available wallets to create: Bitcoin, Ethereum or Solana\n"
        + "Enter the type of wallet you want to create: ");

    var wantedWallet = CapitalizeAndTrim(Console.ReadLine()) + "Wallet";

    Console.WriteLine("WANTED: " + wantedWallet);

    if(!Enum.IsDefined(typeof(CryptoWalletTypes), wantedWallet))
    {
        Console.WriteLine("\nWrong input for wallet type! Wallet can be of types:  Bitcoin, Ethereum or Solana");
        return;
    }

    Type walletType = Type.GetType(GeneralConstants.CLASS_PREFIX + wantedWallet);

    if (ConfirmChoice($"Are you sure you want to add new wallet of type {wantedWallet}?"))
        GlobalData.wallets.Add(Activator.CreateInstance(walletType) as CryptoWallet);
}

//------ ACCESS WALLET ------
void PrintAllWallets()
{
    //TODO migrate to class
    var table = new ConsoleTable("Wallet Type", "Wallet Address", "USD Asset Value", "Value Change");

    foreach (var wallet in GlobalData.wallets)
    {
        var value = GetValueFromWalletByType(wallet);

        table.AddRow(
            wallet.GetWalletType(),
            wallet.Address,
            $"$ {value}",
            wallet.GetValueChange(value)
        );

        wallet.AddValue(value);
    }

    table.Write(Format.Alternative);
    Console.WriteLine(GlobalData.wallets[0].Address);
    Console.WriteLine(GlobalData.wallets[4].Address);
}

double GetValueFromWalletByType(CryptoWallet wallet)
{
    return wallet.GetType().BaseType.Name == GeneralConstants.MAIN_TYPE
            ? wallet.CalculateFungibleValueInUSD()
            : ((CryptoAndNFTWallet)wallet).CalculateValueInUSD();
}
//TODO unify: input guid returns it if it is in the list
CryptoWallet GetWalletByAddress(string queryMessage, string rejectionMessage)
{
    var success = false;

    Console.Write(queryMessage);
    success = Guid.TryParse(Console.ReadLine(), out Guid walletAddress);

    if (!success)
    {
        Console.WriteLine("Input value needs to be a Guid!");
        return null;
    }

    var wallet = GlobalData.wallets.FirstOrDefault(wallet => wallet.Address.Equals(walletAddress));

    if(wallet == null)
        Console.WriteLine(rejectionMessage);

    return wallet;
}

void WalletAccessSubmenu(CryptoWallet targetWallet)
{
    var walletAccessSubmenu = 0;

    do
    {
        walletAccessSubmenu = GetMenuChoiceFromUser(MenuConstants.ACCESS_WALLET_MENU_TEXT, "Access Wallet");

        switch (walletAccessSubmenu)
        {
            case 1:
                Portfolio(targetWallet);
                break;
            case 2:
                Transfer(targetWallet);
                break;
            case 3:
                break;
            case 0:
                return;
            default:
                Console.WriteLine("\nThere is no action for provided input!");
                break;
        }

    } while (walletAccessSubmenu != 0);
}

void Transfer(CryptoWallet senderWallet)
{
    Console.WriteLine("TRANSFER HAPPENING");
    var receivingWallet = GetWalletByAddress(MessageConstants.ACCESS_WALLET_TRANSFER_MESSAGE, MessageConstants.ACESS_WALLET_FAILED_MESSAGE);
}

//TODO migrate to class? shorten
void Portfolio(CryptoWallet targetWallet)
{
    Console.WriteLine($"All asset value: $ {GetValueFromWalletByType(targetWallet)}");

    //------FUNGIBLE ASSETS ------
    PrintGeneralSectionSeparator("Fungible Assets");
    var fungibleAssetsTable = new ConsoleTable("Address", "Name", "Label", "Value (crypto)", "Total Value (USD)", "Value Change");

    foreach (var fungAsset in targetWallet.OwnedFungibleAssets)
    {
        var fungAssetObj = GlobalData.fungibleAssets.First(asset => asset.Address.Equals(fungAsset.Key));

        var values = targetWallet.CalculateFungibleAssetsValue(fungAssetObj);

        fungibleAssetsTable.AddRow(
            fungAssetObj.Address,
            fungAssetObj.Name,
            fungAssetObj.Label,
            values.cryptoValue,
            values.usdValue,
            fungAssetObj.CalculateValueChange(fungAssetObj.USDValue)
        );

        fungAssetObj.AddPastValue(fungAssetObj.USDValue);
    }

    fungibleAssetsTable.Write(Format.Alternative);

    if (targetWallet.GetType().BaseType.Name == GeneralConstants.MAIN_TYPE)
        return;

    //------NON FUNGIBLE ASSETS ------
    PrintGeneralSectionSeparator("Non Fungible Assets");
    var nonFungibleAssetsTable = new ConsoleTable("Address", "Name", "Value (crypto)", "Total Value (USD)", "Value Change");

    foreach (var nonFungAsset in (targetWallet as CryptoAndNFTWallet).OwnedNonFungibleAssets)
    {
        var nonFungAssetObj = GlobalData.nonFungibleAssets.First(asset => asset.Address.Equals(nonFungAsset));
        var belongingFungAsset = GlobalData.fungibleAssets.First(asset => asset.Address.Equals(nonFungAssetObj.FungibleAsset));

        var valueInCrypto = nonFungAssetObj.Value * belongingFungAsset.USDValue;

        var valueUSD = 0;

        nonFungibleAssetsTable.AddRow(
            nonFungAssetObj.Address,
            nonFungAssetObj.Name,
            $"{valueInCrypto} {belongingFungAsset.Label}",
            $"$ {nonFungAssetObj.GetValueInUSD()}",
            nonFungAssetObj.CalculateValueChange(valueInCrypto)
        );

        nonFungAssetObj.AddPastValue(valueUSD);
    }

    nonFungibleAssetsTable.Write(Format.Alternative);
}

//------ HELPER FUNCTIONS ------
#region Helpers
int GetMenuChoiceFromUser(string menuText, string menuTitle)
{
    var success = false;
    var choice = 0;

    do
    {
        PrintGeneralSectionSeparator(menuTitle);

        Console.WriteLine(menuText);
        Console.Write("Input your choice: ");
        success = int.TryParse(Console.ReadLine(), out choice);

        if (!success)
        {
            Console.Clear();
            Console.WriteLine("Value must be a number!\n");
        }

    } while (!success);

    return choice;
}



bool ConfirmChoice(string message = "Are you sure?")
{
    Console.Write($"\n{message} (y/n): ");
    return Console.ReadLine().Trim().ToLower() == "y";
}

void PrintGeneralSectionSeparator(string text)
{
    Console.WriteLine($"\n<<<---------- {text} ---------->>>\n");
}

string CapitalizeAndTrim(string input)
{
    input = input.Trim().ToLower();
    return input.First().ToString().ToUpper() + String.Join("", input.Skip(1));
}
#endregion

Console.ReadKey();