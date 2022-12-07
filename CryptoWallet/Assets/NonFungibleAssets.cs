using System;
using System.Collections.Generic;
using CryptoWallet.Assets;

//non-fungible tokens are non-interchangeable, unique, indivisible, and irreplaceable
//They can’t be exchanged for one another without losing value because each token is unique.
namespace CryptoWallet.Assets
{
    public  class NonFungibleAssets:Assets
    {
        public double Value { get; set; }
        public Guid AllowedFungibleAssetAddress{get;}
        //dodat currency?
        public NonFungibleAssets(string name,double value,Guid supportedFungibleAssetAddress):base(name)
        {
            Value=value;
            AllowedFungibleAssetAddress=supportedFungibleAssetAddress;
            AssetType="Non fungible asset";
        }
        // public Guid GetFungibleAssetAddress()
        // {
        //     return AllowedFungibleAssetAddress;
        // }
   /*      public bool FindSupportedFungibleAsset(List<FungibleAssets>fungibleAssetList,Guid nonFungible)
        {
            fungibleAssetList.Find(x=>x.Address.Equals())
            foreach(var item in fungibleAssetList)
            {

            }
            for(var item=0;item<AllowedAssets.Count;item++)
                if(AllowedFungibleAssetAddress.Equals(AllowedAssets[item]))
                    return true;
            return false;
        } */

        
    }
}