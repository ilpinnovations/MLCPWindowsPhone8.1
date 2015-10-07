using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.IO.IsolatedStorage;

namespace MLCP.Code
{
    class MLCPClient
    {
        //const string BASE_URL = "http://192.168.1.100:8081/mlcp";
        const string BASE_URL = "http://theinspirer.in/mlcpapp";
        const string URL_GET_FLOORS = "?tag=GetFloors";
        const string URL_BOOKING_DETAILS = "?tag=GetBookingDetailsById&employeeId={0}";
        const string URL_AVAIL_SLOTS = "?tag=GetAvailableSlots";
        const string URL_CLEAR_SLOT = "?tag=ClearSlot&slotId={0}";
        const string URL_VALID_USER = "?tag=GetIsValidUser&employeeId={0}&name={1}";
        const string URL_PARKING_STATUS = "?tag=GetParkingStatus";

        Random r = new Random();
        HttpClient client;

        public MLCPClient()
        {
            client = new HttpClient();
        }
        public async Task<string> GetURLContent(string url)
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine("<D351r3> HTTP: " + url);
#endif
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        public async Task<JObject> GetJSONContent(string url)
        {
            //return Task.Run(async () =>
            //{
            //    var strContent = await GetURLContent(url);
            //    return JObject.Parse(strContent);
            //});
            var strContent = await GetURLContent(url);
            return JObject.Parse(strContent);
        }
        private string GetAbsoluteURL (string relativeURL, string[] param)
        {
            string theURL = string.Format(BASE_URL + relativeURL, param);
            theURL += "&_=" + r.Next(1000, 9999);
            return theURL;
        }
        private async Task<MLCPResponseMessage> GetMLCPResponse(string url)
        {
            var json = await GetJSONContent(url);
            return MLCPResponseMessage.FromJSON(json);
        }
        public async Task<MLCPResponseMessage> GetFloors()
        {
            return await GetMLCPResponse(GetAbsoluteURL(URL_GET_FLOORS, new string[] { }));
        }
        public async Task<MLCPResponseMessage> GetBookingDetails()
        {
            int EmployeeId = (int)IsolatedStorageSettings.ApplicationSettings[MLCP.RegisterPage.KEYS.EMPID.ToString()];
            return await GetMLCPResponse(GetAbsoluteURL(URL_BOOKING_DETAILS, new string[] { EmployeeId.ToString() }));
        }
        public async Task<MLCPResponseMessage> GetAvailableSlots()
        {
            return await GetMLCPResponse(GetAbsoluteURL(URL_AVAIL_SLOTS, new string[] { }));
        }
        public async Task<MLCPResponseMessage> ClearSlot(int slotId)
        {
            string s = GetAbsoluteURL(URL_CLEAR_SLOT, new string[] { slotId.ToString() });
            return await GetMLCPResponse(GetAbsoluteURL(URL_CLEAR_SLOT, new string[] { slotId.ToString() }));
        }
        public async Task<MLCPResponseMessage> GetIsValidUser(int empId, string name)
        {
            string s = GetAbsoluteURL(URL_VALID_USER, new string[] { empId.ToString(), name });
            return await GetMLCPResponse(s);
        }
        public async Task<MLCPResponseMessage> GetParkingStatus()
        {
            string s = GetAbsoluteURL(URL_PARKING_STATUS, new string[] { });
            return await GetMLCPResponse(s);
        }
    }
}
