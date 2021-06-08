using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    /// <summary>
    ///"Host" object is the side that using our System to find coustemers
    ///to his Hosting Units
    /// </summary>
    public class Host
    {
        public string HostKey { get;set; }
        public string PrivateName { get; set; }
        public string FamilyName { get; set; }
        public string  PhoneNumber { get; set; }
        public string MailAddress { get; set; }
        public string BankAccountNumber { get; set; }
        public BankBranch HostBankAccuont { get; set; }
        public bool CollectionClearance { get; set; }
        //============================================================================================
        //ToString
        public override string ToString()
        {
            string HostStr = "Host Key: " + HostKey.ToString() + '\n'+"Private Name: "+PrivateName+" Family Name: "+FamilyName+'\n' + "Mail: " + MailAddress +
                '\n' + "phone Number: " + PhoneNumber + '\n'+ "BankAccountNumber: "+BankAccountNumber+"\n";
            return HostStr;
        }
    }
}
