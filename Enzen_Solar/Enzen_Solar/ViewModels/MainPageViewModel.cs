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
                MobileServiceCollection<Share, Share> UserShareList;
                IMobileServiceTable<Share> userShareTable;
                userShareTable = App.MobileService.GetTable<Share>();

                IMobileServiceTable<UserCredit> userCreditTable = App.MobileService.GetTable<UserCredit>();

                var list = await App.MobileService.GetTable<Share>().Take(0).Where
                    (Share => Share.UserId == App.UserID && Share.IsTradeable == false).IncludeTotalCount().ToListAsync();

                int tCount = (int)((IQueryResultEnumerable<Share>)list).TotalCount;

                int icount = tCount;
                int skip = 0;
                while (icount >= 0)
                {
                    list.AddRange(await App.MobileService.GetTable<Share>().Skip(skip).Take(50).Where
                    (Share => Share.UserId == App.UserID && Share.IsTradeable == false).IncludeTotalCount().ToListAsync());
                    skip += 50;
                    icount -= 50;
                }
                    
                MobileServiceCollection<UserCredit, UserCredit> UserCreditList = await userCreditTable.Where
                    (UserCredit => UserCredit.UserId == App.UserID).ToCollectionAsync();

                if (UserCreditList.Count == 1)
                {
                    WalletBalance = int.Parse(UserCreditList[0].WalletBalance) < 0 ? "0" : UserCreditList[0].WalletBalance;
                    CoinBalance = Math.Round(float.Parse(UserCreditList[0].TradeCoins), 2).ToString();
                    ShareBalance = list.Count.ToString();
                }
            }
            catch (Exception e)
            {

            }
        }
    }
}
