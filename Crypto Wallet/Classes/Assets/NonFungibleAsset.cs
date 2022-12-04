using System;
namespace Crypto_Wallet.Classes.Assets
{
	public class NonFungibleAsset : Asset
	{
		public decimal Value { get; set; }
		public Guid FungibleAsset { get; }

		public NonFungibleAsset() : base()
		{

		}
	}
}

