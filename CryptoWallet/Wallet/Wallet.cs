using System;
using System.Collections.Generic;
using CryptoWallet.Wallets;
using CryptoWallet.Assets;
using CryptoWallet.Transactions;
using System.Transactions;
using System.Collections;
using Transaction = CryptoWallet.Transactions.Transaction;
using ConsoleTables;


namespace CryptoWallet.Wallets
{
    public abstract class Wallet
    {
         public List<Guid> OwnedNonFungibleAssets { get;  set; }
        public List<Guid>AllowedNonFungibleAssets{get;set;}
        public Guid Address{get;}
        public List<Guid>AllowedFungibleAssets{get;set;}
        public Dictionary<Guid,double>FungibleAssetsBalance{get;private set;}
        public string WalletTypes { get; set;}
        public Dictionary<Guid,Transaction> TransactionHistory{get;set;}

        public Wallet()
        {
            Address=Guid.NewGuid();
            AllowedFungibleAssets=new List<Guid>();
            FungibleAssetsBalance=new Dictionary<Guid, double>();
            WalletTypes="";
            TransactionHistory=new Dictionary<Guid, Transaction>();
        
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
            {
                FungibleAssetsBalance.Add(fungibleAssets,amountToAdd);
            }
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
        }
        public void MakeNonFungibleTransaction(Wallet receiverWallet,Guid assetAddress)
        {
            if(!OwnedNonFungibleAssets.Contains(assetAddress))
                return;
            if(receiverWallet.OwnedNonFungibleAssets.Contains(assetAddress))
                return;

            NonFungibleAssetTransactions newTransaction=new(assetAddress,this,receiverWallet);

            receiverWallet.OwnedNonFungibleAssets.Add(assetAddress);
            this.OwnedNonFungibleAssets.Remove(assetAddress);
            receiverWallet.TransactionHistory.Add(newTransaction.GetId(),newTransaction);
            this.TransactionHistory.Add(newTransaction.GetId(),newTransaction);
    
            Console.WriteLine("You have made a successfull transfer");
        }
        public virtual double TotalValueOfAssetsInUSD(List<FungibleAssets> fungibleAssetList,List<NonFungibleAssets>nonFungibleAsset)//ova je samo za FA
        {
            double sum = 0;
            foreach (var item in FungibleAssetsBalance)
            {
                var asset=fungibleAssetList.Find(x=>x.Address.Equals(item.Key));
                sum+=item.Value*asset.GetValueInUSD();
            }
            return sum;
        }
        public void PrintTransaction()
        {
            var listSorted=TransactionHistory.Values.OrderByDescending(x => x.TransactionDate).ToList();

            foreach(var item in listSorted)
            {
                Console.WriteLine($"Transaction Id: {item.GetId()}\nTransaction date: {item.TransactionDate}\nType of transaction: {item.TransactionType}\nSender wallet address: {item.SenderAddress}\nReceiver wallet Address: {item.ReceiverAddress}\nWas canceled:{item.IsItCanceled}");
                Console.WriteLine("----------------------------------------------------------");

            }
        }

    }
}