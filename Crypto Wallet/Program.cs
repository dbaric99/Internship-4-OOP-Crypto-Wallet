using Crypto_Wallet.Classes;
using Crypto_Wallet.Classes.Assets;

#region Constants

const string MAIN_MENU_TEXT =
    "1 - Create a wallet\n"
    + "2 - Access a wallet\n"
    + "3 - Exit Application\n\n";

const string CREATE_WALLET_MENU_TEXT =
    "1 - Create a Bitcoin wallet\n"
    + "2 - Create an Ethereum wallet\n"
    + "3 - Create a Solana wallet\n"
    + "0 - Return to main menu\n\n";

const string ACCESS_WALLET_MENU_TEXT =
    "1 - Portfolio\n"
    + "2 - Transfer assets\n"
    + "3 - Transaction history\n"
    + "0 - Return to main menu\n\n";

#endregion

#region InitialValues

var wallets = new List<CryptoWallet>()
{
    new BitcoinWallet(),
    new BitcoinWallet(),
    new BitcoinWallet(),
    new EthereumWallet(),
    new EthereumWallet(),
    new EthereumWallet(),
    new SolanaWallet(),
    new SolanaWallet(),
    new SolanaWallet()
};

var fungibleAssets = new List<FungibleAsset>()
{
    new FungibleAsset("Bitcoin", "BTC",17108.30),
    new FungibleAsset(),
    new FungibleAsset(),
    new FungibleAsset(),
    new FungibleAsset(),
    new FungibleAsset(),
    new FungibleAsset(),
    new FungibleAsset(),
    new FungibleAsset(),
    new FungibleAsset()
};

var nonFungibleAssets = new List<NonFungibleAsset>()
{
    new NonFungibleAsset(),
    new NonFungibleAsset(),
    new NonFungibleAsset(),
    new NonFungibleAsset(),
    new NonFungibleAsset(),
    new NonFungibleAsset(),
    new NonFungibleAsset(),
    new NonFungibleAsset(),
    new NonFungibleAsset(),
    new NonFungibleAsset(),
    new NonFungibleAsset(),
    new NonFungibleAsset(),
    new NonFungibleAsset(),
    new NonFungibleAsset(),
    new NonFungibleAsset(),
    new NonFungibleAsset(),
    new NonFungibleAsset(),
    new NonFungibleAsset(),
    new NonFungibleAsset(),
    new NonFungibleAsset()
};

#endregion