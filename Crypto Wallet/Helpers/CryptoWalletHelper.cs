using ConsoleTables;
using Crypto_Wallet.Classes;
using Crypto_Wallet.Classes.Wallets;
using Crypto_Wallet.Enums;
using Crypto_Wallet.Global.Constants;
using Crypto_Wallet.Global.Data;

namespace Crypto_Wallet.Helpers
{
	public static class CryptoWalletHelper
	{
        public static void CreateNewCryptoWallet()
        {
            Console.Clear();

            GeneralHelper.PrintGeneralSectionSeparator("Create Wallet");
            Console.Write("Available wallets to create: Bitcoin, Ethereum or Solana\n"
                + "Enter the type of wallet you want to create: ");

            var wantedWallet = GeneralHelper.CapitalizeAndTrim(Console.ReadLine()) + "Wallet";

            if (!Enum.IsDefined(typeof(CryptoWalletTypes), wantedWallet))
            {
                Console.WriteLine("\nWrong input for wallet type! Wallet can be of types: Bitcoin, Ethereum or Solana");
                return;
            }

            Type walletType = Type.GetType(GeneralConstants.CLASS_PREFIX + wantedWallet);

            if (GeneralHelper.ConfirmChoice($"Are you sure you want to add new wallet of type {wantedWallet}?"))
                GlobalData.wallets.Add(Activator.CreateInstance(walletType) as CryptoWallet);
        }

        public static void PrintAllWallets()
        {
            var table = new ConsoleTable("Wallet Type", "Wallet Address", "USD Asset Value", "Value Change");

            foreach (var wallet in GlobalData.wallets)
            {
                var value = GetValueFromWalletByType(wallet);
                
                wallet.AddValue(value);

                table.AddRow(
                    wallet.GetWalletType(),
                    wallet.Address,
                    $"$ {value}",
                    GetValueChange(wallet.Values)
                );
            }

            table.Write(Format.Alternative);
        }

        public static double GetValueFromWalletByType(CryptoWallet wallet)
        {
            return !wallet.SupportsNFT()
                    ? wallet.CalculateFungibleValueInUSD()
                    : ((CryptoAndNFTWallet)wallet).CalculateValueInUSD();
        }

        public static CryptoWallet? GetWalletByAddress(string queryMessage, string rejectionMessage)
        {
            var targetWalletAddress = GeneralHelper.GetGuidFromUserInput(queryMessage);

            var wallet = GlobalData.wallets.FirstOrDefault(wallet => wallet.Address.Equals(targetWalletAddress));

            if (wallet == null)
                Console.WriteLine(rejectionMessage);

            return wallet;
        }

        public static string GetValueChange(List<double> pastValues)
        {
            if (pastValues.Count < 2 || pastValues.Last() == pastValues[^2] )
                return "0%";
            
            var lastVal = pastValues.Last();
            var secondToLastVal = pastValues[^2];

            var change = (lastVal - secondToLastVal) / secondToLastVal;

            return $"{Math.Round((double)change * 100, 2)}%";
        }
    }
}

