using System;
using System.Collections.Generic;
using CryptoWallet.Wallets;
using CryptoWallet.Assets;
using CryptoWallet.Transactions;
using ConsoleTables;

namespace CryptoWallet
{
    internal  class Program
    {
        public static void Main(string[] args)
        {
            List<FungibleAssets> fungibleAssetList=AddFungibleAssets();
            List<NonFungibleAssets> nonFungibleAssetList=AddNonFungibleAssets(fungibleAssetList);
            var allWallets=GetWallets(fungibleAssetList,nonFungibleAssetList);

            var choice=-1;
            do{
                Options();
                var choiceSuccess=int.TryParse(Console.ReadLine(),out choice);
                if(choiceSuccess)
                {
                    switch(choice)
                    {
                        case 1:
                            Console.Clear();
                            CreateWallet(allWallets,fungibleAssetList,nonFungibleAssetList);
                            break;
                        case 2:
                            Console.Clear();
                            AccessWallet(allWallets,fungibleAssetList,nonFungibleAssetList);
                            break;
                        default :
                            break;
                    }
                }
            }while(choice!=4);

        }

        static bool UserInput()
        {
            Console.WriteLine("If you want to confirm all changes enter 'yes' ");
            var answer = Console.ReadLine();
            if (answer.ToLower() == "yes")
                return true;
            return false;
        }
        static void Options()
        {
            Console.WriteLine("=========CRYPTO WALLET=========");
            Console.WriteLine("\nWelcome to your own crypto wallet!");
            Console.WriteLine("Your options are:");
            Console.WriteLine("1.Create new wallet\n"+
                              "2.Access your wallet\n");
        }
        static void SubmenuOptions()
        {
            Console.WriteLine("\n\n----------SUBMENU----------");
            Console.WriteLine("Please enter the action you want to make: ");
            Console.WriteLine("1.Portafolio\n2.Transfer\n3.Transaction history\n4.Go back to main menu\n");
        }
        static bool CheckAddress(Guid walletGuid,Guid receiverWalletAddress,Dictionary<Guid,Wallet>allWallets)
        {
            if(walletGuid.ToString()==receiverWalletAddress.ToString())
            {
                Console.WriteLine("You entered the address of the wallet you are currently in!");
                return false;
            }
            if(!allWallets.ContainsKey(receiverWalletAddress))
            {
                Console.WriteLine("Choosen wallet not found! ");
                return false;
            }
            return true;
        }
        static void CreateWallet(Dictionary<Guid,Wallet>allWallets,List<FungibleAssets>fungibleAssetList,List<NonFungibleAssets>nonFungibleAssetList)
        {
            Console.WriteLine("Please enter the number for the wallet you want to create:");
            Console.WriteLine("1.Bitcoin wallet\n2.Ethereum wallet\n3.Solana wallet\n0.Go back to main page");
            var choiceSuccess=int.TryParse(Console.ReadLine(),out var choice);

            if(choiceSuccess)
            {
                switch(choice)
                {
                    case 1:
                        Console.WriteLine("You chose to create new bitcoin wallet!");
                        if(UserInput())
                        {
                            BitcoinWallet newBitcoinWallet=new(fungibleAssetList);
                            allWallets.Add(newBitcoinWallet.Address,newBitcoinWallet);
                            Console.WriteLine("You have added another wallet!\n");
                        }
                        break;
                    case 2:
                        Console.WriteLine("You chose to create new ethereum wallet!");
                        if(UserInput())
                        {
                            EthereumWallet newEthereumWallet=new(fungibleAssetList,nonFungibleAssetList);
                            allWallets.Add(newEthereumWallet.Address,newEthereumWallet);
                            Console.WriteLine("You have added another wallet!\n");

                        }
                        break;
                    case 3:
                        Console.WriteLine("You chose to create new solana wallet!");
                        if(UserInput())
                        {
                            SolanaWallet newSolanaWallet=new(fungibleAssetList,nonFungibleAssetList);
                            allWallets.Add(newSolanaWallet.Address,newSolanaWallet);
                            Console.WriteLine("You have added another wallet!\n");
                        }
                        break;
                    case 0:
                        Console.Clear();
                        break;    
                    default:
                        break;
                }
            }
        }
        static void AccessWallet(Dictionary<Guid,Wallet>allWallets,List<FungibleAssets>fungibleAssetList,List<NonFungibleAssets>nonFungibleAssetList)
        {
            Console.Clear();
            var waletTable = new ConsoleTable("Wallet address", "Wallet type", "Total asset value");

            foreach(var item in allWallets)
            {
                waletTable.AddRow(item.Key,item.Value.WalletTypes,item.Value.TotalValueOfAssetsInUSD(fungibleAssetList,nonFungibleAssetList));
            }
            waletTable.Write(Format.Minimal);
            

            Console.WriteLine("Please enter the address of the wallet you want to access:");
            Guid walletGuid=Guid.Parse(Console.ReadLine());
            if(!allWallets.ContainsKey(walletGuid))
            {
                Console.WriteLine("Choosen wallet not found! ");
                return;
            }
            //Console.Clear();
            var choice=-1;
            do{
                SubmenuOptions();
                int.TryParse(Console.ReadLine(),out choice);
                switch(choice)
                {
                    case 1:
                        Portafolio(walletGuid,allWallets,fungibleAssetList,nonFungibleAssetList);
                        break;
                    case 2:
                        Transfer(walletGuid,allWallets,fungibleAssetList,nonFungibleAssetList);
                        break;
                    case 3:
                        TransactionHistory(allWallets,walletGuid);
                        break;
                    case 4:
                        break;
                    default:
                        break;
                }
            }while(choice!=4);
        }
        static void Portafolio(Guid walletAddress,Dictionary<Guid,Wallet>allWallets,List<FungibleAssets>fungibleAssetList,List<NonFungibleAssets>nonFungibleAssetList)
        {
            allWallets.TryGetValue(walletAddress,out Wallet wallet);

            Console.WriteLine($"Total value in USD: {(decimal)wallet.TotalValueOfAssetsInUSD(fungibleAssetList,nonFungibleAssetList)}");
            Console.WriteLine("\nFungible assets information:\n");

            var fungibleAssetTable = new ConsoleTable("Address", "Name", "Label", "Crypto value", "Total Value in USD","Value change");

            foreach(var item in wallet.FungibleAssetsBalance)//samo fungible assets
            {
                var temp=fungibleAssetList.Find(x=>x.Address.Equals(item));
                var asset=fungibleAssetList.Find(x => x.Address.Equals(item.Key));

                fungibleAssetTable.AddRow(asset.Address,asset.Name,asset.GetLabel(),item.Value,asset.GetValueInUSD(),$"{asset.ValueChangeInPercentage}%");
            }
            fungibleAssetTable.Write(Format.Minimal);

            Console.WriteLine("\nNot fungible assets information:\n ");
            if(wallet.WalletTypes is "Bitcoin wallet")
            {
                Console.WriteLine("Bitcoin wallet doesn't have any non fungible assets!\n");
                return;
            } 

            var nonFungibleAssetTable = new ConsoleTable("Address", "Name","Crypto value","Total value in USD");
            foreach(var item in wallet.OwnedNonFungibleAssets)
            {
                var nfAsset=nonFungibleAssetList.Find(x => x.Address.Equals(item));
                FungibleAssets allowedFungible=nfAsset.GetAllowedFungibleAsset(fungibleAssetList);
                var cryptoValue=nfAsset.GetValueInUSD(fungibleAssetList);

                nonFungibleAssetTable.AddRow(nfAsset.Address,nfAsset.Name,nfAsset.Value,$"{cryptoValue}");
            }
            nonFungibleAssetTable.Write(Format.Minimal);
        }
        static void Transfer(Guid walletGuid,Dictionary<Guid,Wallet>allWallets,List<FungibleAssets>fungibleAssetList,List<NonFungibleAssets>nonFungibleAssetList)
        {
            Console.WriteLine("Enter the address of the receiver wallet");
            Guid receiverWalletAddress=Guid.Parse(Console.ReadLine());
            if(!CheckAddress(walletGuid,receiverWalletAddress,allWallets))
                return;

            Console.WriteLine("Enter the asset address you want to transfer");
            Guid assetAddress=Guid.Parse(Console.ReadLine());

            bool isFungable=false;
            foreach(var item in fungibleAssetList)
            {
                if(item.Address.Equals(assetAddress))
                    isFungable=true;
            }
            if(isFungable)
            {
                Console.WriteLine("Please enter the amount you want to transfer: ");
                bool success=double.TryParse(Console.ReadLine(),out double amount);
                if(!success)
                {
                    Console.WriteLine("wrong input!");
                    return;
                }

                if(UserInput())
                {
                    allWallets[walletGuid].MakeFungibleTransaction(allWallets[receiverWalletAddress],assetAddress,amount);
                    FungibleAssets asset=fungibleAssetList.Find(x=>x.Address.Equals(assetAddress));
                    asset.ChangeAssetValue();
                }
                else Console.WriteLine("The action has been stopped!");
            }
            else
            {
                allWallets[walletGuid].MakeNonFungibleTransaction(allWallets[receiverWalletAddress],assetAddress);
        
                NonFungibleAssets nfAsset=nonFungibleAssetList.Find(x=>x.Address.Equals(assetAddress));
                foreach(var item in fungibleAssetList)
                {
                    if(item.Address==nfAsset.AllowedFungibleAssetAddress)
                        item.ChangeAssetValue();
                }
            }
        }
        static void TransactionHistory(Dictionary<Guid,Wallet>allWallets,Guid walletAddress)
        {
            Console.Clear();
            Wallet walletOfInterest=allWallets[walletAddress];

            walletOfInterest.PrintTransaction();

            Console.WriteLine("\n\n----------SUBMENU----------");
            Console.WriteLine("1.Cancel transaction\n2.Go back one page");
            int.TryParse(Console.ReadLine(),out var choice);
            if(choice==2)
                return;
            if(choice!=2 && choice!=1)
            {
                Console.WriteLine("Invalid input!\n");
                return;
            }
            
            Console.Write("\nPlease enter the ID of the transaction you want to cancel: ");
            Guid transactionId=Guid.Parse(Console.ReadLine());

            if(!walletOfInterest.TransactionHistory.ContainsKey(transactionId))
                return;
            
            Transaction foundTransaction=walletOfInterest.TransactionHistory[transactionId];
            Wallet senderWallet=allWallets[foundTransaction.SenderAddress];
            Wallet receiverWallet=allWallets[foundTransaction.ReceiverAddress];

            if(!UserInput())
            {
                Console.WriteLine("Action has been stopped!");
                return;
            }

            if (DateTime.Now.Subtract(foundTransaction.TransactionDate).TotalSeconds > 45)
            {
                Console.WriteLine("You ran out of time to compleate the cancelation!\n");
                return;
            }
            foundTransaction.CancelTransaction(senderWallet,receiverWallet);
            //return;
        }
        static List<FungibleAssets> AddFungibleAssets()
        {
            var bitcoin=new FungibleAssets("bitcoin",16989.53,"BTC");
            var etherum=new FungibleAssets("etherum",1207,"ETH");
            var dogecoin=new FungibleAssets("dogecoin",0.1,"DOGE");
            var cosmos=new FungibleAssets("cosmos",10.22,"ATOM");
            var bnb=new FungibleAssets("bnb",290.63,"BNB");
            var xrp=new FungibleAssets("xrp",0.39,"XRP");
            var solana=new FungibleAssets("solana",13.5,"SOL");
            var tether=new FungibleAssets("tether",1.1,"USDT");
            var polygon=new FungibleAssets("polygon",0.83,"MATIC");
            var polkadot=new FungibleAssets("polkadot",5.26,"DOT");

            List<FungibleAssets>fungibleAssetList=new List<FungibleAssets>()
            {bitcoin,etherum,dogecoin,cosmos,bnb,xrp,solana,tether,polygon,polkadot};

            return fungibleAssetList;
        }

