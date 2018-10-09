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

        public ICommand PublishCommand { get; private set; }


        public AddRoofViewModel()
        {
            CoinPotential = "1";
            RoofSize = "2";
            TotalShare = "3";
            InvestNeed = "4";
            SharePer = "5";
            CostPerShare = "6";

            PublishCommand = new Command(publishCommandHandler);
        }

        private async void publishCommandHandler()
        {
            var a = CoinPotential;
            var b = RoofSize;
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

    }
}
