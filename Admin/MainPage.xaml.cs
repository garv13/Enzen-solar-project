using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Admin
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        IMobileServiceTable<UserCredit> userCreditTable;
        private MobileServiceCollection<UserCredit, UserCredit> UserShareList;
        public MainPage()
        {
            this.InitializeComponent();
            Loaded += MainPage_Loaded;
        }

        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            userCreditTable = App.MobileService.GetTable<UserCredit>();
            UserShareList = await userCreditTable.Where(UserCredit => UserCredit.UserId == 0).ToCollectionAsync();
            UserCredit c = UserShareList[0];
            Investment.Text = c.WalletBalance.ToString();
            Coin.Text = c.TradeCoins.ToString();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string s = deviceId.Text;
            Mapping map = new Mapping { DeviceId = s, RoofId = 2 };
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Heatmap));
            //navogate to next page
        }
    }
}
