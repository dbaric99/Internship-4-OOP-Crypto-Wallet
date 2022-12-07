using System;
using Crypto_Wallet.Classes;
using Crypto_Wallet.Classes.Assets;

namespace Crypto_Wallet.Global.Data
{
    public class GlobalData
    {
        public static List<FungibleAsset> fungibleAssets = new List<FungibleAsset>()
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

        public static List<NonFungibleAsset> nonFungibleAssets = new List<NonFungibleAsset>()
        {
            new NonFungibleAsset("Mutant Ape Yacht Club #23140", 14.89, fungibleAssets[1].Address),
            new NonFungibleAsset("Bored Ape Yacht Club #3758", 74.5, fungibleAssets[1].Address),
            new NonFungibleAsset("Otherdeed for Otherside#46782", 1.38, fungibleAssets[1].Address),
            new NonFungibleAsset("Moonbirds #1446", 8.88, fungibleAssets[1].Address),
            new NonFungibleAsset("TUDSY #3793", 148.99, fungibleAssets[9].Address),
            new NonFungibleAsset("METASHARK #15", 69, fungibleAssets[9].Address),
            new NonFungibleAsset("Bold Badger #780", 7.38, fungibleAssets[2].Address),
            new NonFungibleAsset("ROGUE SHARKS #2064", 18.45, fungibleAssets[2].Address),
            new NonFungibleAsset("Bad Billy Goats #6345", 1, fungibleAssets[3].Address),
            new NonFungibleAsset("Agent SHIB", 16, fungibleAssets[3].Address),
            new NonFungibleAsset("Smokestack", 12.32, fungibleAssets[1].Address),
            new NonFungibleAsset("Flowers #1991", 10.45, fungibleAssets[1].Address),
            new NonFungibleAsset("Potatoz #3691", 1.73, fungibleAssets[1].Address),
            new NonFungibleAsset("Pudgy Penguin #6454", 3.25, fungibleAssets[1].Address),
            new NonFungibleAsset("RENGA #4197", 1.1557, fungibleAssets[1].Address),
            new NonFungibleAsset("Invisible Friends #2494", 2.19, fungibleAssets[1].Address),
            new NonFungibleAsset("Invisible Friends #3527", 2.35, fungibleAssets[1].Address),
            new NonFungibleAsset("WoW #1736", 2.1, fungibleAssets[2].Address),
            new NonFungibleAsset("Quirkies Originals #4240", 1.98, fungibleAssets[2].Address),
            new NonFungibleAsset("Meebit #13439", 2.91, fungibleAssets[1].Address)
        };

