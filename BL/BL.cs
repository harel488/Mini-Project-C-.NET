using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using BE;
using DAL;

namespace BL
{

    public class BL : IBL
    {

//================================================================================
//Factory method
        static BL instance = null;

        public static BL GetInstance()
        {
            if (instance == null)
                instance = new BL();
            return instance;
        }


//=================================================================================

        static Idal dl = DAL.Factory.get_imp_Dal("xml");//to send to DAL and save in DataSource


//Exist objects:
    public static bool HostExist(string key)
        {
            foreach (var it in dl.getAllUnits())
            {
                if (it.Owner.HostKey == key)
                    return true;
            }
                return false;
        }

    public static bool BankBranchExist(string numBank,string numBranch)
        {
            if (dl.getAllBankBranchs().FirstOrDefault(it => it.BankNumber == numBank && it.BranchNumber == numBranch) != null)
                return true;
            return false;
        }

        public static bool RequestExiest(string key)
        {
            foreach (var it in dl.getAllGuestReqs())
            {
                if (it.GuestRequestKey == key)
                    return true;
            }
            return false;
        }




//CRUD GuestRequest
//============================================================================================
        void IBL.addGuestReq(GuestRequest newRequest)
        {
            
            if (newRequest.EntryDate >= newRequest.ReleaseDate || newRequest.EntryDate < DateTime.Today || newRequest.ReleaseDate < DateTime.Today)
                throw new ArgumentException("התאריכים שנבחרו אינם תקינים");
            newRequest.RegistrationDate = DateTime.Today;
            dl.AddGuestReq(newRequest);
        }

        void IBL.updateGuestReq(GuestRequest ReqToUpdate)
        {
            
            if (ReqToUpdate.EntryDate >= ReqToUpdate.ReleaseDate||ReqToUpdate.EntryDate<DateTime.Today||ReqToUpdate.ReleaseDate<DateTime.Today)
                throw new ArgumentException("התאריכים שנבחרו אינם תקינים");
            
            dl.updateGuestReq(ReqToUpdate);
        }

        void IBL.DeleteGuestReq(GuestRequest ReqToDel)
        {
            foreach(var it in dl.getAllOrders())
            {
                if (it.GuestRequestKey == ReqToDel.GuestRequestKey)
                    dl.DeleteOrder(it);
            }
            dl.DeleteGuestReq(ReqToDel);
        }

        GuestRequest IBL.getGuestReq(GuestRequest req)
        {
            return dl.getGuestReq(req);
        }

        //CRUD HostingUnit
        //=============================================================================

        void IBL.addHostUnit(HostingUnit newUnit)
        {
            dl.addHostUnit(newUnit);
        }

        void IBL.updateUnit(HostingUnit UpdUnit)
        {
            HostingUnit prevUnit = dl.GetHostingUnit(UpdUnit);
            if (prevUnit.Owner.CollectionClearance == true && UpdUnit.Owner.CollectionClearance == false)
            {
                foreach (var it in dl.getAllUnits())
                {
                    if (it.HostingUnitKey == UpdUnit.HostingUnitKey)
                    {
                        UpdUnit.Owner.CollectionClearance = true;
                        throw new ArgumentException("לא ניתן לבטל הרשאת חשבון כל עוד יש הזמנה פתוחה");
                    }
                }
            }
            dl.updateUnit(UpdUnit);
        }

        void IBL.deleteUnit(HostingUnit unitToDel)
        {
            List<Order> lst = dl.getAllOrders();
            foreach (var it in lst)
            {
                if (it.HostingUnitKey == unitToDel.HostingUnitKey)
                    throw new ArgumentException("לא ניתן למחוק את יחידת האירוח כיוון שיש הצעות פתוחות לגביה ");
            }
            if (getAllHostUnits(unitToDel.Owner.HostKey).Count == 1)
                throw new ArgumentException("אינך יכול למחוק יחידה זו משום שאין עוד יחידות למארח זה");
            dl.deleteUnit(unitToDel);
            
        }

