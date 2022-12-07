using System;
namespace Crypto_Wallet.Classes
{
	public abstract class Asset
	{
		public Guid Address { get; }
		//TODO unique
		public string Name { get; set; }

		public List<double> PastValues { get; private set; }

		public Asset()
		{
			Address = Guid.NewGuid();
		}
	}
}

