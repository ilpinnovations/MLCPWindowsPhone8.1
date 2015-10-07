using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLCP.Code
{
    class MLCPResponseMessage
    {
        string _tag;
        bool _error;
        string _errorMsg;
        JArray _values;

        public string Tag { get { return _tag; } set { _tag = value; } }
        public bool Error { get { return _error; } set { _error = value; } }
        public string ErrorMsg { get { return _errorMsg; } set { _errorMsg = value; } }
        public JArray Values { get { return _values; } set { _values = value; } }

        public MLCPResponseMessage()
        {
            Tag = ErrorMsg = "";
            Values = null;
            Error = false;
        }
        public static MLCPResponseMessage FromJSON(JObject json)
        {
            return new MLCPResponseMessage()
            {
                Tag = (string)json["tag"],
                Error = (bool)json["error"],
                ErrorMsg = (string)json["errorMsg"],
                Values = (JArray)json["values"]
            };
        }
    }
}
