using System;
namespace Crypto_Wallet.Classes.Transactions
{
	public class FungibleAssetTransaction : Transaction
	{
		public Guid FungibleAsset { get; }
		public decimal SenderStartBalance { get; set; }
		public decimal SenderEndBalance { get; set; }
		public decimal ReceiverStartBalance { get; set; }
		public decimal ReceiverEndBalance { get; set; }

		public FungibleAssetTransaction() : base()
		{

		}
	}
}

