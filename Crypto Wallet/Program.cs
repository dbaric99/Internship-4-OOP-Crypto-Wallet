using Crypto_Wallet.Classes;
using Crypto_Wallet.Classes.Wallets;
using Crypto_Wallet.Classes.Assets;
using Crypto_Wallet.Enums;
using ConsoleTables;
using Crypto_Wallet.Interfaces;

//TODO: Put them in separate files
#region Constants

const string MAIN_MENU_TEXT =
    "1 - Create a wallet\n"
    + "2 - Access a wallet\n"
    + "0 - Exit Application\n";

const string ACCESS_WALLET_MENU_TEXT =
    "1 - Portfolio\n"
    + "2 - Transfer assets\n"
    + "3 - Transaction history\n"
    + "0 - Return to main menu\n";

const string CLASS_PREFIX = "Crypto_Wallet.Classes.";

const string MAIN_TYPE = "CryptoWallet";

#endregion

#region GlobalProgramProperties

var g_fungibleAssets = new List<FungibleAsset>()
{
    new FungibleAsset("Bitcoin", "BTC", 17108.30),
    new FungibleAsset("Ethereum", "ETH", 1277.60),
    new FungibleAsset("Solana", "SOL", 13.55),
    new FungibleAsset("Tether", "USDT", 1),
    new FungibleAsset("Uniswap", "UNI", 6.13605),
    new FungibleAsset("Chainlink", "LINK", 7.31349),
    new FungibleAsset("Wrapped Bitcoin", "WBTC", 16919.91),
    new FungibleAsset("Axie Infinity", "AXS", 6.75043),
    new FungibleAsset("Mango", "MNGO", 0.02327),
    new FungibleAsset("Binance", "BNB", 2089.05)
};

var g_nonFungibleAssets = new List<NonFungibleAsset>()
{
    new NonFungibleAsset("Mutant Ape Yacht Club #23140", 14.89, g_fungibleAssets[1].Address), 
    new NonFungibleAsset("Bored Ape Yacht Club #3758", 74.5, g_fungibleAssets[1].Address), 
    new NonFungibleAsset("Otherdeed for Otherside#46782", 1.38, g_fungibleAssets[1].Address), 
    new NonFungibleAsset("Moonbirds #1446", 8.88, g_fungibleAssets[1].Address),
    new NonFungibleAsset("TUDSY #3793", 148.99, g_fungibleAssets[9].Address),
    new NonFungibleAsset("METASHARK #15", 69, g_fungibleAssets[9].Address),
    new NonFungibleAsset("Bold Badger #780", 7.38, g_fungibleAssets[2].Address),
    new NonFungibleAsset("ROGUE SHARKS #2064", 18.45, g_fungibleAssets[2].Address),
    new NonFungibleAsset("Bad Billy Goats #6345", 1, g_fungibleAssets[3].Address),
    new NonFungibleAsset("Agent SHIB", 16, g_fungibleAssets[3].Address),
    new NonFungibleAsset("Smokestack", 12.32, g_fungibleAssets[1].Address),
    new NonFungibleAsset("Flowers #1991", 10.45, g_fungibleAssets[1].Address),
    new NonFungibleAsset("Potatoz #3691", 1.73, g_fungibleAssets[1].Address),
    new NonFungibleAsset("Pudgy Penguin #6454", 3.25, g_fungibleAssets[1].Address),
    new NonFungibleAsset("RENGA #4197", 1.1557, g_fungibleAssets[1].Address),
    new NonFungibleAsset("Invisible Friends #2494", 2.19, g_fungibleAssets[1].Address),
    new NonFungibleAsset("Invisible Friends #3527", 2.35, g_fungibleAssets[1].Address),
    new NonFungibleAsset("WoW #1736", 2.1, g_fungibleAssets[2].Address),
    new NonFungibleAsset("Quirkies Originals #4240", 1.98, g_fungibleAssets[2].Address),
    new NonFungibleAsset("Meebit #13439", 2.91, g_fungibleAssets[1].Address)
};

BitcoinWallet.SupportedFungibleAssets = new List<Guid>(2)
{
    g_fungibleAssets[0].Address,
    g_fungibleAssets[6].Address
};

EthereumWallet.SupportedFungibleAssets = new List<Guid>(6)
{
    g_fungibleAssets[1].Address,
    g_fungibleAssets[3].Address,
    g_fungibleAssets[4].Address,
    g_fungibleAssets[5].Address,
    g_fungibleAssets[6].Address,
    g_fungibleAssets[9].Address
};

