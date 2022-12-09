using Crypto_Wallet.Classes.Wallets;

namespace Crypto_Wallet.Classes
{
	public class EthereumWallet : CryptoAndNFTWallet
    {
		public EthereumWallet() : base() { }

        public EthereumWallet(Dictionary<Guid, double> ownedFungibleAssets, List<Guid> ownedNonFungibleAssets, List<Guid>? supportedFungibleAssets, List<Guid>? supportedNonFungableAssets) : base(ownedFungibleAssets, ownedNonFungibleAssets, supportedFungibleAssets, supportedNonFungableAssets) { }
    }
}

