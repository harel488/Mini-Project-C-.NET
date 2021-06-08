using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    /// <summary>
    ///here we save the static data,
    ///this class save the running Keys of units and request
    ///and also system details
    /// </summary>
    public class Configuration
    {
        public static int HostingUnitKey { get; set; } = 10000001;
        public static int GuestRequestKey { get; set; } = 10000001;
        public static int OrderKey { get; set; } = 10000001;
        public static int commission { get; set; } = 10;
        public static int Profits { get; set; } = 0;
        public static string AdminPassword { get; set; } = "1234";
        public static int RequestExpired { get; set; } = 60;
        public static int OrderExpired { get; set; } = 60;
        public static int NumClosedOrders { get; set; } = 0;
        public static int NumOfAllReqs { get; set; } = 0;
        public static int NumOfUnits { get; set; } = 0;
       
    }
}
