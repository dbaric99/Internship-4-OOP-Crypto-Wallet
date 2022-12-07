using System;
namespace Crypto_Wallet.Classes.Transactions
{
	public class Transaction
	{
		public Guid Id { get; }
		public DateTime Date { get; }
		public Guid Sender { get; }
		public Guid Receiver { get; }
		public bool isRevoked { get; private set; }

		public Transaction(Guid senderAddress, Guid receiverAddress)
		{
			Id = Guid.NewGuid();
			Date = DateTime.Now;
			Sender = senderAddress;
			Receiver = receiverAddress;
		}
	}
}

