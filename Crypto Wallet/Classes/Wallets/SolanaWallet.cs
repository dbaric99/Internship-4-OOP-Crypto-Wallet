using Crypto_Wallet.Classes.Wallets;

namespace Crypto_Wallet.Classes
{
    public class SolanaWallet : CryptoAndNFTWallet
    {
        public SolanaWallet() : base() { }

        public SolanaWallet(Dictionary<Guid, double> ownedFungibleAssets, List<Guid> ownedNonFungibleAssets, List<Guid>? supportedFungibleAssets, List<Guid>? supportedNonFungableAssets) : base(ownedFungibleAssets, ownedNonFungibleAssets, supportedFungibleAssets, supportedNonFungableAssets) { }
    }
}