EthereumWallet.SupportedNonFungibleAssets = new List<Guid>(14)
{
    g_nonFungibleAssets[0].Address,
    g_nonFungibleAssets[1].Address,
    g_nonFungibleAssets[2].Address,
    g_nonFungibleAssets[3].Address,
    g_nonFungibleAssets[4].Address,
    g_nonFungibleAssets[5].Address,
    g_nonFungibleAssets[10].Address,
    g_nonFungibleAssets[11].Address,
    g_nonFungibleAssets[12].Address,
    g_nonFungibleAssets[13].Address,
    g_nonFungibleAssets[14].Address,
    g_nonFungibleAssets[15].Address,
    g_nonFungibleAssets[16].Address,
    g_nonFungibleAssets[19].Address
};

SolanaWallet.SupportedFungibleAssets = new List<Guid>(4)
{
    g_fungibleAssets[2].Address,
    g_fungibleAssets[3].Address,
    g_fungibleAssets[7].Address,
    g_fungibleAssets[8].Address
};

SolanaWallet.SupportedNonFungibleAssets = new List<Guid>(6)
{
    g_nonFungibleAssets[6].Address,
    g_nonFungibleAssets[7].Address,
    g_nonFungibleAssets[8].Address,
    g_nonFungibleAssets[9].Address,
    g_nonFungibleAssets[17].Address,
    g_nonFungibleAssets[18].Address
};

var g_wallets = new List<CryptoWallet>()
{
    new BitcoinWallet(new Dictionary<Guid, double>(2)
    {
        { g_fungibleAssets[0].Address, 2.1 },
        { g_fungibleAssets[6].Address, 12.3 }
    }),

    new BitcoinWallet(new Dictionary<Guid, double>(2)
    {
        { g_fungibleAssets[0].Address, 0 },
        { g_fungibleAssets[6].Address, 2.3 }
    }),

    new BitcoinWallet(new Dictionary<Guid, double>(2)
    {
        { g_fungibleAssets[0].Address, 5.6 },
        { g_fungibleAssets[6].Address, 1.3 }
    }),

    new EthereumWallet(new Dictionary<Guid, double>(6)
    {
        { g_fungibleAssets[1].Address, 1.25 },
        { g_fungibleAssets[3].Address, 10 },
        { g_fungibleAssets[4].Address, 12 },
        { g_fungibleAssets[5].Address, 3.5 },
        { g_fungibleAssets[6].Address, 8 },
        { g_fungibleAssets[9].Address, 9.2 }
    }, new List<Guid>(4){
        g_nonFungibleAssets[0].Address,
        g_nonFungibleAssets[10].Address,
        g_nonFungibleAssets[14].Address,
        g_nonFungibleAssets[11].Address
    }),

    new EthereumWallet(new Dictionary<Guid, double>(6)
    {
        { g_fungibleAssets[1].Address, 0.3 },
        { g_fungibleAssets[3].Address, 2 },
        { g_fungibleAssets[4].Address, 0 },
        { g_fungibleAssets[5].Address, 0 },
        { g_fungibleAssets[6].Address, 0 },
        { g_fungibleAssets[9].Address, 17 }
    }, new List<Guid>(2){
        g_nonFungibleAssets[19].Address,
        g_nonFungibleAssets[2].Address
    }),

    new EthereumWallet(new Dictionary<Guid, double>(6)
    {
        { g_fungibleAssets[1].Address, 52.89 },
        { g_fungibleAssets[3].Address, 26 },
        { g_fungibleAssets[4].Address, 37.5 },
        { g_fungibleAssets[5].Address, 42.3 },
        { g_fungibleAssets[6].Address, 12.8 },
        { g_fungibleAssets[9].Address, 4 }
    }, new List<Guid>(6){
        g_nonFungibleAssets[15].Address,
        g_nonFungibleAssets[1].Address,
        g_nonFungibleAssets[3].Address,
        g_nonFungibleAssets[16].Address,
        g_nonFungibleAssets[13].Address,
        g_nonFungibleAssets[5].Address
    }),

    new SolanaWallet(new Dictionary<Guid, double>(4)
    {
        { g_fungibleAssets[2].Address, 14.5 },
        { g_fungibleAssets[3].Address, 8 },
        { g_fungibleAssets[7].Address, 25 },
        { g_fungibleAssets[8].Address, 34.7 }
    }, new List<Guid>(1)
    {
        g_nonFungibleAssets[18].Address
    }),

    new SolanaWallet(new Dictionary<Guid, double>(4)
    {
        { g_fungibleAssets[2].Address, 2 },
        { g_fungibleAssets[3].Address, 1 },
        { g_fungibleAssets[7].Address, 4 },
        { g_fungibleAssets[8].Address, 5 }
    }, new List<Guid>(3)
    {
        g_nonFungibleAssets[6].Address,
        g_nonFungibleAssets[9].Address,
        g_nonFungibleAssets[17].Address
    }),

    new SolanaWallet(new Dictionary<Guid, double>(4)
    {
        { g_fungibleAssets[2].Address, 2.5 },
        { g_fungibleAssets[3].Address, 3.2 },
        { g_fungibleAssets[7].Address, 10.8 },
        { g_fungibleAssets[8].Address, 52.1 }
    }, new List<Guid>(2)
    {
        g_nonFungibleAssets[7].Address,
        g_nonFungibleAssets[8].Address
    })
};

