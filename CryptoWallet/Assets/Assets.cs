using System;
using System.Collections.Generic;
using CryptoWallet.Assets;


namespace CryptoWallet.Assets
{
    public abstract class Assets
    {
        public Guid Address{get;}
        public string Name{get;}
        //public double Value{get;set;}

        public string AssetType{get;set;}
        //private double Value{get;set;}

        public Assets(string name,double value)
        {
            Address=Guid.NewGuid();
            Name=name;
            //Value=value;
            AssetType="";
        }
    }
}