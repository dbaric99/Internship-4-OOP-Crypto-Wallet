using System;
namespace Crypto_Wallet.Classes
{
	public class BitcoinWallet : CryptoWallet
	{
		public BitcoinWallet(Dictionary<Guid, double> ownedFungibleAssets) : base(ownedFungibleAssets)
		{
			
		}

		public BitcoinWallet() : base()
		{

		}
	}
}

