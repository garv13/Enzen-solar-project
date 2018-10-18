using Enzen_Solar.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Enzen_Solar.ViewModels
{
    public class AddRoofViewModel : BindableObject 
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


        public AddRoofViewModel()
        {
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
                obj.CostPerShare = int.Parse(_costPerShare);
                obj.latitude = double.Parse(_latitude);
                obj.longitude = double.Parse(_longitude);
                obj.UserId = App.UserID;
                obj.RoofId = 212121;
                obj.SharesAvailable = int.Parse(_totalShare) - int.Parse(_sharePer);
                await App.MobileService.GetTable<Roof>().InsertAsync(obj);
            }
            catch(Exception e)
            {

            }
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
                OnPropertyChanged(CoinPotential);
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
                OnPropertyChanged(RoofSize);
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
                OnPropertyChanged(TotalShare);
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
                OnPropertyChanged(InvestNeed);
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
                OnPropertyChanged(SharePer);
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
                OnPropertyChanged(CostPerShare);
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
                OnPropertyChanged(Latitude);
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
                OnPropertyChanged(Longitude);
            }
        }

    }
}
