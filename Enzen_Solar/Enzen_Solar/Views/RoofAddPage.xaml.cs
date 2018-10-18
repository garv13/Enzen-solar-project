using Enzen_Solar.ViewModels;
using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace Enzen_Solar.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RoofAddPage : ContentPage
	{
        AddRoofViewModel ViewModel;

        public RoofAddPage ()
		{
			InitializeComponent ();
            ViewModel = new AddRoofViewModel();
            MainGrid.BindingContext = ViewModel;
            Appearing += RoofAddPage_Appearing;
        }

        private async void RoofAddPage_Appearing(object sender, EventArgs e)
        {
            await GetPresentLocatation();
        }
        private async Task GetPresentLocatation()
        {
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 100;
                var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(20), null, true);
                MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude),
                                                             Distance.FromMiles(1)));
                
                ViewModel.Latitude = position.Latitude.ToString();
                ViewModel.Longitude = position.Longitude.ToString();                
            }

            catch(Exception e)
            {

            }
        }
    }
}