using System;
namespace Crypto_Wallet.Classes
{
	public class EthereumWallet : CryptoWallet
	{
		public List<Guid> OwnedNonFungibleAssets { get; set; } = new List<Guid>();
		//TODO same for all, add them and make it readonly
		public List<Guid> SupportedNonFungibleAssets { get; } = new List<Guid>();

		public EthereumWallet() : base()
		{
		}
	}
}

