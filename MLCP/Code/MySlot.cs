using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MLCP.Code
{
    class MySlot
    {
        public bool Result { get; set; }
        public int FloorID { get; set; }
        public string FloorName { get; set; }
        public int SlotID { get; set; }
        public string SlotName { get; set; }
        public MySlot()
        {
            Result = false;
            FloorID = SlotID = 0;
            FloorName = SlotName = "NONE";
        }
        public Task Load()
        {
            HttpClient client = new HttpClient();
            var result = client.GetAsync(Vars.BASE_URL + "myslot_json.php");
            var str = result.Result.Content.ReadAsStringAsync();
            return null;
        }
    }
}
