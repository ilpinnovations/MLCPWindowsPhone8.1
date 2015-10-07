using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;
using MLCP.Code;
using System.Text.RegularExpressions;
using System.Net.Http;

namespace MLCP
{
    public partial class RegisterPage : PhoneApplicationPage
    {
        ProgressIndicator progressIndicator;
        public enum KEYS { EMPID, FULLNAME, LOCATION, EMAILID }

        int _empId;
        string _fullName;
        int _locationIndex;
        string _emailId;

        public RegisterPage()
        {
            InitializeComponent();
            progressIndicator = new ProgressIndicator();
            progressIndicator.IsIndeterminate = true;
            SystemTray.SetProgressIndicator(this, progressIndicator);
        }
        public void LoadFromStore()
        {
            var settings = IsolatedStorageSettings.ApplicationSettings;

            if (settings.TryGetValue<int>(KEYS.EMPID.ToString(), out _empId))
                tbEmployeeId.Text = _empId.ToString();
            if (settings.TryGetValue<string>(KEYS.FULLNAME.ToString(), out _fullName))
                tbFullName.Text = _fullName.ToString();
            if (settings.TryGetValue<int>(KEYS.LOCATION.ToString(), out _locationIndex))
                cbLocation.SelectedIndex = _locationIndex;
            if (settings.TryGetValue<string>(KEYS.EMAILID.ToString(), out _emailId))
                tbEmail.Text = _emailId.ToString();
        }
        public void SaveToStore()
        {
            _emailId = tbEmail.Text;
            _empId = int.Parse(tbEmployeeId.Text);
            _fullName = tbFullName.Text;
            _locationIndex = cbLocation.SelectedIndex;

            IsolatedStorageSettings.ApplicationSettings[KEYS.EMAILID.ToString()]  = _emailId;
            IsolatedStorageSettings.ApplicationSettings[KEYS.EMPID.ToString()]    = _empId;
            IsolatedStorageSettings.ApplicationSettings[KEYS.FULLNAME.ToString()] = _fullName;
            IsolatedStorageSettings.ApplicationSettings[KEYS.LOCATION.ToString()] = _locationIndex;

            Commons.IsFirstTime = false;
        }
        public bool IsValid()
        {
            _emailId = tbEmail.Text;
            bool emailValid = Regex.IsMatch(_emailId, ".+@tcs.com");
            _fullName = tbFullName.Text;
            bool fullNameValid = !string.IsNullOrEmpty(_fullName);
            _locationIndex = cbLocation.SelectedIndex;

            if (!int.TryParse(tbEmployeeId.Text, out _empId))
            {
                MessageBox.Show("Please enter a valid Employee ID", "Error", MessageBoxButton.OK);
                return false;
            } 
            if (!fullNameValid)
            {
                MessageBox.Show("Please enter a valid Name", "Error", MessageBoxButton.OK);
                return false;
            }
            if (_locationIndex == -1)
            {
                MessageBox.Show("Please select a Location", "Error", MessageBoxButton.OK);
                return false;
            }
            if (!emailValid)
            {
                MessageBox.Show("Please enter a valid TCS Email id", "Error", MessageBoxButton.OK);
                return false;
            }

            return true;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            LoadFromStore();
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            //if (IsValid())
            //{
            //    SaveToStore();
            //}
            //else
            //{
            //    MessageBox.Show("Please enter valid values in the fields.", "Error", MessageBoxButton.OK);
            //    e.Cancel = true;
            //}
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PerformRegisterMaster();
            }
            catch (Exception)
            {
                MessageBox.Show("Cannot find the server. Please make sure the internet is working.", "Error", MessageBoxButton.OK);

                //throw;
            }
        }
        private void ShowProgressIndicator(bool show)
        {
            SystemTray.GetProgressIndicator(this).IsVisible = show;
        }

        private async void PerformRegisterMaster()
        {
            try
            {
                if (IsValid())
                {
                    ShowProgressIndicator(true);
                    var resp = await (new MLCPClient()).GetIsValidUser(_empId, _fullName);

                    if (resp.Error)
                    {
                        MessageBox.Show("The specified userID is not authorised to use the application. Please contact the admin.", "Error", MessageBoxButton.OK);
                        ShowProgressIndicator(false);
                        return;
                    }

                    MessageBox.Show("The specified userID is allowed to proceed.", "Authorisation successful", MessageBoxButton.OK);

                    ShowProgressIndicator(false);
                    SaveToStore();
                    if (NavigationService.CanGoBack)
                        NavigationService.GoBack();
                    else
                        Application.Current.Terminate();
                }

            }
            catch (HttpRequestException)
            {
                MessageBox.Show("Make sure the device has an active internet connection and try again");
            }
            catch(Exception ee)
            {
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK);
            }
        }
    }
}