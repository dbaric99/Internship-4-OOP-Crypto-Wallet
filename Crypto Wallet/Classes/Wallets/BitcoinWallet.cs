using System;
namespace Crypto_Wallet.Classes
{
	public class BitcoinWallet : CryptoWallet
	{
        public BitcoinWallet() : base() { }

        public BitcoinWallet(Dictionary<Guid, double> ownedFungibleAssets) : base(ownedFungibleAssets) { }
	}
}

