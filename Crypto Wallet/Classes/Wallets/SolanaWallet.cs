using System;
namespace Crypto_Wallet.Classes
{
	public class SolanaWallet : CryptoWallet
	{
        public List<Guid> OwnedNonFungibleAssets { get; set; } = new List<Guid>();

        public static List<Guid>? SupportedNonFungibleAssets { get; set; }

        public SolanaWallet(Dictionary<Guid, double> ownedFungibleAssets, List<Guid> ownedNonFungibleAssets) : base(ownedFungibleAssets)
		{
			OwnedNonFungibleAssets = ownedNonFungibleAssets;
		}

		public SolanaWallet() : base()
		{
            OwnedNonFungibleAssets = new List<Guid>();
            foreach (var snfa in SupportedNonFungibleAssets)
                OwnedNonFungibleAssets.Add(snfa);
        }
	}
}

