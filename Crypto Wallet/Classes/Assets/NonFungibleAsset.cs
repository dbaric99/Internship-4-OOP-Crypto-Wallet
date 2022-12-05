﻿using System;
namespace Crypto_Wallet.Classes.Assets
{
	public class NonFungibleAsset : Asset
	{
		public double Value { get; set; }
		public Guid FungibleAsset { get; }

		public NonFungibleAsset(string name, double value, Guid fungibleAsset) : base()
		{
			Name = name;
			Value = value;
			FungibleAsset = fungibleAsset;
		}
	}
}

