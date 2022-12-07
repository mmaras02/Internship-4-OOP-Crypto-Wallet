using System;
using System.Collections.Generic;
using CryptoWallet.Transactions;
using CryptoWallet.Wallets;


namespace CryptoWallet.Transactions
{
    public abstract  class Transaction
    {
        private Guid Id{get;}
        public Guid AssetAddress{get;}
        public DateTime TransactionDate{get;}
        public Guid SenderAddress{get;}
        public Guid ReceiverAddress{get;}
        public bool IsItCanceled;
        public string TransactionType{get;set;}

        protected Transaction(Guid assetAddress, Wallet senderWallet, Wallet receiverWallet)
        {
            Id = Guid.NewGuid();
            AssetAddress = assetAddress;
            TransactionDate = DateTime.Now;
            SenderAddress = senderWallet.Address;
            ReceiverAddress = receiverWallet.Address;
            TransactionType="";
            //IsItCanceled = false;-->set na transakciji
        }
        public Guid GetId() => Id;
        public virtual bool CancelTransaction(Wallet senderWaller, Wallet receiverWallet)
        {
            if (IsItCanceled)
                return false;
            return true;
        }
        //CancellationToken transaction-->novac se vraca u sender vallet s istin assetom na odredenoj adresi
        //-->u receiver walletu se oduzima na istoj adresi tocan iznos transakcije

    }
}