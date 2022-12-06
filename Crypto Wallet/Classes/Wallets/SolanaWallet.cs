using System;
using Crypto_Wallet.Classes.Wallets;

namespace Crypto_Wallet.Classes
{
    public class SolanaWallet : CryptoAndNFTWallet
    {
        public SolanaWallet() : base() { }

        public SolanaWallet(Dictionary<Guid, double> ownedFungibleAssets, List<Guid> ownedNonFungibleAssets) : base(ownedFungibleAssets, ownedNonFungibleAssets) { }
    }
}

