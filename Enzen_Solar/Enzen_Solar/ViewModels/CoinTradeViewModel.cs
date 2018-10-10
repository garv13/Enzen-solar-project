using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public string ListQuantity { get; set; }

        public string ListRate { get; set; }

        public ICommand BuyCommand { get; set; }
    }
    public class CoinTradeViewModel : BindableObject
    {
        public CoinTradeViewModel()
        {
            _quantity = "1";
            _rate = "2";
            ListViewObj = new ObservableCollection<ListViewModel>();
            BuyCommand = new Command<ListViewModel>(handleBuyCoinCommand);
            SellCommand = new Command(handleSellCoinCommand);
            ListViewObj.Add(new ListViewModel { ListQuantity = "21", ListRate = "32", BuyCommand = BuyCommand});
            ListViewObj.Add(new ListViewModel { ListQuantity = "11", ListRate = "37" , BuyCommand=BuyCommand});        
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
                OnPropertyChanged(Quantity);
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
                OnPropertyChanged(Rate);
            }
        }

        private void handleSellCoinCommand()
        {
            var a = _rate;
            var b = 3;
        }

        private void handleBuyCoinCommand(ListViewModel item)
        {
            var e = item.ListQuantity;
            var w = item.ListRate;
        }
    }
}
