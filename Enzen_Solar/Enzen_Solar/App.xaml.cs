using Enzen_Solar.Views;
using Microsoft.WindowsAzure.MobileServices;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace Enzen_Solar
{
	public partial class App : Application
	{
        public App ()
		{
			InitializeComponent();
            UserID = 1;
			MainPage = new HamburgerMenu();
		}
        public static int UserID;

        public static MobileServiceClient MobileService = new MobileServiceClient("https://iotsolar.azurewebsites.net");

        protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}

	}
}
