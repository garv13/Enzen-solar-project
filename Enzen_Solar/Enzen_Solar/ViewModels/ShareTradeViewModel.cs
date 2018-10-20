using Enzen_Solar.Models;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Enzen_Solar.ViewModels
{
    public class NewShareListViewModel
    {
        public NewShareListViewModel()
        {

        }

        public string ListQuantity { get; set; }

        public string ListRate { get; set; }

        public string QuantityPurchase { get; set; }

        public ICommand BuyCommand { get; set; }
    }

    public class SecondaryShareListViewModel
    {
        public SecondaryShareListViewModel()
        {

        }

        public string ListQuantity { get; set; }

        public string ListRate { get; set; }

        public string QuantityPurchase { get; set; }

        public ICommand BuyCommand { get; set; }
    }

    public class MyShareListViewModel
    {
        public MyShareListViewModel()
        {

        }

        public string ListQuantity { get; set; }

        public string QuantityPurchase { get; set; }

        public string PurchaseRate { get; set; }

        public ICommand SellCommand { get; set; }
    }

    public class ShareTradeViewModel : BindableObject
    {
        public ObservableCollection<NewShareListViewModel> NewShareListViewObj { get; set; }
        public ObservableCollection<SecondaryShareListViewModel> SecondaryShareListViewObj { get; set; }
        public ObservableCollection<MyShareListViewModel> MyShareListViewObj { get; set; }

        private MobileServiceCollection<Share, Share> UserShareList;
        private MobileServiceCollection<Share, Share> SecondaryShareList;

        IMobileServiceTable<Share> userShareTable;
        public ICommand MyShareSellCommand { get; private set; }
        public ICommand NewShareBuyCommand { get; set; }
        public ICommand SecondaryShareBuyCommand { get; set; }

        public ShareTradeViewModel()
        {
            NewShareListViewObj = new ObservableCollection<NewShareListViewModel>();
            SecondaryShareListViewObj = new ObservableCollection<SecondaryShareListViewModel>();
            MyShareListViewObj = new ObservableCollection<MyShareListViewModel>();

            MyShareSellCommand = new Command<MyShareListViewModel>(handleMyShareSellCommand);
            NewShareBuyCommand = new Command<NewShareListViewModel>(handleNewShareBuyCommand);
            SecondaryShareBuyCommand = new Command<SecondaryShareListViewModel>(handleSecondaryShareBuyCommand);


            NewShareListViewObj.Add(new NewShareListViewModel { ListQuantity = "21", ListRate = "32", BuyCommand = NewShareBuyCommand });
            SecondaryShareListViewObj.Add(new SecondaryShareListViewModel { ListQuantity = "11", ListRate = "37", BuyCommand = SecondaryShareBuyCommand });
        }

        private async void GetUserShares()
        {
            UserShareList.Clear();
            UserShareList = await userShareTable.Where
                (Share => Share.UserId == App.UserID && Share.IsTradeable == false).ToCollectionAsync();
            if (UserShareList.Count != 0)
            {
                // group by year
                MyShareListViewModel tp = new MyShareListViewModel();
                tp.ListQuantity = UserShareList.Count.ToString();
                tp.SellCommand = MyShareSellCommand;
                MyShareListViewObj.Add(tp);
            }
        }

        private async void PopulateSecondaryMarket()
        {
            SecondaryShareList = await userShareTable.Where(Share => Share.IsTradeable == false).ToCollectionAsync();
            var tp = new ObservableCollection<Share>(SecondaryShareList);
            
            //if(SecondaryShareList.Count != 0)
            //{
            //    foreach(Share tp in SecondaryShareList)
            //    {

            //    }
            //}
        }
        private async void handleMyShareSellCommand(MyShareListViewModel item)
        {
            var price = int.Parse(item.PurchaseRate);
            var qty = int.Parse(item.QuantityPurchase);
            for(int i = 0; i < qty; i++)
            {
                UserShareList[i].IsTradeable = true;
                UserShareList[i].Price = price;
                await App.MobileService.GetTable<Share>().UpdateAsync(UserShareList[i]);              
            }
        }


        private async void handleNewShareBuyCommand(NewShareListViewModel item)
        {
            var e = item.ListQuantity;
            var w = item.ListRate;
        }


        private async void handleSecondaryShareBuyCommand(SecondaryShareListViewModel item)
        {
            var e = item.ListQuantity;
            var w = item.ListRate;
        }

    }
}
