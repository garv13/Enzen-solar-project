using Enzen_Solar.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Enzen_Solar.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CoinTradePage : ContentPage
	{
		public CoinTradePage ()
		{
			InitializeComponent();
            MainGrid.BindingContext = new CoinTradeViewModel();
		}

        private void PayBillButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PayBillPage());
        }
    }
}