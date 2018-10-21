using Enzen_Solar.Models;
using Enzen_Solar.Views;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Enzen_Solar.ViewModels
{
    public class NewShareListViewModel
    {
        public NewShareListViewModel()
        {

        }

        public string ListQuantity { get; set; }

        public string ListRate { get; set; }

        public string QuantityPurchase { get; set; }

        public int RoofID { get; set; }

        public ICommand BuyCommand { get; set; }
    }

    public class SecondaryShareListViewModel
    {
        public SecondaryShareListViewModel()
        {

        }

        public string ListQuantity { get; set; }

        public string ListRate { get; set; }

        public string QuantityPurchase { get; set; }

        public ICommand BuyCommand { get; set; }
    }

    public class MyShareListViewModel
    {
        public MyShareListViewModel()
        {

        }

        public string ListQuantity { get; set; }

        public string QuantityPurchase { get; set; }

        public string PurchaseRate { get; set; }

        public ICommand SellCommand { get; set; }
    }

    public class ShareTradeViewModel : BindableObject
    {
        public ObservableCollection<NewShareListViewModel> NewShareListViewObj { get; set; }
        public ObservableCollection<SecondaryShareListViewModel> SecondaryShareListViewObj { get; set; }
        public ObservableCollection<MyShareListViewModel> MyShareListViewObj { get; set; }

        private MobileServiceCollection<Share, Share> UserShareList;
        private MobileServiceCollection<Share, Share> SecondaryShareList;
        private MobileServiceCollection<Roof, Roof> RoofList;

        IMobileServiceTable<Share> userShareTable;
        IMobileServiceTable<UserCredit> userCreditTable;
        IMobileServiceTable<Roof> roofTable;

        public ICommand MyShareSellCommand { get; private set; }
        public ICommand NewShareBuyCommand { get; set; }
        public ICommand SecondaryShareBuyCommand { get; set; }

        public INavigation Navigation;

        public ShareTradeViewModel(INavigation navigation)
        {
            Navigation = navigation;
            NewShareListViewObj = new ObservableCollection<NewShareListViewModel>();
            SecondaryShareListViewObj = new ObservableCollection<SecondaryShareListViewModel>();
            MyShareListViewObj = new ObservableCollection<MyShareListViewModel>();

            MyShareSellCommand = new Command<MyShareListViewModel>(handleMyShareSellCommand);
            NewShareBuyCommand = new Command<NewShareListViewModel>(handleNewShareBuyCommand);
            SecondaryShareBuyCommand = new Command<SecondaryShareListViewModel>(handleSecondaryShareBuyCommand);

            userShareTable = App.MobileService.GetTable<Share>();
            userCreditTable = App.MobileService.GetTable<UserCredit>();
            roofTable = App.MobileService.GetTable<Roof>();

            PopulateNewShares();
            GetUserShares();
            PopulateSecondaryMarket();
        }

        private async void GetUserShares()
        {
            try
            {
                if (UserShareList != null)
                    UserShareList.Clear();
                UserShareList = await userShareTable.Where
                    (Share => Share.UserId == App.UserID && Share.IsTradeable == false).ToCollectionAsync();
                if (UserShareList.Count != 0)
                {
                    // group by year
                    MyShareListViewModel tp = new MyShareListViewModel();
                    tp.ListQuantity = UserShareList.Count.ToString();
                    tp.SellCommand = MyShareSellCommand;
                    MyShareListViewObj.Add(tp);
                }
            }
            
            catch (Exception) { }
        }

        private async void PopulateSecondaryMarket()
        {
            try
            {
                if (SecondaryShareList != null)
                    SecondaryShareList.Clear();
                SecondaryShareList = await userShareTable.Where(Share => Share.IsTradeable == true).ToCollectionAsync();
                var distinctList = SecondaryShareList.ToLookup(x => x.Price);
                foreach(var share in distinctList)
                {
                    SecondaryShareListViewModel temp = new SecondaryShareListViewModel();
                    temp.ListRate = share.Key.ToString();
                    temp.ListQuantity = distinctList[share.Key].Count().ToString();
                    temp.BuyCommand = SecondaryShareBuyCommand;
                    SecondaryShareListViewObj.Add(temp);
                }
            }
            catch(Exception) { }
        }

        private async void PopulateNewShares()
        {
            try
            {
                if (RoofList != null)
                    RoofList.Clear();
                RoofList = await roofTable.Where(roof => roof.SharesAvailable > 0).ToCollectionAsync();
                if (RoofList.Count != 0)
                {
                    // group by year
                    foreach(Roof rf in RoofList)
                    {
                        NewShareListViewModel tp = new NewShareListViewModel();
                        tp.ListQuantity = rf.SharesAvailable.ToString();
                        tp.ListRate = rf.CostPerShare.ToString();
                        tp.BuyCommand = NewShareBuyCommand;
                        tp.RoofID = rf.RoofId;
                        NewShareListViewObj.Add(tp);
                    }                   
                }
            }

            catch (Exception) { }
        }
        private async void handleMyShareSellCommand(MyShareListViewModel item)
        {
            try
            {
                var price = int.Parse(item.PurchaseRate);
                var qty = int.Parse(item.QuantityPurchase);
                for (int i = 0; i < qty; i++)
                {
                    UserShareList[i].IsTradeable = true;
                    UserShareList[i].Price = price;
                    await App.MobileService.GetTable<Share>().UpdateAsync(UserShareList[i]);
                }
                await Navigation.PushModalAsync(new HamburgerMenu());
            }

            catch (Exception) { }            
        }


        private async void handleNewShareBuyCommand(NewShareListViewModel item)
        {
            try
            {
                int price = int.Parse(item.ListRate);
                int qty = int.Parse(item.QuantityPurchase);

                int cost = price * qty;

                var Roof = await roofTable.Where(roof => roof.RoofId == item.RoofID).ToCollectionAsync();
                if (Roof.Count != 0)
                {
                    Roof[0].SharesAvailable -= qty;
                    await roofTable.UpdateAsync(Roof[0]);
                }

                var Buyer = await userCreditTable.Where(x => x.UserId == App.UserID).ToCollectionAsync();
                Buyer[0].WalletBalance = (int.Parse(Buyer[0].WalletBalance) - cost).ToString();
                Buyer[0].Shares = Buyer[0].Shares + qty;
                await userCreditTable.UpdateAsync(Buyer[0]);

                var AdminUser = await userCreditTable.Where(x => x.UserId == App.AdminUserID).ToCollectionAsync();
                AdminUser[0].WalletBalance = (int.Parse(AdminUser[0].WalletBalance) + cost).ToString();
                await userCreditTable.UpdateAsync(AdminUser[0]);

                for(int i = 0; i < qty; i++)
                {
                    Share temp = new Share{ UserId = App.UserID,RoofId = item.RoofID ,IsTradeable = false, ShareId = 1,Price = cost };
                    await userShareTable.InsertAsync(temp);
                }
                await Navigation.PushModalAsync(new HamburgerMenu());
            }

            catch (Exception e)
            {
                var o = 1;
            }
        }


        private async void handleSecondaryShareBuyCommand(SecondaryShareListViewModel item)
        {
            try
            {
                int price = int.Parse(item.ListRate);
                int qty = int.Parse(item.QuantityPurchase);

                int cost = price * qty;
                List<Share> AvailableShares = SecondaryShareList.Where(x => x.Price == price).ToList();
                Dictionary<int, int> UserWalletUpdate = new Dictionary<int, int>();
                for (int i = 0; i < qty; i++)
                {
                    AvailableShares[i].IsTradeable = false;
                    if (UserWalletUpdate.ContainsKey(AvailableShares[i].UserId))
                        UserWalletUpdate[AvailableShares[i].UserId] = UserWalletUpdate[AvailableShares[i].UserId] + 1;
                    else
                        UserWalletUpdate.Add(AvailableShares[i].UserId, 1);
                    AvailableShares[i].UserId = App.UserID;
                    await userShareTable.UpdateAsync(AvailableShares[i]);
                }

                var Buyer = await userCreditTable.Where(x => x.UserId == App.UserID).ToCollectionAsync();
                Buyer[0].WalletBalance = (int.Parse(Buyer[0].WalletBalance) - cost).ToString();
                Buyer[0].Shares = Buyer[0].Shares + qty;

                await userCreditTable.UpdateAsync(Buyer[0]);

                foreach (var a in UserWalletUpdate)
                {
                    var Seller = await userCreditTable.Where(x => x.UserId == a.Key).ToCollectionAsync();
                    Seller[0].WalletBalance = (int.Parse(Seller[0].WalletBalance) + cost * a.Value).ToString();
                    Seller[0].Shares = Seller[0].Shares - a.Value;
                    await userCreditTable.UpdateAsync(Seller[0]);
                }
                await Navigation.PushModalAsync(new HamburgerMenu());
            }

            catch (Exception e)
            {

            }            
        }

    }
}
