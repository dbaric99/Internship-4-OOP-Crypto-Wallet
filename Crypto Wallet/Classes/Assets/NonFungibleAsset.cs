using System;
using Crypto_Wallet.Global.Data;

namespace Crypto_Wallet.Classes.Assets
{
	public class NonFungibleAsset : Asset
	{
		public double Value { get; set; }
		public Guid FungibleAsset { get; }

		public NonFungibleAsset(string name, double value, Guid fungibleAsset) : base()
		{
			Name = name;
			Value = value;
			FungibleAsset = fungibleAsset;
		}

        public double GetValueInUSD()
        {
			return this.Value * GlobalData.fungibleAssets.First(asset => asset.Address.Equals(this.FungibleAsset)).USDValue;
        }

        public void ChangeBelongingFungibleValue()
        {
			var nonFungAssetObj = GlobalData.nonFungibleAssets.First(asset => asset.Address.Equals(Address));

			GlobalData.fungibleAssets.FirstOrDefault(asset => asset.Address.Equals(nonFungAssetObj.FungibleAsset)).ChangeAssetValue();
        }

		public string GetFungibleAssetName()
		{
			return GlobalData.fungibleAssets.FirstOrDefault(asset => asset.Address.Equals(this.FungibleAsset))?.Name;
		}
    }
}

