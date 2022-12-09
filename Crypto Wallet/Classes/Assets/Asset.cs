namespace Crypto_Wallet.Classes
{
	public abstract class Asset
	{
        #region Properties
        public Guid Address { get; }

		public string Name { get; set; }

		public List<double> PastValues { get; private set; } = new List<double>();
        #endregion

        public Asset()
		{
			Address = Guid.NewGuid();
		}
		
		public void AddPastValue(double newValue)
		{
			this.PastValues.Add(newValue);
		}

		public string CalculateValueChange()
		{
            if (this.PastValues.Count < 2 || this.PastValues.Last() == this.PastValues[this.PastValues.Count - 2])
                return "0%";

            var change = this.PastValues.Last() - this.PastValues[this.PastValues.Count - 2];

            var changeValue = change / this.PastValues[this.PastValues.Count - 2];

            return $"{Math.Round(changeValue * 100, 2)} %";
        }
    }
}

