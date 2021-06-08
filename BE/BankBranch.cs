using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    /// <summary>
    ///every Host has a "BankBranch" object,
    ///The system will charge the host a fee according to bank details
    /// </summary>
    public class BankBranch
    {
        public string BankNumber { get; set; }
        public string BankName { get; set; }
        public string BranchNumber { get; set; }
        public string BranchAddress { get; set; }
        public string BranchCity { get; set; }

        //=================================================================================================
        //ToString
        public override string ToString()
        {
            string AcountStr = "Bank Number: " + BankNumber.ToString() + " " +
                "Bank Name: " + BankName + "\n" + "Branch Number: " + BranchNumber+" Branch Address: "+BranchAddress+" Branch City: "+BranchCity+"\n" ;
            return AcountStr;
        }
    }
}
