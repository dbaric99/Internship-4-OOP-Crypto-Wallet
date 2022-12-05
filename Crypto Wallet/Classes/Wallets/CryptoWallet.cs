using System;
namespace Crypto_Wallet.Classes
{
	public abstract class CryptoWallet
	{
		public Guid Address { get; }

		public Dictionary<Guid, double>? OwnedFungibleAssets { get; private set; }

        public static List<Guid>? SupportedFungibleAssets { get; set; }

        public List<Guid> Transactions { get; private set; }

		public CryptoWallet(Dictionary<Guid, double> ownedFungibleAssets)
		{
			Address = Guid.NewGuid();
			OwnedFungibleAssets = ownedFungibleAssets;
		}
	}
}

