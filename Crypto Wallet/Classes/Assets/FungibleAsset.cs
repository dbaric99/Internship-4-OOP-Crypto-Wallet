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

		public void ChangeAssetValue()
		{
			var options = new int[2] { 1, -1 };
			var random = new Random();
			var signum = options[random.Next(0, options.Length)];

			var percentage = random.NextDouble() * 2.5 * signum;

			var newValue = USDValue + (USDValue * (percentage / 100));

			this.USDValue = newValue;
			this.AddPastValue(newValue);
		}
	}
}

