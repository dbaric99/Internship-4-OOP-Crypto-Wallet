using System;
namespace Crypto_Wallet.Classes
{
	public abstract class Asset
	{
		public Guid Address { get; }
		//TODO unique
		public string Name { get; set; }

		public Asset()
		{
			Address = Guid.NewGuid();
		}
	}
}

