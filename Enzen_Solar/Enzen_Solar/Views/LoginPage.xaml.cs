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
	public partial class LoginPage : ContentPage
	{
		public LoginPage ()
		{
			InitializeComponent ();
		}

        private void LoginButton_Clicked(object sender, EventArgs e)
        {
            App.UserID = int.Parse(UsernameEntry.Text);
            Navigation.PushModalAsync(new HamburgerMenu());
        }
    }
}