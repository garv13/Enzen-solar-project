using Enzen_Solar.Models;
using Enzen_Solar.Views;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Enzen_Solar.ViewModels
{
    public class ListViewModel
    {
        public ListViewModel()
        {
            
        }
        public string QuantityPurchase { get; set; }

        public string ListQuantity { get; set; }

        public string ListRate { get; set; }

        public ICommand BuyCommand { get; set; }
    }
    public class CoinTradeViewModel : BindableObject
    {
        IMobileServiceTable<Market> marketTable;
        IMobileServiceTable<UserCredit> userCreditTable;

        INavigation Navigation;
        private MobileServiceCollection<Market, Market> MarketList;

        public CoinTradeViewModel(INavigation navigation)
        {            
            Navigation = navigation;
            marketTable = App.MobileService.GetTable<Market>();
            userCreditTable = App.MobileService.GetTable<UserCredit>();

            ListViewObj = new ObservableCollection<ListViewModel>();
            BuyCommand = new Command<ListViewModel>(handleBuyCoinCommand);
            SellCommand = new Command(handleSellCoinCommand);
            PopulateMarketList();
        }

        public ObservableCollection<ListViewModel> ListViewObj { get; set; }
        private string _quantity { get; set; }
        private string _rate { get; set; }

        public ICommand SellCommand { get; private set; }
        public ICommand BuyCommand { get; set; }
        public string Quantity
        {
            get
            {
                return _quantity;
            }
            set
            {
                _quantity = value;
                OnPropertyChanged("Quantity");
            }
        }

        public string Rate
        {
            get
            {
                return _rate;
            }
            set
            {
                _rate = value;
                OnPropertyChanged("Rate");
            }
        }

        private async void PopulateMarketList()
        {
            MarketList = await marketTable.ToCollectionAsync();
            if (MarketList.Count != 0)
            {
                var orderedList = MarketList.OrderBy(x => x.PerCoinRate).ToList();
                foreach(Market mk in orderedList)
                {
                    ListViewModel tp = new ListViewModel();
                    tp.ListQuantity = mk.CoinQuantity.ToString();
                    tp.ListRate = mk.PerCoinRate.ToString();
                    tp.BuyCommand = BuyCommand;
                    ListViewObj.Add(tp);
                }                 
            }
        }
        private async void handleSellCoinCommand()
        {
            try
            {
                int qty = int.Parse(_quantity);
                int rate = int.Parse(_rate);
                Market temp = new Market();
                temp.PerCoinRate = rate;
                temp.CoinQuantity = qty;
                temp.UserId = App.UserID;
                await marketTable.InsertAsync(temp);
                await Navigation.PushModalAsync(new HamburgerMenu());
            }
            catch(Exception e)
            {

            }            
        }

        private async void handleBuyCoinCommand(ListViewModel item)
        {
            try
            {
                int price = int.Parse(item.ListRate);
                int qty = int.Parse(item.QuantityPurchase);

                int cost = price * qty;

                var CoinList = MarketList.Where(x => x.PerCoinRate == price).ToList();

                int SellerUserId = CoinList[0].UserId;
                CoinList[0].CoinQuantity -= qty;

                if (CoinList[0].CoinQuantity == 0)
                    await marketTable.DeleteAsync(CoinList[0]);
                else
                    await marketTable.UpdateAsync(CoinList[0]);

                var Buyer = await userCreditTable.Where(x => x.UserId == App.UserID).ToCollectionAsync();
                Buyer[0].WalletBalance = (int.Parse(Buyer[0].WalletBalance) - cost).ToString();
                Buyer[0].Shares = Buyer[0].Shares + qty;
                await userCreditTable.UpdateAsync(Buyer[0]);

                var Seller = await userCreditTable.Where(x => x.UserId == SellerUserId).ToCollectionAsync();
                Seller[0].WalletBalance = (int.Parse(Seller[0].WalletBalance) + cost).ToString();
                await userCreditTable.UpdateAsync(Seller[0]);
                await Navigation.PushModalAsync(new HamburgerMenu());
            }
            catch (Exception e)
            {

            }            
        }
    }
}
