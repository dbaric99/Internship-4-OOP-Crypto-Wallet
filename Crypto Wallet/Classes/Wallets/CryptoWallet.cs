using System;
using System.Text.RegularExpressions;
using Crypto_Wallet.Classes.Assets;
using Crypto_Wallet.Interfaces;
using Crypto_Wallet.Global.Data;
using Crypto_Wallet.Global.Constants;
using Crypto_Wallet.Classes.Transactions;

namespace Crypto_Wallet.Classes
{
	public abstract class CryptoWallet : IFungible
	{
		#region Properties

		public Guid Address { get; }

		public Dictionary<Guid, double> OwnedFungibleAssets { get; private set; } = new Dictionary<Guid, double>();

		public static Dictionary<string, List<Guid>> SupportedFungibleAssets { get; set; } = new Dictionary<string, List<Guid>>();

		public List<Guid> Transactions { get; private set; } = new List<Guid>();

		public List<double> Values { get; private set; } = new List<double>();

		#endregion

		public CryptoWallet(Dictionary<Guid, double> ownedFungibleAssets, List<Guid>? supportedFungibleAssets)
		{
            Address = Guid.NewGuid();
            OwnedFungibleAssets = ownedFungibleAssets;

            foreach (var ownedFung in ownedFungibleAssets)
                OwnedFungibleAssets[ownedFung.Key] = ownedFung.Value;
			if(supportedFungibleAssets != null)
				SupportedFungibleAssets[this.GetWalletType()] = supportedFungibleAssets;
		}

		public CryptoWallet()
		{
			Address = Guid.NewGuid();

			foreach (var sfa in SupportedFungibleAssets[this.GetWalletType()])
				OwnedFungibleAssets[sfa] = 0;
		}

		public List<Guid> GetSupportedFungibleAssets()
		{
			return SupportedFungibleAssets[this.GetWalletType()];

        }

		public string GetWalletType()
		{
			return this.GetType().Name;
		}

        public double CalculateFungibleValueInUSD()
		{
			var value = 0d;

			foreach (var ownedFungAsset in OwnedFungibleAssets)
			{
				var targetedAsset = GlobalData.fungibleAssets.First(fa => fa.Address == ownedFungAsset.Key);
				value += targetedAsset.USDValue * ownedFungAsset.Value;
			}

			return value;
		}
		
		public void AddValue(double newValue)
		{
			Values.Add(newValue);
		}

		public string GetValueChange()
		{
			if (this.Values.Count < 2 || this.Values.Last() == this.Values[this.Values.Count - 2])
				return "0%";

			var change = this.Values.Last() - this.Values[this.Values.Count - 2];
			
			var changeValue = change / this.Values[this.Values.Count - 2];

			return $"{Math.Round(changeValue * 100, 2)} %";
		}

		public (string cryptoValue, string usdValue) CalculateFungibleAssetsValue(FungibleAsset targetFungAsset)
		{
			var amount = OwnedFungibleAssets[targetFungAsset.Address];
			var usdValue = amount * targetFungAsset.USDValue;

			return ($"{amount} {targetFungAsset.Label}", $"$ {usdValue}");
		}

		public bool SupportsNFT()
		{
			return this.GetType().BaseType.Name != GeneralConstants.MAIN_TYPE;
        }

		public double GetFungibleValue(Guid fungibleAssetAddress)
		{
			return OwnedFungibleAssets[fungibleAssetAddress];
		}

		public double FungibleValueManipulation(Guid fungibleAssetAddress, double amount, bool isReceiving)
		{
			OwnedFungibleAssets[fungibleAssetAddress] = isReceiving
				? OwnedFungibleAssets[fungibleAssetAddress] + amount
				: OwnedFungibleAssets[fungibleAssetAddress] - amount;

			return OwnedFungibleAssets[fungibleAssetAddress];
		}

		public void AddTransaction(Guid transactionToAddAddress)
		{
			this.Transactions.Add(transactionToAddAddress);
		}

		public void RemoveTransaction(Guid transactionId)
		{
			this.Transactions.Remove(transactionId);
		}

		public List<Transaction> AllTransactionsOrderedByDate(List<Transaction> allTransactions)
		{
			var transactions = new List<Transaction>();

			foreach (var transactionId in this.Transactions)
			{
				var transactionObj = allTransactions.FirstOrDefault(trans => trans.Id.Equals(transactionId));

				transactions.Add(transactionObj);
			}

			return transactions.OrderByDescending(trans => trans.Date).ToList();
		}
    }
}

