using System;
using System.Collections.Generic;
using CryptoWallet.Assets;


namespace CryptoWallet.Assets
{
    public  class Assets
    {
        public Guid Address{get;}
        public string Name{get;}
        public string AssetType{get;set;}
        //private double Value{get;set;}

        public Assets(string name)
        {
            Address=Guid.NewGuid();
            Name=name;
            //Value=value;
        }
        public string GetName()
        {
            return Name;
        }
        

    }
}