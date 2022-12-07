using System;
using System.Collections.Generic;
using CryptoWallet.Assets;
using CryptoWallet.Wallets;


namespace CryptoWallet.Wallets
{
    public class SolanaWallet : ConnectWithNonFungibleAssets
    {
        public SolanaWallet(List<FungibleAssets> fungibleAssetList, List<NonFungibleAssets> nonFungibleAssetList) : base(fungibleAssetList, nonFungibleAssetList) => WalletTypes = "Solana wallet";

        public override bool AddSupportedFungibleAsssets(Guid fungibleAssets) => base.AddSupportedFungibleAsssets(fungibleAssets);
        public override bool IncreaseFungibleAssetsBalance(Guid fungibleAssets, double amountToAdd) => base.IncreaseFungibleAssetsBalance(fungibleAssets, amountToAdd);
        public override bool ReduceSupportedFungibleAssetsBalance(Guid fungibleAssets, double amountToRemove) => base.ReduceSupportedFungibleAssetsBalance(fungibleAssets, amountToRemove);
        public override bool AddNotFungibleAssets(Guid nonFungibleAsset) => base.AddNotFungibleAssets(nonFungibleAsset);
        public override void AddSupportedNonFungibleAsset(Guid nonFungibleAsset) => base.AddSupportedNonFungibleAsset(nonFungibleAsset);


    }
}

