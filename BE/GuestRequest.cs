using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    /// <summary>
    ///all client in our program can send a "GuestRequesr" to our System
    ///this request watched by the host
    /// </summary>
    public class GuestRequest
    {
        public string GuestRequestKey { get; set; }
        public string PrivateName { get; set; }
        public string FamilyName { get; set; }
        public string MailAddress { get; set; }
        public Enums.GuestReqStatus Status { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime ReleaseDate { get; set; }
        public Enums.Area area { get; set; }
        public Enums.HostUnitType Type { get; set; }
        public int Adults { get; set; }
        public int Children { get; set; }
        public bool Pool { get; set; }
        public bool ChildrensAttractions { get; set; }



        //===============================================================================================
        //ToString
        public override string ToString()
        {
            string RequestStr = "Key Number: " + GuestRequestKey.ToString()+"\n"+"Private Name: "+PrivateName +" Family Name: "+FamilyName+"\n"+"Mail: "+MailAddress+ "\n"+ "Entry Date: "
                + EntryDate.ToString() + " " + "Release Date" + ReleaseDate.ToString() + '\n';
            return RequestStr;
        }
   }
}
