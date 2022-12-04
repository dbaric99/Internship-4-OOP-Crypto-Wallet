using System;
namespace Crypto_Wallet.Classes
{
	public abstract class CryptoWallet
	{
		public Guid Address { get; }
		public Dictionary<Guid, int> FungibleAssets { get; private set; } = new Dictionary<Guid, int>();
		//TODO add bitcoin and make it readonly .AsReadOnly()
		public List<Guid> SupportedFungibleAssets { get; } = new List<Guid>();
		public List<Guid> Transactions { get; private set; }

		public CryptoWallet()
		{
			Address = Guid.NewGuid();
		}
	}
}

