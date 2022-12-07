using System;
using Crypto_Wallet.Classes.Assets;

namespace Crypto_Wallet.Interfaces
{
	public interface INonFungible
	{
        public double CalculateNonFungibleValueInUSD();

        public double CalculateValueInUSD();
    }
}

