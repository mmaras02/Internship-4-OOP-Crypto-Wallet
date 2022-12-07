using System;
using System.Collections.Generic;
using CryptoWallet.Transactions;
using CryptoWallet.Wallets;
using CryptoWallet.Assets;


namespace CryptoWallet.Transactions
{
    public class FungibleAssetTransactions:Transaction
    {
        public double StartingReceiverBalance{get;private set;}
        public double FinalReceiverBalance{get;private set;}
        public double StartingSenderBalance{get;private set;}
        public double FinalSenderBalance{get;private set;} 

        public FungibleAssetTransactions(Guid assetAddress, Wallet senderWallet, Wallet receiverWallet,double amount) : base(assetAddress, senderWallet, receiverWallet)
        {
            TransactionType="Fungible asset transaction";
            StartingReceiverBalance= (double)receiverWallet.FungibleAssetsBalance[assetAddress];
            StartingSenderBalance= (double)senderWallet.FungibleAssetsBalance[assetAddress];
    
            GetFinalBalance(senderWallet,receiverWallet,amount);
            
        }
        public override bool CancelTransaction(Wallet senderWallet,Wallet receiverWallet)
        {
            //provjera
            receiverWallet.FungibleAssetsBalance[AssetAddress] =StartingReceiverBalance;
            senderWallet.FungibleAssetsBalance[AssetAddress] =StartingSenderBalance;
            IsItCanceled=true;
            

            return true;
        }
        public void GetFinalBalance(Wallet senderWallet,Wallet receiverWallet,double amount)
        {
            FinalReceiverBalance=StartingReceiverBalance+amount;
            FinalSenderBalance=StartingSenderBalance-amount;

        }
    }
}