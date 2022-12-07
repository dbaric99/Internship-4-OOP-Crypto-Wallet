using Crypto_Wallet.Classes;
using Crypto_Wallet.Classes.Wallets;
using Crypto_Wallet.Classes.Assets;
using Crypto_Wallet.Enums;
using ConsoleTables;
using Crypto_Wallet.Interfaces;
using Crypto_Wallet.Global.Constants;
using Crypto_Wallet.Global.Data;
using Crypto_Wallet.Classes.Transactions;

var globalData = new GlobalData();
var allTransactions = new List<Transaction>();

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
    return !wallet.SupportsNFT()
            ? wallet.CalculateFungibleValueInUSD()
            : ((CryptoAndNFTWallet)wallet).CalculateValueInUSD();
}

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

Asset GetAssetFromWalletByGuid(CryptoWallet wallet)
{
    var success = false;

    Console.Write("\nInput address of asset you are sending: ");
    success = Guid.TryParse(Console.ReadLine(), out Guid assetAddress);

    if (!success)
    {
        Console.WriteLine("Input value needs to be a Guid!");
        return null;
    }

    var asset = (Asset)(GlobalData.fungibleAssets.First(asset => asset.Address.Equals(assetAddress)) != null
        ? GlobalData.fungibleAssets.FirstOrDefault(asset => asset.Address.Equals(assetAddress))
        : GlobalData.nonFungibleAssets.FirstOrDefault(asset => asset.Address.Equals(assetAddress)));

    if(asset == null)
        Console.WriteLine("There is no asset with that address!");

    if (!wallet.SupportsNFT())
    {
        if (!CryptoWallet.SupportedFungibleAssets.Contains(assetAddress))
        {
            Console.WriteLine("\nAsset not supported!");
            Console.WriteLine("Supported assets for the wallet type are: ");
            foreach (var fungAsset in CryptoWallet.SupportedFungibleAssets)
            {
                Console.WriteLine($"\t{fungAsset} - {GlobalData.fungibleAssets.FirstOrDefault(asset => asset.Address.Equals(fungAsset)).Name}");
                return null;
            }
        }
    }

    if (!CryptoAndNFTWallet.SupportedNonFungibleAssets.Contains(assetAddress))
    {
        Console.WriteLine("\nAsset not supported!");
        Console.WriteLine("Supported assets for the wallet type are: ");
        foreach (var nonFungAsset in CryptoAndNFTWallet.SupportedNonFungibleAssets)
        {
            Console.WriteLine($"\t{nonFungAsset} - {GlobalData.nonFungibleAssets.FirstOrDefault(asset => asset.Address.Equals(nonFungAsset)).Name}");
            return null;
        }
    }

    return asset;
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
                if(Transfer(targetWallet))
                    Console.WriteLine("\nAsset transfer successful!");
                else
                    Console.WriteLine("\nAsset transfer failed!");
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

bool Transfer(CryptoWallet senderWallet)
{
    var receivingWallet = GetWalletByAddress(MessageConstants.ACCESS_WALLET_TRANSFER_MESSAGE, MessageConstants.ACESS_WALLET_FAILED_MESSAGE);

    if (senderWallet.Equals(receivingWallet))
    {
        Console.WriteLine("\nSender and receiver wallet cannot be the same!");
        return false;
    }

    var assetToSend = GetAssetFromWalletByGuid(senderWallet);

    if (assetToSend == null) return false;

    if (assetToSend.GetType().BaseType.Name == GeneralConstants.FUNGIBLE_ASSET_TYPE)
        return HandleFungibleAssetTransaction(senderWallet, receivingWallet, (FungibleAsset)assetToSend);
    else
        return HandleNonFungibleAssetTransaction(senderWallet, receivingWallet, (NonFungibleAsset)assetToSend);
}

bool HandleFungibleAssetTransaction(CryptoWallet senderWallet, CryptoWallet receiverWallet, FungibleAsset sendingCrypto)
{
    //TODO check if wallets support the type

    var success = false;

    Console.Write("\nInsert amount of fungible asset you are sending: ");
    success = double.TryParse(Console.ReadLine(), out double amount);

    if (!success)
    {
        Console.WriteLine("\nAmount must be a number!");
        return false;
    }
    else if (senderWallet.OwnedFungibleAssets[sendingCrypto.Address] < amount)
    {
        Console.WriteLine("\nBalance insufficient!");
        return false;
    }

    if (!ConfirmChoice(MessageConstants.TRANSACTION_CONFORMATION_MESSAGE))
        return false;

    //TODO maybe add this logic to the class when adding new transaction
    var senderStartBalance = senderWallet.GetFungibleValue(sendingCrypto.Address);

    var receiverStartBalance = receiverWallet.GetFungibleValue(sendingCrypto.Address);

    var senderEndBalance = senderWallet.FungibleValueManipulation(sendingCrypto.Address, amount, false);

    var receiverEndBalance = receiverWallet.FungibleValueManipulation(sendingCrypto.Address, amount, true);

    var transactionInProgress = new FungibleAssetTransaction(senderWallet.Address, receiverWallet.Address, sendingCrypto.Address, senderStartBalance, senderEndBalance, receiverStartBalance, receiverEndBalance);

    sendingCrypto.ChangeAssetValue();

    senderWallet.AddTransaction(transactionInProgress.Id);
    receiverWallet.AddTransaction(transactionInProgress.Id);

    allTransactions.Add(transactionInProgress);

    return true;
}

bool HandleNonFungibleAssetTransaction(CryptoWallet senderWallet, CryptoWallet receiverWallet, NonFungibleAsset sendingNFT)
{
    //TODO check if wallets support the type
    if (!senderWallet.SupportsNFT())
    {
        Console.WriteLine("\nYour wallet type doesn't support non fungible assets!");
        return false;
    }
    else if (!receiverWallet.SupportsNFT())
    {
        Console.WriteLine("\nYou cannot send non fungible assets to a wallet that doesn't support them!");
        return false;
    }
    else if((senderWallet as CryptoAndNFTWallet).OwnedNonFungibleAssets.FirstOrDefault(asset => asset.Equals(sendingNFT.Address)) == null)
    {
        Console.WriteLine("\nYou do not own that specific non fungible asset!");
        return false;
    }

    //TODO migrate
    var transactionInProgress = new NonFungibleAssetTransaction(senderWallet.Address, receiverWallet.Address, sendingNFT.Address);

    senderWallet.AddTransaction(transactionInProgress.Id);
    receiverWallet.AddTransaction(transactionInProgress.Id);

    (senderWallet as CryptoAndNFTWallet).SendNonFungibleAsset(sendingNFT.Address);
    (receiverWallet as CryptoAndNFTWallet).ReceiveNonFungibleAsset(sendingNFT.Address);

    return true;
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

    if (!targetWallet.SupportsNFT())
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