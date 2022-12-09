using Crypto_Wallet.Global.Constants;

namespace Crypto_Wallet.Classes.Transactions
{
	public class Transaction
	{
        #region Properties
        public Guid Id { get; }

		public DateTime Date { get; }

		public Guid Sender { get; }

		public Guid Receiver { get; }

		public bool isRevoked { get; private set; }
        #endregion

        public Transaction(Guid senderAddress, Guid receiverAddress)
		{
			Id = Guid.NewGuid();
			Date = DateTime.Now;
			Sender = senderAddress;
			Receiver = receiverAddress;
		}

        public bool IsNonFungible()
        {
			return this.GetType().Name == GeneralConstants.NONFUNGIBLE_TRANSACTION_TYPE;

        }

		public void Revoke()
		{
			this.isRevoked = true;
		}

		public virtual string GetFungibleAssetName()
		{
			return String.Empty;
		}
    }
}

