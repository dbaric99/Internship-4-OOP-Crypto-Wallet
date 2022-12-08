using System;
namespace Crypto_Wallet.Global.Constants
{
	public static class MessageConstants
	{
        public const string ACCESS_WALLET_REQUEST_MESSAGE = "\nInput the address of a wallet you want to access: ";

        public const string ACESS_WALLET_FAILED_MESSAGE = "There is no wallet by the target guid!";

        public const string ACCESS_WALLET_TRANSFER_MESSAGE = "\nInput the address of receiving wallet: ";

        public const string TRANSACTION_CONFORMATION_MESSAGE = "\nAre you sure you want to transfer assets to another account?";

        public const string ASSET_TO_TRANSFER_REQUEST_MESSAGE = "\nInput address of asset you are sending: ";
    }
}

