using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    /// <summary>
    ///here we save all the Enums of this Solution
    /// </summary>
    public class Enums
    {
        public enum HostUnitType {מלון,צימר,דירת_אירוח,מאהל}
        public enum Area { צפון,דרום,מרכז,ירושלים_והסביבה}
        public enum OrderStatus { נסגר_מחוסר_הענות_לקוח,נסגר_בהענות_לקוח,טרם_טופל,נשלח_מייל}
        public enum GuestReqStatus { פתוחה,נסגרה_דרך_האתר,סגורה}
    }
}
