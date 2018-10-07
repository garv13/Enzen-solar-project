using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Enzen_Solar.Views
{
    class DashboardDataView
    {
        public string WalletBalance { get; set; }

        public string CoinBalance { get; set; }

        public string ShareBalance { get; set; }
    }
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
            DashboardDataView db = new DashboardDataView();
            db.CoinBalance = "18";
            db.ShareBalance = "6";
            db.WalletBalance = "22.5";
            MainGrid.BindingContext = db;
		}

        private void AddWalletButton_Clicked(object sender, EventArgs e)
        {

        }

        private void WithdrawWalletButton_Clicked(object sender, EventArgs e)
        {

        }

        private void PayBillButton_Clicked(object sender, EventArgs e)
        {

        }

        private void TradeCoinButton_Clicked(object sender, EventArgs e)
        {

        }

        private void TradeShareButton_Clicked(object sender, EventArgs e)
        {

        }
    }
}
