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

		public string CalculateValueChange(double newValue)
		{
            if (!this.PastValues.Any())
                return "0%";

            if (newValue > this.PastValues.Last())
                return $"+{(this.PastValues.Last() / newValue) * 100}%";

            return $"-{(newValue / this.PastValues.Last()) * 100}%";
        }
    }
}

