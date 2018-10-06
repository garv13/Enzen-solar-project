using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Enzen_Solar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HamburgerMenu : MasterDetailPage
    {
        public HamburgerMenu()
        {
            InitializeComponent();
            MyMenu();
        }
        public void MyMenu()
        {
            Detail = new NavigationPage(new MainPage());
            List<Menu> menu = new List<Menu>
            {
                new Menu{ Page= new MainPage(),MenuTitle="Dashboard",icon="user.png"},
                new Menu{ Page= new ShareTradePage(),MenuTitle="Share Trading", icon="message.png"},
                new Menu{ Page= new CoinTradePage(),MenuTitle="Coin Trading",icon="contacts.png"},
                new Menu{ Page= new RoofAddPage(),MenuTitle="Add Roof", icon="settings.png"},
            };
            ListMenu.ItemsSource = menu;
        }
        private void ListMenu_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var menu = e.SelectedItem as Menu;
            if (menu != null)
            {
                IsPresented = false;
                Detail = new NavigationPage(menu.Page);
            }
        }

        public class Menu
        {
            public string MenuTitle
            {
                get;
                set;
            }
            public string MenuDetail
            {
                get;
                set;
            }

            public ImageSource icon
            {
                get;
                set;
            }

            public Page Page
            {
                get;
                set;
            }

        }
    }
}