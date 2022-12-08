using System;
using Crypto_Wallet.Global.Data;

namespace Crypto_Wallet.Classes.Transactions
{
	public class NonFungibleAssetTransaction : Transaction
	{
		public Guid NonFungibleAsset { get; }

		public NonFungibleAssetTransaction(Guid senderAddress, Guid receiverAddress, Guid nonFungibleAssetAddress) : base(senderAddress, receiverAddress)
		{
			NonFungibleAsset = nonFungibleAssetAddress;
		}

		public string GetFungibleAssetInvolved()
		{
			return GlobalData.nonFungibleAssets.FirstOrDefault(asset => asset.Address.Equals(this.NonFungibleAsset)).GetFungibleAssetName();
		}
	}
}

