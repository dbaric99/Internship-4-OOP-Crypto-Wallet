using System;
namespace Crypto_Wallet.Classes.Transactions
{
	public class Transaction
	{
		public Guid Id { get; }
		public DateTime Date { get; }
		public Guid Sender { get; }
		public Guid Receiver { get; }
		public bool isRevoked { get; set; }

		public Transaction()
		{
			Id = Guid.NewGuid();
		}
	}
}

