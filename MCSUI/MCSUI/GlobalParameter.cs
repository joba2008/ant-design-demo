using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCSUI
{
    public class GlobalParameter
    {
        public static Dictionary<string, recordInfoBaseOnLocation> LocationFoupIDMapping = new Dictionary<string, recordInfoBaseOnLocation>();
        public static bool ProgressBarFlag { get; set; }
        public struct recordInfoBaseOnLocation
        {
            public string location_id;
            public string foup_id;
            public string location_available;
            public string port_status;
        };
    }
}