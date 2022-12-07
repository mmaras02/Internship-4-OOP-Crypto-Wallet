using System;
using System.Collections.Generic;
using CryptoWallet.Wallets;
using CryptoWallet.Assets;
using CryptoWallet.Transactions;
using System.Transactions;
using System.Collections;
using Transaction = CryptoWallet.Transactions.Transaction;

//stavi za racunanje USD
namespace CryptoWallet.Wallets
{
    public abstract class Wallet
    {
         public List<Guid> OwnedNonFungibleAssets { get;  set; }//change u dictionary??
        public List<Guid>AllowedNonFungibleAssets{get;set;}
        public Guid Address{get;}
        public List<Guid>AllowedFungibleAssets{get;set;}//Assetsi koje mozemo posjedovat
        //public List<Guid> TransactionHistory{get;private set;}
        public Dictionary<Guid,double>FungibleAssetsBalance{get;private set;}
        public string WalletTypes { get; set;}
        public Dictionary<Guid,Transaction> TransactionHistory{get;set;}

        public Wallet()
        {
            Address=Guid.NewGuid();
            AllowedFungibleAssets=new List<Guid>();
            //TransactionHistory=new List<Guid>();
            FungibleAssetsBalance=new Dictionary<Guid, double>();
            WalletTypes="";
            TransactionHistory=new Dictionary<Guid, Transaction>();
            //OwnedNonFungibleAssets=new List<Guid>();
            //AllowedNonFungibleAssets=new List<Guid>();
        }
        public virtual bool AddSupportedFungibleAsssets(Guid fungibleAssets)
        {
            if(!AllowedFungibleAssets.Contains(fungibleAssets))
                return false;

            AllowedFungibleAssets.Add(fungibleAssets);
            FungibleAssetsBalance.Add(fungibleAssets,0);
            return true;
        }
        public virtual bool IncreaseFungibleAssetsBalance(Guid fungibleAssets,double amountToAdd)
        {
            if(!AllowedFungibleAssets.Contains(fungibleAssets))
                return false;

            if(FungibleAssetsBalance.ContainsKey(fungibleAssets))
                FungibleAssetsBalance[fungibleAssets]+=amountToAdd;

            else
                FungibleAssetsBalance.Add(fungibleAssets,amountToAdd);

            return true;
        }
        public virtual bool ReduceSupportedFungibleAssetsBalance(Guid fungibleAssets,double amountToRemove)
        {
            if(!AllowedFungibleAssets.Contains(fungibleAssets))
            {
                Console.WriteLine("Fungible asset not supported in this wallet!\n");
                return false;
            }
            if(FungibleAssetsBalance[fungibleAssets]<amountToRemove)
            {
                Console.WriteLine("There is not enough value to remove!\n");
                return false;
            }
            FungibleAssetsBalance[fungibleAssets]-=amountToRemove;
            return true;
        }

        public FungibleAssetTransactions CreateNewTransaction(Wallet receiverWallet, Guid assetAddress, double amount)
        {
            FungibleAssetTransactions newTransaction=new(assetAddress,this,receiverWallet,amount);
            return newTransaction;
        }
        /* public virtual bool RemoveSupportedAsset(Guid fungibleAssets)
{
   if(!AllowedAssets.Contains(fungibleAssets))
       return false;
   AllowedAssets.Remove(fungibleAssets);
   FungibleAssetsBalance.Remove(fungibleAssets);
   return true;
} */
        /* public virtual bool RecordTransactionHistory(Wallet receiverWallet,Guid assetAddress,double amount)
        {
            if(!receiverWallet.AllowedFungibleAssets.Contains(assetAddress))
                return false;
            //FungibleAssetTransactions newTransaction=new (assetAddress,this,receiverWallet,amount);
            //TransactionHistory.Add(newTransaction);
            //TransactionHistory.Add(assetAddress);
            FungibleAssetTransactions newTransaction=new(assetAddress,senderWallet,receiverWallet,amount)
            TransactionHistory[assetAddress]=newTransaction;
            return true;
        }
      */
        public void MakeFungibleTransaction(Wallet receiverWallet,Guid assetAddress,double amount)
        {
            if(!this.AllowedFungibleAssets.Contains(assetAddress))
                return;
            if(!receiverWallet.AllowedFungibleAssets.Contains(assetAddress))
                return;
            if(FungibleAssetsBalance[assetAddress]<amount)
                return;

            FungibleAssetTransactions newTransaction=CreateNewTransaction(receiverWallet,assetAddress,amount);

            this.ReduceSupportedFungibleAssetsBalance(assetAddress,amount);
            receiverWallet.IncreaseFungibleAssetsBalance(assetAddress,amount);

            this.TransactionHistory.Add(newTransaction.GetId(),newTransaction);
            receiverWallet.TransactionHistory.Add(newTransaction.GetId(),newTransaction);
            Console.WriteLine("You have successfully made a transaction!");
            //return;
        }

        public virtual double TotalValueOfFungibleAssetsInUSD(List<FungibleAssets> fungibleAssetList)//ova je samo za FA
        {
            //var wallet = wallets.Find(x => x.Address.Equals(walletAddress));
            double sum = 0;
            foreach (var item in FungibleAssetsBalance)
            {
                //var asset=fungibleAssetList.Find(x=>x.Address.Equals(item.Key));
                //sum+=item.Value*asset.GetValueInUSD();
                foreach(var item1 in fungibleAssetList)
                {
                    if(item.Key==item1.Address)
                        sum+=item.Value * item1.GetValueInUSD();
                }
            }
            return sum;
        }

        //napravi za racunanje u USD
        //postotak opadanja i porasta
        public virtual void PrintAssets(List<FungibleAssets> fungibleAssetList, List<NonFungibleAssets> nonFungibleAssetList)
        {
                foreach(var item in FungibleAssetsBalance)
                {
                    var searchFungible=FindFungible(fungibleAssetList, item.Key);
                    Console.WriteLine($"{searchFungible.Address} - {searchFungible.Name} - {searchFungible.GetLabel()}");//dodat enum assetTipe?? 
                }
        }
        public FungibleAssets FindFungible(List<FungibleAssets> fungibleAssetList,Guid address)
        {
            foreach(var item in fungibleAssetList)
            {
                //FungibleAssets asset=fungibleAssetList.Find(x=>x.Address.Equals(item.Address));
                if(item.Address==address) return item;
            }
            return null;
        }

        internal void MakeFungibleTransaction(Guid address, Guid assetAddress, double amount)
        {
            throw new NotImplementedException();
        }
    }
}