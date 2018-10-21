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
	public partial class ShareTradePage : ContentPage
	{
		public ShareTradePage ()
		{
			InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MainGrid.BindingContext = new ShareTradeViewModel(Navigation);
        }
    }
}