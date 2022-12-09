using Crypto_Wallet.Classes.Assets;
using Crypto_Wallet.Global.Data;

namespace Crypto_Wallet.Classes.Transactions
{
	public class FungibleAssetTransaction : Transaction
	{
        #region Properties
        public Guid FungibleAsset { get; }

		public double SenderStartBalance { get; set; }

		public double SenderEndBalance { get; set; }

		public double ReceiverStartBalance { get; set; }

		public double ReceiverEndBalance { get; set; }
        #endregion

        public FungibleAssetTransaction(Guid senderAddress, Guid receiverAddress, Guid fungibleAssetAddress, double senderStartBalace, double senderEndBalance, double receiverStartBalance, double receiverEndBalance) : base(senderAddress, receiverAddress)
		{
			FungibleAsset = fungibleAssetAddress;
			SenderStartBalance = senderStartBalace;
			SenderEndBalance = senderEndBalance;
			ReceiverStartBalance = receiverStartBalance;
			ReceiverEndBalance = receiverEndBalance;
        }

        public override string GetFungibleAssetName()
        {
			return GlobalData.fungibleAssets.FirstOrDefault(asset => asset.Address.Equals(this.FungibleAsset)).Name;
        }
    }
}

