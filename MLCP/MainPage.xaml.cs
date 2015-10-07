using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MLCP.Resources;
using MLCP.Code;
using System.Net.Http;

namespace MLCP
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (Commons.IsFirstTime)
            {
                NavigationService.Navigate(new Uri("/RegisterPage.xaml", UriKind.RelativeOrAbsolute));
                return;
            }

            List<MainMenuItemModel> iList = new List<MainMenuItemModel>()
            {
                new MainMenuItemModel(){ ImageSource="/Assets/myslot_img.png", SubtitleString="Get Information on currently alloted slot", TitleString="MY SLOT"},
                new MainMenuItemModel(){ ImageSource="/Assets/parkinfo_img.png", SubtitleString="Get some quick info on parking slots", TitleString="MLCP Parking Info"},
                new MainMenuItemModel(){ ImageSource="/Assets/parkstatus_img.png", SubtitleString="Quick analysis of MLCP", TitleString="Parking Status"}
            };


            // image configuration in metro studio : 128px size and 30 px padding

            btnSlot.DataContext = iList[0];
            btnParkInfo.DataContext = iList[1];
            btnStatus.DataContext = iList[2];
        }
        private async void callsample()
        {

        }
        private async void myslotsample()
        {

        }

        private void btnSlot_Click(object sender, RoutedEventArgs e)
        {
            myslotsample();

            NavigationService.Navigate(new Uri("/MySlotPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void btnParkInfo_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/ParkingInfoPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void btnStatus_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/ParkingStatusPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void AppBar_Register_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/RegisterPage.xaml", UriKind.RelativeOrAbsolute));
        }
        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}