        static List<NonFungibleAssets>AddNonFungibleAssets(List<FungibleAssets>fungibleAssetList)
        {
            var devilApe=new NonFungibleAssets("Devil-Ape", 1.33,fungibleAssetList[0].Address);
            var regularApe=new NonFungibleAssets("Regular-Ape", 1,fungibleAssetList[0].Address);
            var punkApe=new NonFungibleAssets("Punk-Ape", 20,fungibleAssetList[1].Address);
            var shyApe=new NonFungibleAssets("Shy-Ape", 30000,fungibleAssetList[1].Address);
            var moonbirds=new NonFungibleAssets("Moonbirds#1748",8.74,fungibleAssetList[1].Address);
            var pudgyPenguin=new NonFungibleAssets("PudgyPenguin#212", 3.1,fungibleAssetList[1].Address);
            var pudgyPenguin2=new NonFungibleAssets("PudgyPenguin#3416", 3.69,fungibleAssetList[1].Address);
            var cloneX1=new NonFungibleAssets("CloneX #8710", 7.45,fungibleAssetList[1].Address);
            var cloneX2=new NonFungibleAssets("CloneX #9924", 7.8,fungibleAssetList[1].Address);
            var moonbirds1=new NonFungibleAssets("Moonbirds #6480", 9.84,fungibleAssetList[1].Address);
            var potatoz=new NonFungibleAssets("Potatoz #3377", 1.68,fungibleAssetList[1].Address);
            var moonbirds2=new NonFungibleAssets("Moonbirds #9509", 9.85,fungibleAssetList[1].Address);
            var potatoz1=new NonFungibleAssets("Potatoz #4413", 1.77,fungibleAssetList[1].Address);
            var invisible=new NonFungibleAssets("Invisible friends #3640", 2.39,fungibleAssetList[1].Address);
            var bean1=new NonFungibleAssets("Bean #10476", 0.95,fungibleAssetList[1].Address);
            var bean2=new NonFungibleAssets("Bean #13560", 1.05,fungibleAssetList[1].Address);
            var pokePxls1=new NonFungibleAssets("PokePxls #147", 0.03,fungibleAssetList[1].Address);
            var pokePxls2=new NonFungibleAssets("PokePxls #150", 0.07,fungibleAssetList[1].Address);
            var character1=new NonFungibleAssets("Character #2728", 7.78,fungibleAssetList[1].Address);
            var character2=new NonFungibleAssets("Character #1816", 5.6,fungibleAssetList[1].Address);

            List<NonFungibleAssets>nonFungibleAssetList=new List<NonFungibleAssets>()
            {devilApe,regularApe,punkApe,shyApe,moonbirds,pudgyPenguin,pudgyPenguin2,cloneX1,cloneX2,moonbirds1,moonbirds2,potatoz,potatoz1,invisible,bean1,bean2,pokePxls1,pokePxls2,character1,character2};

            return nonFungibleAssetList;
        }
        static Dictionary<Guid,Wallet> GetWallets(List<FungibleAssets>fungibleAssetList,List<NonFungibleAssets>nonFungibleAssetList)
        {

            Dictionary<Guid,Wallet> allWallets=new Dictionary<Guid, Wallet>();

            BitcoinWallet bitcoinWallet1=new(fungibleAssetList);
            BitcoinWallet bitcoinWallet2=new(fungibleAssetList);
            BitcoinWallet bitcoinWallet3=new(fungibleAssetList);
    
            allWallets.Add(bitcoinWallet1.Address,bitcoinWallet1);
            allWallets.Add(bitcoinWallet2.Address,bitcoinWallet2);
            allWallets.Add(bitcoinWallet3.Address,bitcoinWallet3);

            EthereumWallet ethereumWallet1=new(fungibleAssetList,nonFungibleAssetList);
            EthereumWallet ethereumWallet2=new(fungibleAssetList,nonFungibleAssetList);
            EthereumWallet ethereumWallet3=new(fungibleAssetList,nonFungibleAssetList);

            for(var i=10;i<14;i++)
                ethereumWallet1.OwnedNonFungibleAssets.Add(nonFungibleAssetList[i].Address);
            for(var i=14;i<17;i++)
                ethereumWallet2.OwnedNonFungibleAssets.Add(nonFungibleAssetList[i].Address);
            for(var i=17;i<20;i++)
                ethereumWallet3.OwnedNonFungibleAssets.Add(nonFungibleAssetList[i].Address);

            allWallets.Add(ethereumWallet1.Address,ethereumWallet1);//ako stavin enum pa dodan koja su
            allWallets.Add(ethereumWallet2.Address,ethereumWallet2);
            allWallets.Add(ethereumWallet3.Address,ethereumWallet3);

            SolanaWallet solanaWallet1=new(fungibleAssetList,nonFungibleAssetList);
            SolanaWallet solanaWallet2=new(fungibleAssetList,nonFungibleAssetList);
            SolanaWallet solanaWallet3=new(fungibleAssetList,nonFungibleAssetList);

            for(var i=0;i<4;i++)
                solanaWallet1.OwnedNonFungibleAssets.Add(nonFungibleAssetList[i].Address);
            for(var i=4;i<7;i++)
                solanaWallet2.OwnedNonFungibleAssets.Add(nonFungibleAssetList[i].Address);
            for(var i=7;i<10;i++)
                solanaWallet3.OwnedNonFungibleAssets.Add(nonFungibleAssetList[i].Address);
    

            allWallets.Add(solanaWallet1.Address,solanaWallet1);
            allWallets.Add(solanaWallet2.Address,solanaWallet2);
            allWallets.Add(solanaWallet3.Address,solanaWallet3);

            return allWallets;

        }

    }
}
