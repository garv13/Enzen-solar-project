using Enzen_Solar.Models;
using Enzen_Solar.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Enzen_Solar.ViewModels
{
    public class AddRoofViewModel : BindableObject , INotifyPropertyChanged
    {
        private string _coinPotential;
        private string _roofSize;
        private string _totalShare;
        private string _investNeed;
        private string _sharePer;
        private string _costPerShare;
        private string _latitude;
        private string _longitude;

        public ICommand PublishCommand { get; private set; }

        new public event PropertyChangedEventHandler PropertyChanged;

        INavigation Navigation;

        public AddRoofViewModel(INavigation navigation)
        {
            Navigation = navigation;
            _coinPotential = "15";
            _totalShare = "0";
            PublishCommand = new Command(publishCommandHandler);
        }

        private async void publishCommandHandler()
        {
            try
            {
                Roof obj = new Roof();
                obj.Size = int.Parse(_roofSize);
                obj.Percentage = int.Parse(_sharePer);
                obj.Potential = double.Parse(_coinPotential);
                obj.TotalShares = int.Parse(_totalShare);
                obj.Investment = int.Parse(_investNeed);
                obj.CostPerShare = double.Parse(_costPerShare);
                obj.latitude = double.Parse("37.4220");
                obj.longitude = double.Parse("-122.0840");
                obj.UserId = App.UserID;
                obj.RoofId = 2;
                obj.SharesAvailable = int.Parse(_totalShare) - int.Parse(_sharePer);
                await App.MobileService.GetTable<Roof>().InsertAsync(obj);
                Navigation.PushModalAsync(new HamburgerMenu());
            }
            catch(Exception e)
            {

            }
        }

        protected override void OnPropertyChanged(string propertyName)
        {          
            if(propertyName == "RoofSize")
            {
                _totalShare = (int.Parse(_roofSize) * int.Parse(_coinPotential)).ToString();
                _investNeed = (int.Parse(_roofSize) * 160).ToString();
                OnPropertyChanged("TotalShare");
                OnPropertyChanged("InvestNeed");
            }

            if (propertyName == "SharePer")
            {
                double costpersharev = double.Parse(_investNeed) / (double.Parse(_totalShare) - double.Parse(_sharePer));
                _costPerShare = Math.Round(costpersharev,2).ToString();
                OnPropertyChanged("CostPerShare");
            }
            var propertyChangedCallback = PropertyChanged;
            propertyChangedCallback?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }   

        public string CoinPotential
        {
            get
            {
                return _coinPotential;
            }
            set
            {
                _coinPotential = value;
                OnPropertyChanged("CoinPotential");
            }
        }
        public string RoofSize
        {
            get
            {
                return _roofSize;
            }
            set
            {
                _roofSize = value;
                OnPropertyChanged("RoofSize");
            }
        }
        public string TotalShare
        {
            get
            {
                return _totalShare;
            }
            set
            {
                _totalShare = value;
                OnPropertyChanged("TotalShare");
            }
        }
        public string InvestNeed
        {
            get
            {
                return _investNeed;
            }
            set
            {
                _investNeed = value;
                OnPropertyChanged("InvestNeed");
            }
        }
        public string SharePer
        {
            get
            {
                return _sharePer;
            }
            set
            {
                _sharePer = value;
                OnPropertyChanged("SharePer");
            }
        }
        public string CostPerShare
        {
            get
            {
                return _costPerShare;
            }
            set
            {
                _costPerShare = value;
                OnPropertyChanged("CostPerShare");
            }
        }
        public string Latitude
        {
            get
            {
                return _latitude;
            }
            set
            {
                _latitude = value;
                OnPropertyChanged("Latitude");
            }
        }
        public string Longitude
        {
            get
            {
                return _longitude;
            }
            set
            {
                _longitude = value;
                OnPropertyChanged("Longitude");
            }
        }

    }
}
