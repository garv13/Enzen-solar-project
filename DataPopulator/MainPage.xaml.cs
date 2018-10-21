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

namespace DataPopulator
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// 
    /// 
    /// Adds 3 users
    /// 1 roof
    /// 100 shares distributed as 5%, 30%, 65%
    /// 1 mapping
    /// 3 user credit
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            //adding mapping
            Mapping item = new Mapping();
            item.DeviceId = "5f37a6af-7b02-471d-b5a7-8ec57d1f6d43";
            item.RoofId = 1;
            await App.MobileService.GetTable<Mapping>().InsertAsync(item);
            //item = new Mapping();
            //item.DeviceId = "c114a1a2-43c5-4b63-bcb2-4efe65c2cc82";
            //item.RoofId = 2;
            //await App.MobileService.GetTable<Mapping>().InsertAsync(item);

            //adding users

            User u = new User
            { Email = "akg@akg.com",
              FirstName = "akshay",
              LastName="Gupta",
              Mobile ="9876543210",
              Password ="1234",
              UserName = "Axe",
              UseriD =1
            };
            await App.MobileService.GetTable<User>().InsertAsync(u);

            u = new User
            {
                Email = "Garv@akg.com",
                FirstName = "Garv",
                LastName = "Jasuja",
                Mobile = "1234567890",
                Password = "1234",
                UserName = "Garv",
                UseriD = 2
            };
            await App.MobileService.GetTable<User>().InsertAsync(u);

            u = new User
            {
                Email = "Ankur@akg.com",
                FirstName = "Ankur",
                LastName = "Gupta",
                Mobile = "9873216540",
                Password = "1234",
                UserName = "Ankur",
                UseriD = 3
            };
            await App.MobileService.GetTable<User>().InsertAsync(u);

            u = new User
            {
                Email = "admin@akg.com",
                FirstName = "admin",
                LastName = "solar",
                Mobile = "17982023",
                Password = "1234",
                UserName = "admin",
                UseriD = 0
            };
            //await App.MobileService.GetTable<User>().InsertAsync(u);
            //u = new User
            //{
            //    Email = "billi@akg.com",
            //    FirstName = "Ram",
            //    LastName = "Gupta",
            //    Mobile = "9876543210",
            //    Password = "1234",
            //    UserName = "Ram",
            //    UseriD = 5
            //};
            //await App.MobileService.GetTable<User>().InsertAsync(u);


            //roof
            Roof r = new Roof {
                 Investment=100000,
                 RoofId=1,
                  Percentage=5,
                   Potential=1,
                    SharesAvailable=0,
                     TotalShares=100,
                      Size=100,
                       UserId=1,
                        CostPerShare = 7829190123,
                         latitude=30.12,
                          longitude=-73.12

            };
            await App.MobileService.GetTable<Roof>().InsertAsync(r);

            // shares

            for (int i = 0; i < 100; i++)
            {

                Share s = new Share();
                s.IsTradeable = false;
                s.Price = 10;
                s.ShareId = i + 1;
                s.RoofId = 1;
                if (i < 5)
                {
                    s.UserId = 1;
                }
                else if(i<35)
                {
                    s.UserId = 2;

                }
                else
                { s.UserId = 3; }

                await App.MobileService.GetTable<Share>().InsertAsync(s);

            }
            // User Credit
            UserCredit uc = new UserCredit {
             UserId=1,
             CoinsMined= 10000,
             WalletBalance=100000,
             TradeCoins=1000,
             shares = 5};
            await App.MobileService.GetTable<UserCredit>().InsertAsync(uc);

            uc = new UserCredit
            {
                UserId = 2,
                CoinsMined = 7000,
                WalletBalance = 100000,
                TradeCoins = 500,
                shares = 30
            };
            await App.MobileService.GetTable<UserCredit>().InsertAsync(uc);
            uc = new UserCredit
            {
                UserId = 3,
                CoinsMined = 100000,
                WalletBalance = 100000,
                TradeCoins = 10000,
                shares = 65
            };
            uc = new UserCredit
            {
                UserId = 0,
                CoinsMined = 0,
                WalletBalance = 100000,
                TradeCoins = 1000,
                shares = 0
            };
            
            await App.MobileService.GetTable<UserCredit>().InsertAsync(uc);



        }

        private async void button1_Click(object sender, RoutedEventArgs e)
        {
           
            
        }
    }
    }

