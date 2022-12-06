using System;
using System.Text.RegularExpressions;

namespace Crypto_Wallet.Classes
{
	public abstract class CryptoWallet
	{
		public Guid Address { get; }

		public Dictionary<Guid, double>? OwnedFungibleAssets { get; private set; }

        public static List<Guid>? SupportedFungibleAssets { get; set; }

        public List<Guid> Transactions { get; private set; }

		public List<double> Values { get; private set; }

		public CryptoWallet(Dictionary<Guid, double> ownedFungibleAssets)
		{
			Address = Guid.NewGuid();
			OwnedFungibleAssets = ownedFungibleAssets;
		}

		//TODO
		public CryptoWallet()
		{
			Address = Guid.NewGuid();

			OwnedFungibleAssets = new Dictionary<Guid, double>();

			foreach (var sfa in SupportedFungibleAssets)
				OwnedFungibleAssets.Add(sfa, 0);
		}

		//TODO use global constants file?
		public string GetWalletType()
		{
			return Regex.Replace(this.GetType().ToString().Replace("Crypto_Wallet.Classes.", ""), "[A-Z]", " $0");
		}
	}
}

