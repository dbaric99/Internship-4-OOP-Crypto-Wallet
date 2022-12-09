using Crypto_Wallet.Classes;
using Crypto_Wallet.Classes.Assets;
using Crypto_Wallet.Classes.Transactions;
using Crypto_Wallet.Classes.Wallets;

namespace Crypto_Wallet.Helpers
{
	public static class TransactionHelper
	{
		public static Transaction FungibleTransaction(CryptoWallet sender, CryptoWallet receiver, FungibleAsset asset, double amount)
		{
			var senderStartBalance = sender.GetFungibleValue(asset.Address);
			var receiverStartBalance = receiver.GetFungibleValue(asset.Address);

			var senderEndBalance = sender.FungibleValueManipulation(asset.Address, amount, false);
			var receiverEndBalance = receiver.FungibleValueManipulation(asset.Address, amount, true);

            var transactionInProgress = new FungibleAssetTransaction(sender.Address, receiver.Address, sender.Address, senderStartBalance, senderEndBalance, receiverStartBalance, receiverEndBalance);

            sender.AddTransaction(transactionInProgress.Id);
            receiver.AddTransaction(transactionInProgress.Id);

            asset.ChangeAssetValue();

			return transactionInProgress;
        }

		public static Transaction NonFungibleTransaction(CryptoAndNFTWallet sender, CryptoAndNFTWallet receiver, NonFungibleAsset asset)
		{
			var transactionInProgress = new NonFungibleAssetTransaction(sender.Address, receiver.Address, asset.Address);

			sender.SendNonFungibleAsset(asset.Address);
			receiver.ReceiveNonFungibleAsset(asset.Address);

			asset.ChangeBelongingFungibleValue();

			return transactionInProgress;
		}
	}
}

