using System;
using System.Collections.Generic;
using CryptoWallet.Wallets;
using CryptoWallet.Assets;



namespace CryptoWallet.Wallets
{
    public class EthereumWallet : ConnectWithNonFungibleAssets
    {
        public override bool AddSupportedFungibleAsssets(Guid fungibleAssets) => base.AddSupportedFungibleAsssets(fungibleAssets);
        public override bool IncreaseFungibleAssetsBalance(Guid fungibleAssets, double amountToAdd) => base.IncreaseFungibleAssetsBalance(fungibleAssets, amountToAdd);
        public override bool ReduceSupportedFungibleAssetsBalance(Guid fungibleAssets, double amountToRemove) => base.ReduceSupportedFungibleAssetsBalance(fungibleAssets, amountToRemove);
        public override bool AddNotFungibleAssets(Guid nonFungibleAsset) => base.AddNotFungibleAssets(nonFungibleAsset);
        public override void AddSupportedNonFungibleAsset(Guid nonFungibleAsset) => base.AddSupportedNonFungibleAsset(nonFungibleAsset);
        public EthereumWallet(List<FungibleAssets> fungibleAssetList, List<NonFungibleAssets> nonFungibleAssetList) : base(fungibleAssetList, nonFungibleAssetList) => WalletTypes = "Ethereum wallet";

    }
}