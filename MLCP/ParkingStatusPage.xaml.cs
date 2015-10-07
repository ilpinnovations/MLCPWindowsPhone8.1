using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MLCP.Code;

namespace MLCP
{
    public partial class ParkingStatusPage : PhoneApplicationPage
    {
        ProgressIndicator progressIndicator;
        public ParkingStatusPage()
        {
            InitializeComponent();
           
            progressIndicator = new ProgressIndicator();
            progressIndicator.IsIndeterminate = true;
            SystemTray.SetProgressIndicator(this, progressIndicator);
        }
        private void ShowProgressIndicator(bool show)
        {
            SystemTray.GetProgressIndicator(this).IsVisible = show;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            PerformParkingStatusMaster();
        }
        public async void PerformParkingStatusMaster()
        {
            try
            {
                ShowProgressIndicator(true);
                var parkObj = await (new MLCPClient()).GetParkingStatus();
                string yesterdayString = (string)parkObj.Values[0]["yesterday"];
                string todayString = (string)parkObj.Values[0]["today"];

                //Update the UI...
                tbYesterday.Text = yesterdayString;
                tbToday.Text = todayString;
                
            }
            catch (Exception)
            {
                MessageBox.Show("Cannot fetch the data. Please try again later.", "Error", MessageBoxButton.OK);
                NavigationService.GoBack();
            }
            finally
            {
                ShowProgressIndicator(false);
            }
        }
    }
}