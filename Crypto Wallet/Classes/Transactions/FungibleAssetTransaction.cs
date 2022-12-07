using System;
namespace Crypto_Wallet.Classes.Transactions
{
	public class FungibleAssetTransaction : Transaction
	{
		public Guid FungibleAsset { get; }
		public double SenderStartBalance { get; set; }
		public double SenderEndBalance { get; set; }
		public double ReceiverStartBalance { get; set; }
		public double ReceiverEndBalance { get; set; }

		public FungibleAssetTransaction(Guid senderAddress, Guid receiverAddress, Guid fungibleAssetAddress, double senderStartBalace, double senderEndBalance, double receiverStartBalance, double receiverEndBalance) : base(senderAddress, receiverAddress)
		{
			FungibleAsset = fungibleAssetAddress;
			SenderStartBalance = senderStartBalace;
			SenderEndBalance = senderEndBalance;
			ReceiverStartBalance = receiverStartBalance;
			ReceiverEndBalance = receiverEndBalance;
		}
	}
}

