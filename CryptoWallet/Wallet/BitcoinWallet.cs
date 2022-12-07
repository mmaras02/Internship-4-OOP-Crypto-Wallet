using System;
using System.Collections.Generic;
using CryptoWallet.Assets;
using CryptoWallet.Wallets;

namespace CryptoWallet.Wallets
{
    public class BitcoinWallet : Wallet
    {

        public BitcoinWallet() :base(){}
       
        public override bool AddSupportedFungibleAsssets(Guid fungibleAssets) => base.AddSupportedFungibleAsssets(fungibleAssets);
        public override bool IncreaseFungibleAssetsBalance(Guid fungibleAssets, double amountToAdd) => base.IncreaseFungibleAssetsBalance(fungibleAssets, amountToAdd);
        public override bool ReduceSupportedFungibleAssetsBalance(Guid fungibleAssets, double amountToRemove) => base.ReduceSupportedFungibleAssetsBalance(fungibleAssets, amountToRemove);

        public BitcoinWallet(List<FungibleAssets>fungibleAssetList)
        {
            WalletTypes="Bitcoin wallet";
            foreach(var item in fungibleAssetList)
            {
                AllowedFungibleAssets.Add(item.Address);
                IncreaseFungibleAssetsBalance(item.Address,item.Value);
            }

        }
    }
}