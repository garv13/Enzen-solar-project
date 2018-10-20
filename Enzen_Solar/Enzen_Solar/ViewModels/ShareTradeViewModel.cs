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

        public string ListYear { get; set; }

        public string QuantityPurchase { get; set; }

        public string PurchaseRate { get; set; }

        public ICommand SellCommand { get; set; }
    }

    public class ShareTradeViewModel : BindableObject
    {
        public ObservableCollection<NewShareListViewModel> NewShareListViewObj { get; set; }
        public ObservableCollection<SecondaryShareListViewModel> SecondaryShareListViewObj { get; set; }
        public ObservableCollection<MyShareListViewModel> MyShareListViewObj { get; set; }

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
            MyShareListViewObj.Add(new MyShareListViewModel { ListQuantity = "21", ListYear = "2020", SellCommand = MyShareSellCommand });

        }

        private async void handleMyShareSellCommand(MyShareListViewModel item)
        {
            var e = item.ListQuantity;
            var w = item.ListYear;
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
