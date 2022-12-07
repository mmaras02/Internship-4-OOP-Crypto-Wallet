using System;
using System.Collections.Generic;
using CryptoWallet.Assets;


namespace CryptoWallet.Assets
{
    public abstract class Assets
    {
        public Guid Address{get;}
        public string Name{get;}
        public double Value{get;set;}
        public List<double>ValueHistory{get;set;}
        public double ValueChangeInPercentage{get;set;}
        public string AssetType{get;set;}
        public Assets(string name,double value)
        {
            Address=Guid.NewGuid();
            Name=name;
            Value=value;
            AssetType="";
            ValueHistory=new List<double>();
            ValueChangeInPercentage=0;
        }
        public void AddValueHistory(double newValue)
        {
            ValueHistory.Add(newValue);
        }
    }
}