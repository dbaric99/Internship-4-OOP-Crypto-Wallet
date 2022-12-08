using System;
namespace Crypto_Wallet.Classes
{
	public abstract class Asset
	{
		public Guid Address { get; }
		//TODO unique
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
		public string CalculateValueChange(double newValue)
		{
            if (!this.PastValues.Any() || this.PastValues.Last() == newValue)
                return "0%";

            return $"{Math.Round(((newValue - this.PastValues.Last()) / this.PastValues.Last()) * 100, 2)}%";
        }
    }
}

