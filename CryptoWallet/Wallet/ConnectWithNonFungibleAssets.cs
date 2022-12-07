using System;
using System.Collections.Generic;
using CryptoWallet.Wallets;
using CryptoWallet.Assets;
using CryptoWallet.Transactions;

namespace CryptoWallet.Wallets
{
    public class ConnectWithNonFungibleAssets : Wallet
    {
        public List<string>AllowedAssetNames=new List<string>{"tether", "cardano", "ethereum","bitcoin","polygon"};

        public ConnectWithNonFungibleAssets(List<FungibleAssets>fungibleAssetList,List<NonFungibleAssets>nonFungibleAssetList):base()
        {    

            OwnedNonFungibleAssets=new List<Guid>();
            AllowedNonFungibleAssets=new List<Guid>();
        
            LoadAssets(fungibleAssetList,nonFungibleAssetList);
        }
        public override bool AddSupportedFungibleAsssets(Guid fungibleAssets) => base.AddSupportedFungibleAsssets(fungibleAssets);
        public override bool IncreaseFungibleAssetsBalance(Guid fungibleAssets, double amountToAdd) => base.IncreaseFungibleAssetsBalance(fungibleAssets, amountToAdd);
        public override bool ReduceSupportedFungibleAssetsBalance(Guid fungibleAssets, double amountToRemove) => base.ReduceSupportedFungibleAssetsBalance(fungibleAssets, amountToRemove);
        public void LoadAssets(List<FungibleAssets>fungibleAssetList,List<NonFungibleAssets>nonFungibleAssetList)
        {
            foreach(var item in fungibleAssetList)
            {
                AllowedFungibleAssets.Add(item.Address);
                IncreaseFungibleAssetsBalance(item.Address,item.Value);
            }
            foreach(var item in nonFungibleAssetList)
            {
                AddSupportedNonFungibleAsset(item.Address);
            }
        }
        public virtual bool DoesOwnNonFungibleAsset()
        {
            if (OwnedNonFungibleAssets.Contains(this.Address))
                return true;
            return false;
        }
        public virtual bool AddNotFungibleAssets(Guid nonFungibleAsset)
        {
            if(OwnedNonFungibleAssets.Contains(nonFungibleAsset))
                return false;
            OwnedNonFungibleAssets.Add(nonFungibleAsset);
            return true;
        }
        public virtual void AddSupportedNonFungibleAsset(Guid nonFungibleAsset)
        {
            if(AllowedNonFungibleAssets.Contains(nonFungibleAsset))
                return;
            AllowedNonFungibleAssets.Add(nonFungibleAsset);
        }
        public override double TotalValueOfAssetsInUSD(List<FungibleAssets> fungibleAssetList,List<NonFungibleAssets>nonFungibleAssetList)
        {
            double sum=0;
            foreach(var item in OwnedNonFungibleAssets)
            {
                var asset=nonFungibleAssetList.Find(x=>x.Address.Equals(item));
                var fungible=asset.GetAllowedFungibleAsset(fungibleAssetList);
                sum+=fungible.Value*asset.Value;
            }

            return Math.Round(base.TotalValueOfAssetsInUSD(fungibleAssetList,nonFungibleAssetList)+sum,2);

        }


    }
}