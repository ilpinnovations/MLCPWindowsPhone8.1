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
    public partial class MySlotPage : PhoneApplicationPage
    {
        int SlotID { get; set; }
        string SlotName { get; set; }
        int FloorID { get; set; }
        string FloorName { get; set; }

        ProgressIndicator progressIndicator;
        public MySlotPage()
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
        private async void GetMySlotDetails()
        {
            try
            {
                ShowProgressIndicator(true);
                var floorObj = await (new MLCPClient()).GetBookingDetails();
                SlotID = int.Parse((string)floorObj.Values[0]["slotid"]);
                SlotName = (string)floorObj.Values[0]["slotname"];
                FloorName = (string)floorObj.Values[0]["floorname"];
                DateTime timeOfBook = DateTime.Parse((string)floorObj.Values[0]["book_timein"]);
                TimeSpan timeDiff = DateTime.Now - timeOfBook;

                double totalMins = Math.Round(timeDiff.TotalMinutes);
                double Hours = Math.Round(totalMins / 60);
                double Minutes = Math.Round(totalMins % 60);

                string inTime = timeOfBook.ToString("dd MMM yyyy AT hh:mm tt");
                string timeLapsedString = string.Format("{0} Hour(s), {1} Minute(s)", Hours, Minutes);

                //string msg = String.Format("SlotID: {0}\nLevel: {1}\nBookTime: {2}\nETA: {3}",
                //    SlotID, floorId, inTime, timeLapsedString);
                //MessageBox.Show(msg, "Info", MessageBoxButton.OK);

                //Update the UI...
                tbSlot.Text = SlotName.ToString();
                tbFloor.Text = FloorName.ToString();
                tbInTime.Text = inTime;
                tbHours.Text = timeLapsedString;
            }
            catch (Exception)
            {
                MessageBox.Show("No Slot alloted or data not available currently.", "Error", MessageBoxButton.OK);
                NavigationService.GoBack();
            }
            finally
            {
                ShowProgressIndicator(false);
            }
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            sliderSwipeOut.IsChecked = false;

            GetMySlotDetails();
            base.OnNavigatedTo(e);
        }

        private void sliderSwipeOut_Checked(object sender, RoutedEventArgs e)
        {
            // perform swipe out
            ClearSlot();
        }
        private async void ClearSlot()
        {
            try
            {
                if (Confirmed() == false)
                {
                    sliderSwipeOut.IsChecked = false;
                    return;
                }

                ShowProgressIndicator(true);
                var clearSlotResponse = await (new MLCPClient()).ClearSlot(SlotID);
                MessageBox.Show("Slot has been deallocated.", "Confirmed", MessageBoxButton.OK);
                NavigationService.GoBack();
            }
            catch(Exception)
            {

            }
            finally
            {
                ShowProgressIndicator(false);
            } 
        }

        private bool Confirmed()
        {
            return MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButton.OKCancel) == MessageBoxResult.OK;
        }
    }
}