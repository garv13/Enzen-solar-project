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
            try
            {
                InitializeComponent();
                UserID = 2;
                MainPage = new LoginPage();
            }
            catch(Exception e)
            {

            }
			
		}
        public static int UserID;

        public const int AdminUserID = 0;

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
