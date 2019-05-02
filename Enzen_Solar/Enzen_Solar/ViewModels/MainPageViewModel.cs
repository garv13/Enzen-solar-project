using Enzen_Solar.Models;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace Enzen_Solar.ViewModels
{
    public class MainPageViewModel : BindableObject
    {
        private string _walletBalance;
        private string _coinBalance;
        private string _shareBalance;

        public string WalletBalance
        {
            get
            {
                return _walletBalance;
            }
            set
            {
                _walletBalance = value;
                OnPropertyChanged("WalletBalance");
            }
        }

        public string CoinBalance
        {
            get
            {
                return _coinBalance;
            }
            set
            {
                _coinBalance = value;
                OnPropertyChanged("CoinBalance");
            }
        }
        public string ShareBalance
        {
            get
            {
                return _shareBalance;
            }
            set
            {
                _shareBalance = value;
                OnPropertyChanged("ShareBalance");
            }
        }
        public MainPageViewModel()
        {
            try
            {
                getUserData();
            }
            catch (Exception e)
            {
                return;
            }
        }
        private async void getUserData()
        {
            try
            {
                IMobileServiceTable<UserCredit> userCreditTable = App.MobileService.GetTable<UserCredit>();
                MobileServiceCollection<UserCredit, UserCredit> UserCreditList = await userCreditTable.Where
                    (UserCredit => UserCredit.UserId == App.UserID).ToCollectionAsync();
                if (UserCreditList.Count == 1)
                {
                    WalletBalance = int.Parse(UserCreditList[0].WalletBalance) < 0 ? "0" : UserCreditList[0].WalletBalance;
                    CoinBalance = Math.Round(float.Parse(UserCreditList[0].TradeCoins), 2).ToString();
                    ShareBalance = UserCreditList[0].Shares.ToString();
                }
            }
            catch (Exception e)
            {

            }
        }
    }
}
