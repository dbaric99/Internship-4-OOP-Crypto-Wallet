namespace Crypto_Wallet.Classes.Assets
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
	}
}