        HostingUnit IBL.GetHostingUnit(HostingUnit Unit)
        {
            return dl.GetHostingUnit(Unit);
        }

        //CRUD Order
        //======================================================================================
        void IBL.addOrder(Order newOrder)
        {
            HostingUnit unit = getUnitByKey(newOrder.HostingUnitKey);
            GuestRequest req = getReqByKey(newOrder.GuestRequestKey);
            DateTime i = req.EntryDate;
            while (i != req.ReleaseDate)
            {
                if (unit.Diary[i.Month - 1, i.Day - 1])
                    throw new ArgumentException("התאריכים בהזמנה תפוסים");
                i = i.AddDays(1);
            }
            dl.addOrder(newOrder);
        }

        void IBL.updateOrder(Order orderToUpd)
        {
            Order prevOrder = getOrderByKey(orderToUpd.OrderKey);
            Host _host = getUnitByKey(orderToUpd.HostingUnitKey).Owner;
            bool mailSend = false;
            if (orderToUpd.Status == Enums.OrderStatus.נשלח_מייל||orderToUpd.Status==Enums.OrderStatus.נסגר_בהענות_לקוח)
            {
                if (_host.CollectionClearance==false)
                    throw new ArgumentException("אינך חתמת על הרשאת חיוב חשבון אנא הסדר זאת על מנת לשלוח מייל ללקוח");
               orderToUpd.OrderDate = new DateTime();
               orderToUpd.OrderDate = DateTime.Today;
                
                mailSend = true;
            }
            if (ClosedTransaction(prevOrder) && !ClosedTransaction(orderToUpd))
                throw new ArgumentException("אינך יכול לשנות את סטטוס ההזמנה מכיוון שהיא כבר סגורה");
            if (ClosedTransaction(orderToUpd))
            {
                //חישוב העמלה 
                int thisCommission = (BE.Configuration.commission) * (getNumOfHolidayDays(getReqByKey(orderToUpd.GuestRequestKey)));
                Configuration.Profits += thisCommission;
                
                
                //

                HostingUnit unit = getUnitByKey(orderToUpd.HostingUnitKey);
                GuestRequest req = getReqByKey(orderToUpd.GuestRequestKey);
                DateTime i = req.EntryDate;
                while (i != req.ReleaseDate)
                {
                    unit.Diary[i.Month - 1, i.Day - 1] = true;
                    i = i.AddDays(1);
                }
                dl.updateUnit(unit);
                req.Status = Enums.GuestReqStatus.נסגרה_דרך_האתר;
                dl.updateGuestReq(req);
               
            }
            if (mailSend)
            {
               
                
                MailMessage mail = new MailMessage();                mail.To.Add(getReqByKey(orderToUpd.GuestRequestKey).MailAddress);                mail.From = new MailAddress("harel488@gmail.com");
                mail.Subject = "הזמנה לחופשה: ";                mail.Body = "פרטי יחידת האירוח"+"\n"+"שם היחידה "+getUnitByKey(orderToUpd.HostingUnitKey).HostingUnitName+"\n"+                   "מחיר ללילה "+ getUnitByKey(orderToUpd.HostingUnitKey).Price+"\n"+                  "איזור "+ getUnitByKey(orderToUpd.HostingUnitKey).Area.ToString()+"\n"+                  "טלפון "+ getUnitByKey(orderToUpd.HostingUnitKey).Owner.PhoneNumber+"\n"+                  "מייל לאישור "+ getUnitByKey(orderToUpd.HostingUnitKey).Owner.MailAddress+"\n";                SmtpClient smtp = new SmtpClient("smtp.gmail.com",587);
                smtp.EnableSsl = true;
               
                smtp.Credentials = new System.Net.NetworkCredential("harel488@gmail.com","026789135");                                try
                {
                    smtp.Send(mail);                   
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }

            dl.updateOrder(orderToUpd);
        }

        void IBL.DeleteOrder(Order orderToDel)
        {
            dl.DeleteOrder(orderToDel);
        }

        Order IBL.getOrder(Order order)
        {
            
            return dl.getOrder(order);
        }

       
        
       

        //return list of object functions
        //================================================================================

        List<HostingUnit> IBL.getAllUnits()
        {
            return dl.getAllUnits();
        }
        //---------------------------------------
        List<GuestRequest> IBL.getAllGuestReqs()
        {
            return dl.getAllGuestReqs();
        }
        //---------------------------------------
        List<Order> IBL.getAllOrders()
        {
            return dl.getAllOrders();
        }
        //---------------------------------------
        List<Host> IBL.getAllHosts()
        {
            return dl.getAllHosts();
        }
        List<BankBranch> IBL.getAllBankBranchs()
        {
            return dl.getAllBankBranchs();
        }
        //================================================================================



        //Other function
        //================================================================================

        /// <summary>
        /// send HostingUnitKey To refer to the specific unit 
        /// </summary>
        /// <param name="key">the key of the Hosting Unit </param>
        /// <returns> a Copy of the requested unit  </returns>
        public static HostingUnit getUnitByKey(string key)
        {
            HostingUnit unit = dl.getAllUnits().FirstOrDefault(it => it.HostingUnitKey == key);
            return unit;
        }

        /// <summary>
        /// send GuestRequesKey To refer to the specific Guest Request
        /// </summary>
        /// <param name="key">the key of the guest</param>
        /// <returns> a Copy of the requested GuestRequest  </returns>
        public static GuestRequest getReqByKey(string key)
        {
            GuestRequest req = dl.getAllGuestReqs().FirstOrDefault(it => it.GuestRequestKey == key);
            return req;
        }

        /// <summary>
        /// send HostKey To refer to the specific Host 
        /// </summary>
        /// <param name="key">the key of the Host</param>
        /// <returns> a Copy of the requested Host  </returns>
        public static Host getHostByKey(string key)
        {
            Host host = dl.getAllUnits().FirstOrDefault(it => it.Owner.HostKey == key).Owner;
            return host;
        }

        /// <summary>
        /// send Order To refer to the specific Order
        /// </summary>
        /// <param name="key">the key of the Order</param>
        /// <returns> a Copy of the requested Order  </returns>
        public static Order getOrderByKey(string key)
        {
            Order ord = dl.getAllOrders().FirstOrDefault(it => it.OrderKey == key);
            return ord;
        }

        /// <summary>
        /// Checks if the order is closed to send an email to the customer
        /// </summary>
        /// <param name="ord">the Order to check</param>
        /// <returns> a boolean value  </returns>
        public static bool ClosedTransaction(Order ord)
        {
            if (ord.Status == Enums.OrderStatus.נסגר_בהענות_לקוח)
                return true;
            return false;
        }

        /// <summary>
        /// Checking which free hosting unit is on a certain date for several days
        /// </summary>
        /// <param name="date">the first day <param name="numOfDays">num of vacation days</param>
        /// <returns> List of available Hosting units  </returns>
        public static List<HostingUnit> getAllFreeUnitsInDate(DateTime date, int numOfDays)
        {
            List<HostingUnit> lst = dl.getAllUnits();
            List<HostingUnit> lstReturned = new List<HostingUnit>();
            foreach (var it in lst)
            {
                bool isFree = true;
                int i = numOfDays;
                int day = date.Day;
                int month = date.Month;
                while (i != 0)
                {
                    if (it.Diary[month - 1, day - 1])
                    {
                        isFree = false;
                        i = 0;
                    }
                    else
                    {
                        day++;
                        if (day > 30)
                        {
                            day = 0;
                            month++;
                        }
                        i--;
                    }
                }
                if (isFree)
                {
                    lstReturned.Add(it);
                }
            }
            return lstReturned;
        }

        /// <summary>
        /// the num of the days in guest request dates
        /// </summary>
        /// <param name="req">the request we want to check the num of vacation days</param>
        /// <returns> (int) num of days from entry date to release </returns>
        public static int getNumOfHolidayDays(GuestRequest req)
        {
            int num = 1;
            DateTime start = req.EntryDate;
            while (start != req.ReleaseDate)
            {
                num++;
                start = start.AddDays(1);
            }
            return num;
        }

        /// <summary>
        /// Check the num of the order that send to a customer
        /// </summary>
        /// <param name="req">the guest request to check</param>
        /// <returns> (int) num of Orders </returns>
        public static int getNumOfOrdersToRequest(GuestRequest req)
        {
            int num = dl.getAllOrders().Count(item => item.GuestRequestKey == req.GuestRequestKey);
            return num;
        }

        /// <summary>
        /// the num of the closed Orders in specific HostingUnit
        /// </summary>
        /// <param name="unit">the HostingUnit to check</param>
        /// <returns> (int) num of closed Order </returns>
        public static int getNumOfClosedOrder(HostingUnit unit)
        {
            int num = dl.getAllOrders().Count(item => (item.HostingUnitKey == unit.HostingUnitKey && ClosedTransaction(item)));
            return num;
        }

        /// <summary>
        /// we want get sub group of requets that requesting to specific area 
        /// than we can filter suggestions for hosts
        /// </summary>
        /// <returns> a groups of GuestReques sorted by Area </returns>
        public static IEnumerable<IGrouping<BE.Enums.Area, BE.GuestRequest>> groups_req_by_area()
        {
            var AreaGroups = dl.getAllGuestReqs().GroupBy(it => it.area);
            return AreaGroups;
        }



        /// <summary>
        /// we want get sub group of requets that requesting to specific num of vacationers
        /// than we can filter suggestions for hosts
        /// </summary>
        /// <returns> a groups of GuestReques sorted by num of vacationers </returns>
        public static IEnumerable<IGrouping<int, GuestRequest>> group_req_by_num_of_Vacationers()
        {
            var numVacGroup = dl.getAllGuestReqs().GroupBy(it => it.Children + it.Adults);
            return numVacGroup;
        }

        //-------------------------------------------------------------------------
        /// <summary>
        ///This is an auxiliary function until the following function 
        ///counts the number of units a host has
        /// </summary>
        /// <param name="host">the Host</param>
        /// <returns> (int) num of this Host HostingUnits </returns>
        public static int numOfHostUnits(Host host)
        {
            int num = 0;
            foreach (var item in dl.getAllUnits())
            {
                if (item.Owner.HostKey == host.HostKey)
                    num++;
            }
            return num;
        }

        /// <summary>
        /// we want get sub groups of Hosts By their number of units
        /// </summary>
        /// <returns> a groups of Hosts sorted by num of HostingUnits </returns>
        public static IEnumerable<IGrouping<int, Host>> group_by_num_of_hostingUnit()
        {
            List<Host> lst = dl.getAllHosts(); 
            var HostGroup = lst.GroupBy(it => numOfHostUnits(it));
            return HostGroup;
        }
       

        /// <summary>
        /// we want get sub group of HostingUnits that places in specific area 
        /// than we can filter suggestions for customers
        /// </summary>
        /// <returns> a groups of Units sorted by Area </returns>
        public static IEnumerable<IGrouping<BE.Enums.Area, HostingUnit>> group_units_by_area()
        {
            var unitsGroup = dl.getAllUnits().GroupBy(it => it.Area);
            return unitsGroup;
        }

        /// <summary>
        /// we want get sub group of GuestRequest that places in specific area 
        /// than we can filter suggestions for Hosts
        /// </summary>
        /// <returns> a groups of Units sorted by Area </returns>
        public static IEnumerable<IGrouping<BE.Enums.Area, GuestRequest>> group_reqs_by_area()
        {
            var reqGroup = dl.getAllGuestReqs().GroupBy(it => it.area);
            return reqGroup;
        }

        /// <summary>
        /// we want get sub group of Order that has specific Status 
        /// than we can filter suggestions for Hosts
        /// </summary>
        /// <returns> a groups of Order sorted by Status </returns>
        public static IEnumerable<IGrouping<BE.Enums.OrderStatus, Order>> group_order_by_status(List<Order> lst)
        {
            return lst.GroupBy(it => it.Status);
        }

        /// <summary>
        ///We want to return hosts to the requests that are right for them,.
        ///so this function will filter according to the delegate it accepts.
        /// Boolean Diligate so that if a certain condition happens the request is returned
        /// </summary>
        /// <param name="isReq"></param>
        /// <returns> return IEnumerable with all request with true in the condition<GuestRequest> </returns>
        public delegate bool delegateRequest(GuestRequest req); 
        public static IEnumerable<GuestRequest> reqByDelegat(delegateRequest isReq)
        {
            
           var lst = from it in dl.getAllGuestReqs()
                     where isReq(it)
                     select it;
            return lst;
        }

        /// <summary>
        /// returned all the Units of specific Host
        /// </summary>
        /// <param name="thisHost"></param>
        /// <returns></returns>
        public static List<HostingUnit> getAllHostUnits(string thisHost)
        {
            List<HostingUnit> lst = (from it in dl.getAllUnits()
                                     where it.Owner.HostKey == thisHost
                                     select it).ToList();
            return lst;
        }
//-----------------------------------------------------------------------------
        /// <summary>
        /// returned all the Orders of specific Host
        /// </summary>
        /// <param name="thisHost"></param>
        /// <returns></returns>
        public static List<Order> getAllHostOrders(string thisHost)
        {
            
            List<Order> lst = (from it in dl.getAllOrders()
                                     where OrderToHost_isExiest(it.OrderKey,thisHost)==true
                                     select it).ToList();
            return lst;
        }

        public static bool OrderToHost_isExiest(string ord , string hst)
        {
            if (getUnitByKey(getOrderByKey(ord).HostingUnitKey).Owner.HostKey == hst)
                return true;
            return false;
        }
 //-----------------------------------------------------------------------------------------------
        /// <summary>
        /// return HostingUnitKey by the HostinUnitName
        /// </summary>
       public static string getUnitKeyByUnitName(string name)
        {
            foreach(var it in dl.getAllUnits())
            {
                if (it.HostingUnitName == name)
                    return it.HostingUnitKey;
            }
            return "";
        }
        //-------------------------------------------------------------------------------------------------
        /// <summary>
        /// Checks whether the request has expired
        /// </summary>
        /// <param name="req"></param>
        public static bool isRequestExpired(GuestRequest req)
        {
            int num = 0;
            DateTime reqDate = req.RegistrationDate;
            while(reqDate!=DateTime.Today)
            {
               reqDate=reqDate.AddDays(1);
                num++;
            }
            if (num >= BE.Configuration.RequestExpired)
                return true;
            return false;
        }
        /// <summary>
        /// Checks if the request closed succsefully by Order in the sysem 
        /// </summary>
        /// <param name="req"></param>
        public static bool isReqClosedByWeb(GuestRequest req)
        {
            foreach(var it in dl.getAllOrders())
            {
                if (it.GuestRequestKey == req.GuestRequestKey && it.Status == Enums.OrderStatus.נסגר_בהענות_לקוח)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// checking if order is Expired
        /// </summary>
        /// <param name="ord"></param>
        /// <returns>boolean value</returns>
        public static bool OrderExp(Order ord)
        {
            int counter = 0;
            while(ord.CreateDate!=DateTime.Today)
            {
                ord.CreateDate = ord.CreateDate.AddDays(1);
                counter++;
            }
            if (counter > Configuration.OrderExpired)
                return true;
            return false;
        }
        /// <summary>
        /// if configuration changed 
        /// </summary>
        public static void ChangeConfig()
        {
            Dal_XML_imp.isConfigToUpd = true;
        }
    }
}
