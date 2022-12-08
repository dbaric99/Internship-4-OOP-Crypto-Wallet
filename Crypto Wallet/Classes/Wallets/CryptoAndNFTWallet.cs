using System;
using Crypto_Wallet.Classes.Assets;
using Crypto_Wallet.Interfaces;
using Crypto_Wallet.Global.Data;

namespace Crypto_Wallet.Classes.Wallets
{
	public abstract class CryptoAndNFTWallet : CryptoWallet, INonFungible
	{
        #region Properties

        public List<Guid> OwnedNonFungibleAssets { get; set; } = new List<Guid>();

        public static List<Guid> SupportedNonFungibleAssets { get; set; } = new List<Guid>();

        #endregion

        public CryptoAndNFTWallet() : base()
		{
            
        }

		public CryptoAndNFTWallet(Dictionary<Guid, double> ownedFungibleAssets, List<Guid> ownedNonFungibleAssets) : base(ownedFungibleAssets)
		{
			OwnedNonFungibleAssets = ownedNonFungibleAssets;
		}

        public static void AddSupportedNonFungibleAssets(List<Guid> supportedNonFungAssets)
        {
            SupportedNonFungibleAssets.AddRange(supportedNonFungAssets);
        }

        public List<Guid> GetSupportedNonFungibleAssets()
        {
            return SupportedNonFungibleAssets;
        }

        public double CalculateNonFungibleValueInUSD()
        {
            var value = 0d;

            foreach (var nonFungAddress in OwnedNonFungibleAssets)
            {
                var nonFungAsset = GlobalData.nonFungibleAssets.First(asset => asset.Address.Equals(nonFungAddress));

                var belongingFungibleAsset = GlobalData.fungibleAssets.First(asset => asset.Address.Equals(nonFungAsset.FungibleAsset));

                value += nonFungAsset.Value * belongingFungibleAsset.USDValue;
            }

            return value;
        }

        public double CalculateValueInUSD()
        {
            return base.CalculateFungibleValueInUSD() + this.CalculateNonFungibleValueInUSD();
        }

        public void SendNonFungibleAsset(Guid nonFungAddress)
        {
            this.OwnedNonFungibleAssets.Remove(nonFungAddress);
        }

        public void ReceiveNonFungibleAsset(Guid nonFungAddress)
        {
            this.OwnedNonFungibleAssets.Add(nonFungAddress);
        }
    }
}

