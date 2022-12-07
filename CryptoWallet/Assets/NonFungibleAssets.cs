using System;
using System.Collections.Generic;
using CryptoWallet.Assets;


namespace CryptoWallet.Assets
{
    public  class NonFungibleAssets:Assets
    {
        public Guid AllowedFungibleAssetAddress{get;set;}
        public NonFungibleAssets(string name,double value,Guid supportedFungibleAssetAddress):base(name,value)
        {
            AllowedFungibleAssetAddress=supportedFungibleAssetAddress;
            AssetType="Non fungible asset";
        }
        public FungibleAssets GetAllowedFungibleAsset(List<FungibleAssets>fungibleAssetList)
        {
            return fungibleAssetList.Find(x=>x.Address.Equals(AllowedFungibleAssetAddress));
        }
        public double GetValueInUSD(List<FungibleAssets>fungibleAssetList)
        {
            return this.Value*(this.GetAllowedFungibleAsset(fungibleAssetList).Value);
        }
        
    }
}