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
		//TODO check references
		public Dictionary<Guid, double> OwnedFungibleAssets { get; private set; } = new Dictionary<Guid, double>();

		public static List<Guid> SupportedFungibleAssets { get; set; } = new List<Guid>();

        public List<Guid> Transactions { get; private set; }

		public List<double> Values { get; private set; } = new List<double>();

        #endregion

        public CryptoWallet(Dictionary<Guid, double> ownedFungibleAssets)
		{
			Address = Guid.NewGuid();
			OwnedFungibleAssets = ownedFungibleAssets;

			foreach (var ownedFung in ownedFungibleAssets)
				OwnedFungibleAssets[ownedFung.Key] = ownedFung.Value;
        }

		//TODO
		public CryptoWallet()
		{
			Address = Guid.NewGuid();

			foreach (var sfa in SupportedFungibleAssets)
				OwnedFungibleAssets.Add(sfa, 0);
		}

		public static void AddSupportedFungibleAssets(List<Guid> supportedFungAssets)
		{
			SupportedFungibleAssets.AddRange(supportedFungAssets);
		}

		public string GetWalletType()
		{
			return Regex.Replace(this.GetType().ToString().Replace(GeneralConstants.CLASS_PREFIX, ""), "[A-Z]", " $0");
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
		//TODO refactor #1
		public void AddValue(double newValue)
		{
			if (Values.Any() && Values.Last() == newValue) return;
			Values.Add(newValue);
		}

		public string GetValueChange(double newValue)
		{
			if (!this.Values.Any() || this.Values.Last() == newValue)
				return "0%";
            
            else if (newValue > this.Values.Last())
				return $"+{(this.Values.Last() / newValue) * 100}%";

			return $"-{(newValue / this.Values.Last()) * 100}%";
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

			AddValue(OwnedFungibleAssets[fungibleAssetAddress]);

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

			foreach (var transactionId in Transactions)
			{
				var transactionObj = allTransactions.FirstOrDefault(trans => trans.Id.Equals(transactionId));

				transactions.Add(transactionObj);
			}

			return transactions.OrderByDescending(trans => trans.Date).ToList<Transaction>();
		}
    }
}

