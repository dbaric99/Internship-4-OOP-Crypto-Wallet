﻿using System;
using Crypto_Wallet.Classes.Assets;

namespace Crypto_Wallet.Interfaces
{
	public interface INonFungible
	{
        public double CalculateNonFungibleValueInUSD(List<NonFungibleAsset> allNonFungibleAssets, List<FungibleAsset> allFungibleAssets);

        public double CalculateValueInUSD(List<FungibleAsset> allFungibleAssets, List<NonFungibleAsset> allNonFungibleAssets);
    }
}

