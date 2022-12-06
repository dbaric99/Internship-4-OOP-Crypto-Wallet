using System;
using Crypto_Wallet.Classes.Wallets;

namespace Crypto_Wallet.Classes
{
	public class EthereumWallet : CryptoAndNFTWallet
    {
		public EthereumWallet() : base() { }

        public EthereumWallet(Dictionary<Guid, double> ownedFungibleAssets, List<Guid> ownedNonFungibleAssets) : base(ownedFungibleAssets, ownedNonFungibleAssets) { }
    }
}

