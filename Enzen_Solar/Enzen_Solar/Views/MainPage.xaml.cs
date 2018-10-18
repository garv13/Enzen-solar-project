using Enzen_Solar.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Enzen_Solar.Views
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
            MainGrid.BindingContext = new MainPageViewModel();
		}

        private void AddWalletButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddMoneyPage());
        }

        private void WithdrawWalletButton_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Oops", "This feature is coming soon", "CLOSE");
        }

        private void PayBillButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PayBillPage());
        }

        private void TradeCoinButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CoinTradePage());
        }

        private void TradeShareButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ShareTradePage());
        }
    }
}
