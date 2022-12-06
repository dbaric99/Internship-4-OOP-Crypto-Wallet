using System;
namespace Crypto_Wallet.Classes.Wallets
{
	public abstract class CryptoAndNFTWallet : CryptoWallet
	{
		public List<Guid> OwnedNonFungibleAssets { get; set; } = new List<Guid>();

		public static List<Guid>? SupportedNonFungibleAssets { get; set; }

		public CryptoAndNFTWallet() : base()
		{
            OwnedNonFungibleAssets = new List<Guid>();
            foreach (var snfa in SupportedNonFungibleAssets)
                OwnedNonFungibleAssets.Add(snfa);
        }

		public CryptoAndNFTWallet(Dictionary<Guid, double> ownedFungibleAssets, List<Guid> ownedNonFungibleAssets) : base(ownedFungibleAssets)
		{
			OwnedNonFungibleAssets = ownedNonFungibleAssets;
		}
	}
}

