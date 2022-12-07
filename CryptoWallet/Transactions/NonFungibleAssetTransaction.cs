using System;
using System.Collections.Generic;
using CryptoWallet.Transactions;
using CryptoWallet.Wallets;
using CryptoWallet.Assets;


namespace CryptoWallet.Transactions
{
    public class NonFungibleAssetTransactions:Transaction
    { 
        public NonFungibleAssetTransactions(Guid assetAddress, ConnectWithNonFungibleAssets senderWallet, ConnectWithNonFungibleAssets receiverWallet):base(assetAddress,senderWallet,receiverWallet)
        {
            IsItCanceled=false;
            TransactionType="Non fungible asset transaction";

            senderWallet.OwnedNonFungibleAssets.Remove(assetAddress);
            receiverWallet.OwnedNonFungibleAssets.Add(assetAddress);       
        }

        public NonFungibleAssetTransactions(Guid assetAddress, Wallet senderWallet, Wallet receiverWallet) : base(assetAddress, senderWallet, receiverWallet)
        {
        }

        /* public NonFungibleAssetTransactions CreateNewTransaction(Wallet senderWallet,Wallet receiverWallet,Guid assetAddress)
        {
            NonFungibleAssetTransactions newTransaction=new(assetAddress, senderWallet, receiverWallet);
            return newTransaction;
        } */
    }
}