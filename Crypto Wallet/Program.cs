#region Dependencies
using Crypto_Wallet.Classes;
using Crypto_Wallet.Classes.Wallets;
using Crypto_Wallet.Classes.Assets;
using ConsoleTables;
using Crypto_Wallet.Global.Constants;
using Crypto_Wallet.Global.Data;
using Crypto_Wallet.Classes.Transactions;
using Crypto_Wallet.Helpers;
#endregion

//list of all transactions
var allTransactions = new List<Transaction>();

//------ MAIN MENU ------

while(true) {
    var mainMenuChoice = GeneralHelper.GetMenuChoiceFromUser(MenuConstants.MAIN_MENU_TEXT, "Main Menu");

    switch (mainMenuChoice)
    {
        case 1:
            CryptoWalletHelper.CreateNewCryptoWallet();
            break;
        case 2:
            Console.Clear();
            CryptoWalletHelper.PrintAllWallets();
            var targetWallet = CryptoWalletHelper.GetWalletByAddress(MessageConstants.ACCESS_WALLET_REQUEST_MESSAGE, MessageConstants.ACESS_WALLET_FAILED_MESSAGE);
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
}

//------ WALLET ACCESS SUBMENU ------

void WalletAccessSubmenu(CryptoWallet targetWallet)
{
    while(true) {
        var walletAccessSubmenu = GeneralHelper.GetMenuChoiceFromUser(MenuConstants.ACCESS_WALLET_MENU_TEXT, "Access Wallet");

        switch (walletAccessSubmenu)
        {
            case 1:
                Portfolio(targetWallet);
                break;
            case 2:
                if (Transfer(targetWallet))
                    Console.WriteLine("\nAsset transfer successful!");
                else
                    Console.WriteLine("\nAsset transfer failed!");
                break;
            case 3:
                TransactionHistory(targetWallet);
                break;
            case 0:
                return;
            default:
                Console.WriteLine("\nThere is no action for provided input!");
                break;
        }

    }
}

bool Transfer(CryptoWallet senderWallet)
{
    CryptoWalletHelper.PrintAllWallets();

    var receivingWallet = CryptoWalletHelper.GetWalletByAddress(MessageConstants.ACCESS_WALLET_TRANSFER_MESSAGE, MessageConstants.ACESS_WALLET_FAILED_MESSAGE);

    if (receivingWallet == null) return false;

    if (senderWallet.Equals(receivingWallet))
    {
        Console.WriteLine("\nSender and receiver wallet cannot be the same!");
        return false;
    }

    Portfolio(senderWallet);

    var assetToSend = GetAssetFromWalletByGuid(senderWallet);

    if (assetToSend == null) return false;

    if (assetToSend.GetType().Name == GeneralConstants.FUNGIBLE_ASSET_TYPE)
        return HandleFungibleAssetTransaction(senderWallet, receivingWallet, (FungibleAsset)assetToSend);
    else
        return HandleNonFungibleAssetTransaction(senderWallet, receivingWallet, (NonFungibleAsset)assetToSend);
}

Asset GetAssetFromWalletByGuid(CryptoWallet wallet)
{
    var assetAddress = GeneralHelper.GetGuidFromUserInput(MessageConstants.ASSET_TO_TRANSFER_REQUEST_MESSAGE);

    var asset = (Asset?)GlobalData.fungibleAssets.FirstOrDefault(asset => asset.Address.Equals(assetAddress)) ??
                GlobalData.nonFungibleAssets.FirstOrDefault(asset => asset.Address.Equals(assetAddress));

    if(asset == null)
        Console.WriteLine("There is no asset with that address!");

    if (!wallet.SupportsNFT())
    {
        if (!wallet.GetSupportedFungibleAssets().Contains(assetAddress))
        {
            Console.WriteLine("\nAsset not supported!");
            Console.WriteLine("Supported assets for the wallet type are: ");
            foreach (var fungAsset in wallet.GetSupportedFungibleAssets())
            {
                Console.WriteLine($"\t{fungAsset} - {GlobalData.fungibleAssets.FirstOrDefault(asset => asset.Address.Equals(fungAsset)).Name}");
                return null;
            }
        }
    }
    else
    {
        if (!wallet.GetSupportedFungibleAssets().Contains(assetAddress) && asset.GetType().Name == GeneralConstants.FUNGIBLE_ASSET_TYPE)
        {
            Console.WriteLine("\nAsset not supported!");
            Console.WriteLine("Supported fungible assets for the wallet type are: ");
            foreach (var fungAsset in wallet.GetSupportedFungibleAssets())
            {
                Console.WriteLine($"\t{fungAsset} - {GlobalData.fungibleAssets.FirstOrDefault(asset => asset.Address.Equals(fungAsset)).Name}");
                return null;
            }
        }
        else if (!(wallet as CryptoAndNFTWallet).GetSupportedNonFungibleAssets().Contains(assetAddress) && !(asset.GetType().Name == GeneralConstants.FUNGIBLE_ASSET_TYPE))
        {
            Console.WriteLine("\nAsset not supported!");
            Console.WriteLine("Supported non fungible assets for the wallet type are: ");
            foreach (var nonFungAsset in (wallet as CryptoAndNFTWallet).GetSupportedNonFungibleAssets())
            {
                Console.WriteLine($"\t{nonFungAsset} - {GlobalData.nonFungibleAssets.FirstOrDefault(asset => asset.Address.Equals(nonFungAsset)).Name}");
                return null;
            }
        }
    }

    return asset;
}

Transaction GetTransactionByGuid(CryptoWallet currentWallet)
{
    var transactionId = GeneralHelper.GetGuidFromUserInput(MessageConstants.TRANSACTION_ID_REQUEST_MESSAGE);

    var transaction = allTransactions.FirstOrDefault(trans => trans.Id.Equals(transactionId));

    if (transaction == null)
        Console.WriteLine("\nThere is no transaction by the required id!");

    else if (currentWallet.Transactions.FirstOrDefault(trans => trans.Equals(transaction.Id)) == null)
    {
        Console.WriteLine("\nYou cannot revoke a transaction that you didn't create!");
        return null;
    }

    return transaction;
}

void TransactionHistory(CryptoWallet targetWallet)
{
    var transactionsForWallet = targetWallet.AllTransactionsOrderedByDate(allTransactions);

    GeneralHelper.PrintGeneralSectionSeparator("Transaction history");
    var transactionTable = new ConsoleTable("Transaction Id", "Date and time", "Sending wallet address", "Receiving wallet address", "Amount (fungible)", "Asset name", "Revoked");

    foreach (var trans in transactionsForWallet)
    {
        if (!trans.IsNonFungible())
        {
            transactionTable.AddRow(
                trans.Id,
                trans.Date,
                trans.Sender,
                trans.Receiver,
                (trans as FungibleAssetTransaction).ReceiverEndBalance - (trans as FungibleAssetTransaction).ReceiverStartBalance,
                trans.GetFungibleAssetName(),
                trans.isRevoked
            );
        }
        else
        {
            transactionTable.AddRow(
                trans.Id,
                trans.Date,
                trans.Sender,
                trans.Receiver,
                "-",
                (trans as NonFungibleAssetTransaction).GetNonFungibleAssetName(),
                trans.isRevoked
            );
        }
    }

    transactionTable.Write(Format.Alternative);

    RevokeTransaction(targetWallet);
}

void RevokeTransaction(CryptoWallet senderWallet)
{
    var transaction = GetTransactionByGuid(senderWallet);

    if (transaction == null) return;

    if(transaction.isRevoked)
    {
        Console.WriteLine("\nThis transaction is already revoked!");
        return;
    }

    if(DateTime.Now.Subtract(transaction.Date).TotalSeconds > 45)
    {
        Console.WriteLine("\nYou cannot revoke a transaction that is created more than 45 seconds ago!");
        return;
    }

    transaction.Revoke();

    if (!transaction.IsNonFungible())
    {
        var fungAssetTransaction = (FungibleAssetTransaction)transaction;
        senderWallet.FungibleValueManipulation(fungAssetTransaction.FungibleAsset, fungAssetTransaction.ReceiverEndBalance - fungAssetTransaction.ReceiverStartBalance, true);

        var receivingWallet = GlobalData.wallets.First(wallet => wallet.Address.Equals(transaction.Receiver));

        receivingWallet.FungibleValueManipulation(fungAssetTransaction.FungibleAsset, fungAssetTransaction.ReceiverEndBalance - fungAssetTransaction.ReceiverStartBalance, false);

        return;
    }

    Console.WriteLine("\nTransaction successfully revoked!");
}

bool HandleFungibleAssetTransaction(CryptoWallet senderWallet, CryptoWallet receiverWallet, FungibleAsset sendingCrypto)
{
    if(!receiverWallet.GetSupportedFungibleAssets().Contains(sendingCrypto.Address))
    {
        Console.WriteLine("\nReceiving wallet doesn't support that asset type!");
        return false;
    }

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

    if (!GeneralHelper.ConfirmChoice(MessageConstants.TRANSACTION_CONFORMATION_MESSAGE))
        return false;

    allTransactions.Add(TransactionHelper.FungibleTransaction(senderWallet, receiverWallet, sendingCrypto, amount));

    return true;
}

bool HandleNonFungibleAssetTransaction(CryptoWallet senderWallet, CryptoWallet receiverWallet, NonFungibleAsset sendingNFT)
{
    if (!receiverWallet.SupportsNFT())
    {
        Console.WriteLine("\nReceiving wallet doesn't support non fungible assets!");
        return false;
    }

    var supportsAsset = receiverWallet.GetWalletType() == "SolanaWallet"
        ? (receiverWallet as SolanaWallet).GetSupportedNonFungibleAssets().Contains(sendingNFT.Address)
        : (receiverWallet as EthereumWallet).GetSupportedNonFungibleAssets().Contains(sendingNFT.Address);

    if (!supportsAsset)
    {
        Console.WriteLine("\nReceiving wallet doesn't support that asset type!");
        return false;
    }

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

    allTransactions.Add(TransactionHelper.NonFungibleTransaction(senderWallet as CryptoAndNFTWallet, receiverWallet as CryptoAndNFTWallet, sendingNFT));

    return true;
}

void Portfolio(CryptoWallet targetWallet)
{
    Console.WriteLine($"All asset value: $ {CryptoWalletHelper.GetValueFromWalletByType(targetWallet)}");

    //------FUNGIBLE ASSETS ------
    GeneralHelper.PrintGeneralSectionSeparator("Fungible Assets");
    var fungibleAssetsTable = new ConsoleTable("Address", "Name", "Label", "Value (crypto)", "Total Value (USD)", "Value Change");

    foreach (var fungAsset in targetWallet.OwnedFungibleAssets)
    {
        var fungAssetObj = GlobalData.fungibleAssets.First(asset => asset.Address.Equals(fungAsset.Key));

        var values = targetWallet.CalculateFungibleAssetsValue(fungAssetObj);

        var valueInUSD = double.Parse(values.usdValue.Replace("$", "").Trim());
        
        fungAssetObj.AddPastValue(valueInUSD);

        fungibleAssetsTable.AddRow(
            fungAssetObj.Address,
            fungAssetObj.Name,
            fungAssetObj.Label,
            values.cryptoValue,
            values.usdValue,
            CryptoWalletHelper.GetValueChange(fungAssetObj.PastValues)
        );
    }

    fungibleAssetsTable.Write(Format.Alternative);

    if (!targetWallet.SupportsNFT())
        return;

    //------NON FUNGIBLE ASSETS ------
    GeneralHelper.PrintGeneralSectionSeparator("Non Fungible Assets");
    var nonFungibleAssetsTable = new ConsoleTable("Address", "Name", "Value (crypto)", "Total Value (USD)", "Value Change");

    foreach (var nonFungAsset in (targetWallet as CryptoAndNFTWallet).OwnedNonFungibleAssets)
    {
        var nonFungAssetObj = GlobalData.nonFungibleAssets.First(asset => asset.Address.Equals(nonFungAsset));
        var belongingFungAsset = GlobalData.fungibleAssets.First(asset => asset.Address.Equals(nonFungAssetObj.FungibleAsset));

        var valueInUSD = nonFungAssetObj.Value * belongingFungAsset.USDValue;
        
        nonFungAssetObj.AddPastValue(valueInUSD);

        nonFungibleAssetsTable.AddRow(
            nonFungAssetObj.Address,
            nonFungAssetObj.Name,
            $"{nonFungAssetObj.Value} {belongingFungAsset.Label}",
            $"$ {nonFungAssetObj.GetValueInUSD()}",
            CryptoWalletHelper.GetValueChange(nonFungAssetObj.PastValues)
        );
    }

    nonFungibleAssetsTable.Write(Format.Alternative);
}