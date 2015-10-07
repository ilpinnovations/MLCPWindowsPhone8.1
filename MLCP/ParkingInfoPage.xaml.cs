using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;
using MLCP.Code;

namespace MLCP
{
    public partial class ParkingInfoPage : PhoneApplicationPage
    {
        int nAvailableSlots = 0;
        int nTotalSlots = 1;

        ProgressIndicator progressIndicator;
        public int AvailableSlots
        {
            get { return nAvailableSlots; }
            set
            {
                if (value >= 0 && value <= TotalSlots)
                {
                    nAvailableSlots = value; 
                    SetAvailableSlots(value);
                }
            }
        }
        public int TotalSlots { get { return nTotalSlots; } set { nTotalSlots = value; SetTotalSlots(value); } }
        public ParkingInfoPage()
        {
            InitializeComponent();

            progressIndicator = new ProgressIndicator();
            progressIndicator.IsIndeterminate = true;
            SystemTray.SetProgressIndicator(this, progressIndicator);
        }
        private void SetAvailableSlots(int value)
        {
            tbAvailable.Text = value.ToString();

            //Caculate color
            double x = ((double)value / (double)nTotalSlots);
            byte rComp = (byte)(255*(1-x));
            byte gComp = (byte)(255 - rComp);
            SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(255, rComp, gComp, 0));
            tbAvailable.Foreground = brush;
        }
        private void SetTotalSlots(int value)
        {
            tbTotal.Text = value.ToString();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            AvailableSlots = 0; TotalSlots = 0;
            GetParkingDetails();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AvailableSlots += 10;
        }

        private async void GetParkingDetails()
        {
            SystemTray.GetProgressIndicator(this).IsVisible = true;
            try
            {
                var floorObj = await (new MLCPClient()).GetAvailableSlots();
                TotalSlots = (int)floorObj.Values[0]["total"];
                AvailableSlots = (int)floorObj.Values[0]["available"];
           }
            catch (Exception)
            {
                MessageBox.Show("No Parking Information available currently.", "Error", MessageBoxButton.OK);
                NavigationService.GoBack();
            }
            finally
            {
                SystemTray.GetProgressIndicator(this).IsVisible = false;
            }
        }

    }
}