#endregion

//------ MAIN MENU ------
var mainMenuChoice = 0;

do
{
    mainMenuChoice = GetMenuChoiceFromUser(MAIN_MENU_TEXT, "Main Menu");

    switch (mainMenuChoice)
    {
        case 1:
            CreateNewCryptoWallet();
            break;
        case 2:
            Console.Clear();
            PrintAllWallets();
            var targetWallet = GetWalletByAddress();
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

    Type walletType = Type.GetType(CLASS_PREFIX + wantedWallet);

    if (ConfirmChoice($"Are you sure you want to add new wallet of type {wantedWallet}?"))
        g_wallets.Add(Activator.CreateInstance(walletType) as CryptoWallet);
}

//------ ACCESS WALLET ------
void PrintAllWallets()
{
    //TODO migrate to class
    var table = new ConsoleTable("Wallet Type", "Wallet Address", "USD Asset Value", "Value Change");

    foreach (var wallet in g_wallets)
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
    Console.Write(g_wallets[0].Address);
}

double GetValueFromWalletByType(CryptoWallet wallet)
{
    return wallet.GetType().BaseType.Name == MAIN_TYPE
            ? wallet.CalculateFungibleValueInUSD(g_fungibleAssets)
            : ((CryptoAndNFTWallet)wallet).CalculateValueInUSD(g_fungibleAssets, g_nonFungibleAssets);
}

CryptoWallet GetWalletByAddress()
{
    var success = false;

    Console.Write("\nInput the address of a wallet you want to access: ");
    success = Guid.TryParse(Console.ReadLine(), out Guid walletAddress);

    if (!success)
    {
        Console.WriteLine("Input value needs to be a Guid!");
        return null;
    }

    var wallet = g_wallets.FirstOrDefault(wallet => wallet.Address.Equals(walletAddress));

    if(wallet == null)
        Console.WriteLine("There is no wallet by the target guid!");

    return wallet;
}

void WalletAccessSubmenu(CryptoWallet targetWallet)
{
    var walletAccessSubmenu = 0;

    do
    {
        walletAccessSubmenu = GetMenuChoiceFromUser(ACCESS_WALLET_MENU_TEXT, "Access Wallet");

        switch (walletAccessSubmenu)
        {
            case 1:
                Portfolio(targetWallet);
                break;
            case 2:
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

//TODO migrate to class? shorten
void Portfolio(CryptoWallet targetWallet)
{
    Console.WriteLine($"All asset value: $ {GetValueFromWalletByType(targetWallet)}");

    //------FUNGIBLE ASSETS ------
    PrintGeneralSectionSeparator("Fungible Assets");
    var fungibleAssetsTable = new ConsoleTable("Address", "Name", "Label", "Value (crypto)", "Total Value (USD)", "Value Change");
    var nonFungibleAssetsTable = new ConsoleTable("Address", "Name", "Label", "Value (crypto)", "Total Value (USD)", "Value Change");

    foreach (var fungAsset in targetWallet.OwnedFungibleAssets)
    {
        var fungAssetObj = g_fungibleAssets.First(asset => asset.Address.Equals(fungAsset.Key));

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

    //------NON FUNGIBLE ASSETS ------
    PrintGeneralSectionSeparator("Non Fungible Assets");
    nonFungibleAssetsTable.Write(Format.Alternative);
}

//------ HELPER FUNCTIONS ------
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

Console.ReadKey();