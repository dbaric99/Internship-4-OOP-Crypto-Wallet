using System;
namespace Crypto_Wallet.Classes.Assets
{
	public class FungibleAsset : Asset
	{
		//TODO unique
		public string Label { get; set; }
		public double USDValue { get; private set; }

		public FungibleAsset(string name, string label, double value) : base()
		{
			Name = name;
			Label = label;
			USDValue = value;
		}
	}
}

