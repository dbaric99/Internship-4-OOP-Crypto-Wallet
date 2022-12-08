using System;
namespace Crypto_Wallet.Classes
{
	public abstract class Asset
	{
		public Guid Address { get; }

		public string Name { get; set; }

		public List<double> PastValues { get; private set; } = new List<double>();

		public Asset()
		{
			Address = Guid.NewGuid();
		}
		//TODO refactor #1
		public void AddPastValue(double newValue)
		{
			this.PastValues.Add(newValue);
		}
		//TODO +0%
		public string CalculateValueChange()
		{
            if (this.PastValues.Count() < 2 || this.PastValues.Last() == this.PastValues[this.PastValues.Count() - 2])
                return "0%";

			return $"{Math.Round(((this.PastValues.Last() - this.PastValues[^2]) / this.PastValues[this.PastValues.Count() - 2]) * 100, 2)}%";
        }
    }
}

