using System;
using System.Collections.Generic;
using CryptoWallet.Assets;

namespace CryptoWallet.Assets
{
    public  class FungibleAssets:Assets
    {
        public string Label{get;set;}

        public FungibleAssets(string name,double value,string label):base(name,value)
        {
            Value=value;
            Label=label;
            AssetType="Fungible asset";
        }
        public double GetValueInUSD() => Value;
        public string GetLabel() => Label;
        public void ChangeAssetValue()
        {
            Random random=new Random();

            double percentage=random.Next((int)-2.5,(int)2.5);
            Value+=(Value*(percentage/100));
            ValueHistory.Add(Value);
            ValueChangeInPercentage=percentage;
            //Console.WriteLine($"Your new value is {Value}");
        }

    }
}