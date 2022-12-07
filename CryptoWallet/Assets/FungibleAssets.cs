using System;
using System.Collections.Generic;
using CryptoWallet.Assets;

//nemogu postojat dva fungibleAssets s istom vrijednosti
//“fungible” is “easy to exchange or trade for something else of the same type and value.”
//divisible, interchangeable, and not unique.
namespace CryptoWallet.Assets
{
    public  class FungibleAssets:Assets
    {
        private string Label{get;set;}
        private double Value{get;set;}

        public FungibleAssets(string name,double value,string label):base(name)
        {
            Value=value;
            Label=label;
            AssetType="Fungible asset";
        }
        public double GetValueInUSD() => Value;
        public string GetLabel() => Label;
        //public void SetLabel(string label) => Label = label;
    }
}