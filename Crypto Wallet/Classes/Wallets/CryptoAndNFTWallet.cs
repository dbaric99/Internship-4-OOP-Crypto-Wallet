﻿using System;
using Crypto_Wallet.Classes.Assets;
using Crypto_Wallet.Interfaces;

namespace Crypto_Wallet.Classes.Wallets
{
	public abstract class CryptoAndNFTWallet : CryptoWallet, INonFungible
	{
        #region Properties
        //TODO check references
        public List<Guid>? OwnedNonFungibleAssets { get; set; }

        public static List<Guid>? SupportedNonFungibleAssets { get; set; }

        #endregion

        public CryptoAndNFTWallet() : base()
		{
            OwnedNonFungibleAssets = new List<Guid>();
            foreach (var supportedNonFungAsset in SupportedNonFungibleAssets)
                OwnedNonFungibleAssets.Add(supportedNonFungAsset);
        }

		public CryptoAndNFTWallet(Dictionary<Guid, double> ownedFungibleAssets, List<Guid> ownedNonFungibleAssets) : base(ownedFungibleAssets)
		{
			OwnedNonFungibleAssets = ownedNonFungibleAssets;
		}

        public double CalculateNonFungibleValueInUSD(List<NonFungibleAsset> allNonFungibleAssets, List<FungibleAsset> allFungibleAssets)
        {
            var value = 0d;

            foreach (var nonFungAddress in OwnedNonFungibleAssets)
            {
                var nonFungAsset = allNonFungibleAssets.First(asset => asset.Address.Equals(nonFungAddress));

                var belongingFungibleAsset = allFungibleAssets.First(asset => asset.Address.Equals(nonFungAsset.FungibleAsset));

                value += nonFungAsset.Value * belongingFungibleAsset.USDValue;
            }

            return value;
        }

        public double CalculateValueInUSD(List<FungibleAsset> allFungibleAssets, List<NonFungibleAsset> allNonFungibleAssets)
        {
            return base.CalculateFungibleValueInUSD(allFungibleAssets) + this.CalculateNonFungibleValueInUSD(allNonFungibleAssets, allFungibleAssets);
        }
    }
}

