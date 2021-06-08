using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    /// <summary>
    /// Invitation object that a host offers to the customer
    /// the host watching the Guest Request and if there is a match
    /// he can send an "Order" to the client
    /// </summary>
    public class Order
    {
        public string HostingUnitKey { get; set; }
        public string GuestRequestKey { get; set; }
        public string OrderKey { get; set; }
        public Enums.OrderStatus Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime OrderDate { get; set; }

        //================================================================================================
        public override string ToString()
        {
            string OrderStr = "Order Key: " + OrderKey + '\n' +
                "Status: " + Status.ToString()+"Create Date: "+CreateDate.ToString()+" Order Date: "+OrderDate.ToString()+"\n";
            return OrderStr;
        }

        
    }
}
