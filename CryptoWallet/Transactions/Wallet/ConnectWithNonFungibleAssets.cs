using System;
using System.Collections.Generic;
using CryptoWallet.Wallets;
using CryptoWallet.Assets;
using CryptoWallet.Transactions;

namespace CryptoWallet.Wallets
{
    public class ConnectWithNonFungibleAssets : Wallet
    {
        //public List<Guid> OwnedNonFungibleAssets { get; private set; }//change u dictionary??
        //public List<Guid>AllowedNonFungibleAssets{get;set;}
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
                IncreaseFungibleAssetsBalance(item.Address,10);//napravi da stavlja random vrijednost-->nemoze bit fiksnp
            }
            foreach(var item in nonFungibleAssetList)
            {
                AddSupportedNonFungibleAsset(item.Address);
                //if(!DoesOwnNonFungibleAsset(item.Address))
                //    OwnedNonFungibleAssets.Add(item.Address);
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
        public override double TotalValueOfFungibleAssetsInUSD(List<FungibleAssets> fungibleAssetList)
        {

            return base.TotalValueOfFungibleAssetsInUSD(fungibleAssetList);

        }


    }
}