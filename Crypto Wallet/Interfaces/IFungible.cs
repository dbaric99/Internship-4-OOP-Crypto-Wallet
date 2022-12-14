using System;
using Crypto_Wallet.Classes.Assets;

namespace Crypto_Wallet.Interfaces
{
	public interface IFungible
	{
        public double CalculateFungibleValueInUSD();

        public (string cryptoValue, string usdValue) CalculateFungibleAssetsValue(FungibleAsset targetFungAsset);
    }
}

