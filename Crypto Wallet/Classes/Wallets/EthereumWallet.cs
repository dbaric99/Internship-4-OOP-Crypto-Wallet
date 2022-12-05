using System;
namespace Crypto_Wallet.Classes
{
	public class EthereumWallet : CryptoWallet
	{
		public List<Guid> OwnedNonFungibleAssets { get; set; } = new List<Guid>();
		public static List<Guid>? SupportedNonFungibleAssets { get; set; }

		public EthereumWallet() : base()
		{
		}
	}
}

