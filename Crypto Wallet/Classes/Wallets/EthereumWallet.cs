﻿using System;
namespace Crypto_Wallet.Classes
{
	public class EthereumWallet : CryptoWallet
	{
		public List<Guid> OwnedNonFungibleAssets { get; set; } = new List<Guid>();

		public static List<Guid>? SupportedNonFungibleAssets { get; set; }

		public EthereumWallet(Dictionary<Guid,double> ownedFungibleAssets, List<Guid> ownedNonFungibleAssets) : base(ownedFungibleAssets)
		{
			OwnedNonFungibleAssets = ownedNonFungibleAssets;
		}

		public EthereumWallet() : base()
		{
            OwnedNonFungibleAssets = new List<Guid>();
            foreach (var snfa in SupportedNonFungibleAssets)
                OwnedNonFungibleAssets.Add(snfa);
        }
	}
}