        public static List<CryptoWallet> wallets = new List<CryptoWallet>()
        {
            new BitcoinWallet(new Dictionary<Guid, double>(2)
            {
                { fungibleAssets[0].Address, 2.1 },
                { fungibleAssets[6].Address, 12.3 }
            }),

            new BitcoinWallet(new Dictionary<Guid, double>(2)
            {
                { fungibleAssets[0].Address, 0 },
                { fungibleAssets[6].Address, 2.3 }
            }),

            new BitcoinWallet(new Dictionary<Guid, double>(2)
            {
                { fungibleAssets[0].Address, 5.6 },
                { fungibleAssets[6].Address, 1.3 }
            }),

            new EthereumWallet(new Dictionary<Guid, double>(6)
            {
                { fungibleAssets[1].Address, 1.25 },
                { fungibleAssets[3].Address, 10 },
                { fungibleAssets[4].Address, 12 },
                { fungibleAssets[5].Address, 3.5 },
                { fungibleAssets[6].Address, 8 },
                { fungibleAssets[9].Address, 9.2 }
            }, new List<Guid>(4){
                nonFungibleAssets[0].Address,
                nonFungibleAssets[10].Address,
                nonFungibleAssets[14].Address,
                nonFungibleAssets[11].Address
            }),

            new EthereumWallet(new Dictionary<Guid, double>(6)
            {
                { fungibleAssets[1].Address, 0.3 },
                { fungibleAssets[3].Address, 2 },
                { fungibleAssets[4].Address, 0 },
                { fungibleAssets[5].Address, 0 },
                { fungibleAssets[6].Address, 0 },
                { fungibleAssets[9].Address, 17 }
            }, new List<Guid>(2){
                nonFungibleAssets[19].Address,
                nonFungibleAssets[2].Address
            }),

            new EthereumWallet(new Dictionary<Guid, double>(6)
            {
                { fungibleAssets[1].Address, 52.89 },
                { fungibleAssets[3].Address, 26 },
                { fungibleAssets[4].Address, 37.5 },
                { fungibleAssets[5].Address, 42.3 },
                { fungibleAssets[6].Address, 12.8 },
                { fungibleAssets[9].Address, 4 }
            }, new List<Guid>(6){
                nonFungibleAssets[15].Address,
                nonFungibleAssets[1].Address,
                nonFungibleAssets[3].Address,
                nonFungibleAssets[16].Address,
                nonFungibleAssets[13].Address,
                nonFungibleAssets[5].Address
            }),

            new SolanaWallet(new Dictionary<Guid, double>(4)
            {
                { fungibleAssets[2].Address, 14.5 },
                { fungibleAssets[3].Address, 8 },
                { fungibleAssets[7].Address, 25 },
                { fungibleAssets[8].Address, 34.7 }
            }, new List<Guid>(1)
            {
                nonFungibleAssets[18].Address
            }),

            new SolanaWallet(new Dictionary<Guid, double>(4)
            {
                { fungibleAssets[2].Address, 2 },
                { fungibleAssets[3].Address, 1 },
                { fungibleAssets[7].Address, 4 },
                { fungibleAssets[8].Address, 5 }
            }, new List<Guid>(3)
            {
                nonFungibleAssets[6].Address,
                nonFungibleAssets[9].Address,
                nonFungibleAssets[17].Address
            }),

            new SolanaWallet(new Dictionary<Guid, double>(4)
            {
                { fungibleAssets[2].Address, 2.5 },
                { fungibleAssets[3].Address, 3.2 },
                { fungibleAssets[7].Address, 10.8 },
                { fungibleAssets[8].Address, 52.1 }
            }, new List<Guid>(2)
            {
                nonFungibleAssets[7].Address,
                nonFungibleAssets[8].Address
            })
        };


        public GlobalData()
        {
            BitcoinWallet.SupportedFungibleAssets = new List<Guid>(2)
            {
                fungibleAssets[0].Address,
                fungibleAssets[6].Address
            };

            EthereumWallet.SupportedFungibleAssets = new List<Guid>(6)
            {
                fungibleAssets[1].Address,
                fungibleAssets[3].Address,
                fungibleAssets[4].Address,
                fungibleAssets[5].Address,
                fungibleAssets[6].Address,
                fungibleAssets[9].Address
            };

            EthereumWallet.SupportedNonFungibleAssets = new List<Guid>(14)
            {
                nonFungibleAssets[0].Address,
                nonFungibleAssets[1].Address,
                nonFungibleAssets[2].Address,
                nonFungibleAssets[3].Address,
                nonFungibleAssets[4].Address,
                nonFungibleAssets[5].Address,
                nonFungibleAssets[10].Address,
                nonFungibleAssets[11].Address,
                nonFungibleAssets[12].Address,
                nonFungibleAssets[13].Address,
                nonFungibleAssets[14].Address,
                nonFungibleAssets[15].Address,
                nonFungibleAssets[16].Address,
                nonFungibleAssets[19].Address
            };

            SolanaWallet.SupportedFungibleAssets = new List<Guid>(4)
            {
                fungibleAssets[2].Address,
                fungibleAssets[3].Address,
                fungibleAssets[7].Address,
                fungibleAssets[8].Address
            };

            SolanaWallet.SupportedNonFungibleAssets = new List<Guid>(6)
            {
                nonFungibleAssets[6].Address,
                nonFungibleAssets[7].Address,
                nonFungibleAssets[8].Address,
                nonFungibleAssets[9].Address,
                nonFungibleAssets[17].Address,
                nonFungibleAssets[18].Address
            };
        }
    }
}

