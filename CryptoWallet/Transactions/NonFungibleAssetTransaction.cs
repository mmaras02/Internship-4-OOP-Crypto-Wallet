using System;
using System.Collections.Generic;
using CryptoWallet.Transactions;
using CryptoWallet.Wallets;
using CryptoWallet.Assets;


namespace CryptoWallet.Transactions
{
    public class NonFungibleAssetTransactions:Transaction
    { 
        public NonFungibleAssetTransactions(Guid assetAddress, Wallet senderWallet, Wallet receiverWallet):base(assetAddress,senderWallet,receiverWallet)
        {
            IsItCanceled=false;
            TransactionType="Non fungible asset transaction";

            senderWallet.OwnedNonFungibleAssets.Remove(assetAddress);
            receiverWallet.OwnedNonFungibleAssets.Add(assetAddress);       
        }
    }
}