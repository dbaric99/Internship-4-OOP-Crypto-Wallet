using System;
using System.Text.RegularExpressions;
using Crypto_Wallet.Classes.Assets;
using Crypto_Wallet.Interfaces;

namespace Crypto_Wallet.Classes
{
	public abstract class CryptoWallet : IFungible
	{
        #region Properties

        public Guid Address { get; }
		//TODO check references
        public Dictionary<Guid, double>? OwnedFungibleAssets { get; private set; }

        public static List<Guid>? SupportedFungibleAssets { get; set; }

        public List<Guid> Transactions { get; private set; }

		public List<double> Values { get; private set; } = new List<double>();

        #endregion

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

        public double CalculateFungibleValueInUSD(List<FungibleAsset> allFungibleAssets)
		{
			var value = 0d;

			foreach (var ownedFungAsset in OwnedFungibleAssets)
			{
				var targetedAsset = allFungibleAssets.First(fa => fa.Address == ownedFungAsset.Key);
				value += targetedAsset.USDValue * ownedFungAsset.Value;
			}

			return value;
		}

		public void AddValue(double newValue)
		{
			Values.Add(newValue);
		}

		public string GetValueChange(double newValue)
		{
			if (!this.Values.Any())
				return "0%";

			if(newValue > this.Values.Last())
				return $"+{(this.Values.Last() / newValue) * 100}%";

			return $"-{(newValue / this.Values.Last()) * 100}%";
		}
    }
}

