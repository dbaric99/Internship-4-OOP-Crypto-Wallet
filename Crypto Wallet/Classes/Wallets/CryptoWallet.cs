using System;
namespace Crypto_Wallet.Classes
{
	public abstract class CryptoWallet
	{
		public Guid Address { get; }
		public Dictionary<Guid, int> FungibleAssets { get; private set; } = new Dictionary<Guid, int>();
        public static List<Guid>? SupportedFungibleAssets { get; set; }
        public List<Guid> Transactions { get; private set; }

		public CryptoWallet()
		{
			Address = Guid.NewGuid();
		}
	}
}

