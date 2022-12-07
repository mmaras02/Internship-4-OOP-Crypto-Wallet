using System;
using System.Collections.Generic;
using CryptoWallet.Assets;
using CryptoWallet.Wallets;

//ako kreiramo novi wallet onda odma pridodajemo sve allowed assets i stavimo novi balance
//
namespace CryptoWallet.Wallets
{
    public class BitcoinWallet : Wallet
    {
        public BitcoinWallet() :base(){}
        List<string>allowedAssets=new List<string>{"bitcoin", "ethereum", "dogecoin","solana","polygon", "xrp", "tether", "polkadot", "cosmos"};


        //private List<string> AllowedAssetNames = new(){"bitcoin", "ethereum", "dogecoin","solana","polygon", "xrp", "tether", "polkadot", "cosmos"};
        public override bool AddSupportedFungibleAsssets(Guid fungibleAssets) => base.AddSupportedFungibleAsssets(fungibleAssets);
        public override bool IncreaseFungibleAssetsBalance(Guid fungibleAssets, double amountToAdd) => base.IncreaseFungibleAssetsBalance(fungibleAssets, amountToAdd);
        public override bool ReduceSupportedFungibleAssetsBalance(Guid fungibleAssets, double amountToRemove) => base.ReduceSupportedFungibleAssetsBalance(fungibleAssets, amountToRemove);

        public BitcoinWallet(List<FungibleAssets>fungibleAssetList)
        {
            WalletTypes="Bitcoin wallet";

            for(var asset=0;asset<allowedAssets.Count;asset++)
            {
                AllowedFungibleAssets.Add(fungibleAssetList[asset].Address);
                IncreaseFungibleAssetsBalance(fungibleAssetList[asset].Address,5);//random stavi
            }
        }
        public void PrintAllowedFungibleAssets()
        {
            foreach(var item in allowedAssets)
                Console.WriteLine(item);
        }
    }
}