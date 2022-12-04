using System;
namespace Crypto_Wallet.Classes.Transactions
{
	public class NonFungibleAssetTransaction : Transaction
	{
		public Guid NonFungibleAsset { get; }

		public NonFungibleAssetTransaction() : base()
		{

		}
	}
}

