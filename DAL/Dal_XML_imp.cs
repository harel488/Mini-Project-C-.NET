using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Net;

using BE;
namespace DAL
{
    public class Dal_XML_imp:Idal
    {

        static Dal_XML_imp instance = null;
        public static Dal_XML_imp GetInstance()
        {
            if (instance == null)
                instance = new Dal_XML_imp();
            return instance;
        }

        //Matrix xml problem
         public static void SaveToXML(bool[,] source, XElement ELM)
         {
            for (int i = 0; i < source.GetLength(0); i++)
            {
                XElement row = new XElement("row"); 
                for (int j = 0; j < source.GetLength(1); j++)
                    row.Add(new  XElement("item", source[i, j]));
                ELM.Element("Diary").Add(row);
            }
        }
         public static bool[,] LoadFromXML(XElement ELM)
         {
            int i = 0, j = 0;
            bool[,] matrix = new bool[12, 31];
            foreach(var it in ELM.Elements())
            {
                foreach(var it2 in it.Elements())
                {
                    matrix[i, j] = Convert.ToBoolean( it2.Value);
                    j++;
                }
                i++;
                j = 0;
            }
            return matrix;
         }




        //----------------------------------------------------------------------------
        public static bool isConfigToUpd = false;

        XElement GuestReqRoot;
        string GuestReqPath;

        XElement HostingUnitRoot;
        string HostingUnitPath;

        XElement OrderRoot;
        string OrderPath;

        XElement BankBranchRoot;
        string BankBranchPath;

        XElement ConfigRoot;
        string ConfigPath;


        //ctor------------------------------------------------------------------
       private Dal_XML_imp()
        {

            Thread configUpdateTread = new Thread(UpdConfig);
            configUpdateTread.Start();
            string str = Assembly.GetExecutingAssembly().Location;
            string localPath = Path.GetDirectoryName(str);

            GuestReqPath = localPath + @"\GuestReqs.xml";
            HostingUnitPath= localPath + @"\HostingUnits.xml";
            OrderPath= localPath + @"\Orders.xml"; 
            BankBranchPath= localPath + @"\atm.xml";
            ConfigPath= localPath + @"\Configs.xml";

            if (!File.Exists(GuestReqPath))
                CreateReqsFile();

            if (!File.Exists(HostingUnitPath))
                CreateUnitsFile();

            if (!File.Exists(OrderPath))
                CreateOrdersFile();

            if (!File.Exists(BankBranchPath))
                CreateBankFile();

            if (!File.Exists(ConfigPath))
                CreateConfigFile();


            UpdConfigClass();


        }
        //---------------------------------------------------------------------------
       
            
        
       
            
        /// <summary>
        /// Dwonloading the atm.XML from bank israel site
        /// </summary>
        void DownloadBankBranchesXML()
        {
            const string xmlLocalPath = @"atm.xml";
            WebClient wc = new WebClient();
            try
            {
                string xmlServerPath =
                @"http://www.boi.org.il/he/BankingSupervision/BanksAndBranchLocations/Lists/BoiBankBranchesDocs/at
            m.xml";
                wc.DownloadFile(xmlServerPath, xmlLocalPath);
            }
            catch (Exception)
            {
                wc.Dispose();
                string xmlServerPath = @"http://www.jct.ac.il/~coshri/atm.xml";
                wc.DownloadFile(xmlServerPath, xmlLocalPath);
            }
            
        }
//create------------------------------------------------------------------------
        void CreateReqsFile()
        {
            GuestReqRoot = new XElement("GuestReqs");
            GuestReqRoot.Save(GuestReqPath);
        }

       void CreateUnitsFile()
        {
            HostingUnitRoot = new XElement("HostingUnits");
            HostingUnitRoot.Save(HostingUnitPath);
        }

        void CreateOrdersFile()
        {
            OrderRoot = new XElement("Orders");
            OrderRoot.Save(OrderPath);
        }

        void CreateConfigFile()
        {
            ConfigRoot = new XElement("Configs");
            ConfigRoot.Add(new XElement("HostingUnitKey", Configuration.HostingUnitKey.ToString()));
            ConfigRoot.Add(new XElement("GuestRequestKey", Configuration.GuestRequestKey.ToString()));
            ConfigRoot.Add(new XElement("OrderKey", Configuration.OrderKey.ToString()));
            ConfigRoot.Add(new XElement("commission", Configuration.commission.ToString()));
            ConfigRoot.Add(new XElement("Profits", Configuration.Profits.ToString()));
            ConfigRoot.Add(new XElement("AdminPassword", Configuration.AdminPassword));
            ConfigRoot.Add(new XElement("RequestExpired", Configuration.RequestExpired.ToString()));
            ConfigRoot.Add(new XElement("OrderExpired", Configuration.OrderExpired.ToString()));
            ConfigRoot.Add(new XElement("NumClosedOrders", Configuration.NumClosedOrders.ToString()));
            ConfigRoot.Add(new XElement("NumOfAllReqs", Configuration.NumOfAllReqs.ToString()));
            ConfigRoot.Add(new XElement("NumOfUnits", Configuration.NumOfUnits.ToString()));
            ConfigRoot.Save(ConfigPath);
        }



        void CreateBankFile()
        {
          
              DownloadBankBranchesXML();
              BankBranchRoot.Save(BankBranchPath);
        }

        //----------------------------------------------------------------------------


        //Converting-------------------------------------------------           

        GuestRequest XElementGuestRequest_To_GuestRequest(XElement ELM)
        {
            GuestRequest req = new GuestRequest();
            req.GuestRequestKey = ELM.Element("GuestRequestKey").Value;
            req.PrivateName = ELM.Element("PrivateName").Value;
            req.FamilyName = ELM.Element("FamilyName").Value;
            req.MailAddress = ELM.Element("MailAddress").Value;
            req.Status = (Enums.GuestReqStatus)Enum.Parse(typeof(Enums.GuestReqStatus), ELM.Element("Status").Value);
            req.RegistrationDate = new DateTime(int.Parse(ELM.Element("RegistrationDate").Element("Year").Value),
                                                       int.Parse(ELM.Element("RegistrationDate").Element("Month").Value),
                                                       int.Parse(ELM.Element("RegistrationDate").Element("Day").Value));
            req.EntryDate = new DateTime(int.Parse(ELM.Element("EntryDate").Element("Year").Value),
                                                       int.Parse(ELM.Element("EntryDate").Element("Month").Value),
                                                       int.Parse(ELM.Element("EntryDate").Element("Day").Value));
            req.ReleaseDate = new DateTime(int.Parse(ELM.Element("ReleaseDate").Element("Year").Value),
                                                       int.Parse(ELM.Element("ReleaseDate").Element("Month").Value),
                                                       int.Parse(ELM.Element("ReleaseDate").Element("Day").Value));
            req.area = (Enums.Area)Enum.Parse(typeof(Enums.Area), ELM.Element("area").Value);
            req.Type = (Enums.HostUnitType)Enum.Parse(typeof(Enums.HostUnitType), ELM.Element("Type").Value);
            req.Adults = int.Parse(ELM.Element("Adults").Value);
            req.Children = int.Parse(ELM.Element("Children").Value);
            req.Pool = bool.Parse(ELM.Element("Pool").Value);
            req.ChildrensAttractions = bool.Parse(ELM.Element("ChildrensAttractions").Value);


                return req;
        }

        XElement GuestRequest_To_XElementGuestRequest(GuestRequest req)
        {
            XElement key = new XElement("GuestRequestKey", req.GuestRequestKey);
            XElement Pname = new XElement("PrivateName", req.PrivateName);
            XElement Fname = new XElement("FamilyName", req.FamilyName);
            XElement mail = new XElement("MailAddress", req.MailAddress);
            XElement status = new XElement("Status", req.Status.ToString());
            XElement regDateYear = new XElement("Year", req.RegistrationDate.Year);
            XElement regDateMonth = new XElement("Month", req.RegistrationDate.Month);
            XElement regDateDay = new XElement("Day", req.RegistrationDate.Day);
            XElement regDate = new XElement("RegistrationDate", regDateYear, regDateMonth, regDateDay);
            XElement entDateYear = new XElement("Year", req.EntryDate.Year);
            XElement entDateMonth = new XElement("Month", req.EntryDate.Month);
            XElement entDateDay = new XElement("Day", req.EntryDate.Day);
            XElement entDate = new XElement("EntryDate", entDateYear, entDateMonth, entDateDay);
            XElement relDateYear = new XElement("Year", req.ReleaseDate.Year);
            XElement relDateMonth = new XElement("Month", req.ReleaseDate.Month);
            XElement relDateDay = new XElement("Day", req.ReleaseDate.Day);
            XElement relDate = new XElement("ReleaseDate", relDateYear, relDateMonth, relDateDay);
            XElement _area = new XElement("area", req.area.ToString());
            XElement _type = new XElement("Type", req.Type.ToString());
            XElement _adults = new XElement("Adults", req.Adults);
            XElement _childrens = new XElement("Children", req.Children);
            XElement _pool = new XElement("Pool", req.Pool);
            XElement _attractions = new XElement("ChildrensAttractions", req.ChildrensAttractions);

            XElement NewObj = new XElement("GuestRequest", key, Pname, Fname, mail, status,
                regDate, entDate, relDate, _area, _type, _adults, _childrens, _pool, _attractions);

            return NewObj;
        }

        //File Load

        void GuestRequest_FileLoad()
        {
            try
            {
                GuestReqRoot = XElement.Load(GuestReqPath);
            }
            catch
            {
                throw new FileLoadException(" בעיה בטעינת קובץ");
            }
        }

        //Exiest
        bool GuestRequestExiest(string key)
        {
            try
            {
                GuestRequest_FileLoad();
                foreach(var it in GuestReqRoot.Elements())
                {
                    if (it.Element("GuestRequestKey").Value == key)
                        return true;
                }
                return false;
            }
            catch(FileLoadException a)
            {
                throw a;
            }
        }
        
        //ADD
        void Idal.AddGuestReq(GuestRequest newRequest)
        {
            GuestRequest_FileLoad();
            newRequest.GuestRequestKey = Configuration.GuestRequestKey.ToString();
            if (GuestRequestExiest(newRequest.GuestRequestKey))
                throw new ArgumentException("מפתח כבר קיים");
            Configuration.NumOfAllReqs++;
            Configuration.GuestRequestKey++;
            //UpdConfig();
            isConfigToUpd = true;
            GuestReqRoot.Add(GuestRequest_To_XElementGuestRequest(newRequest));
            GuestReqRoot.Save(GuestReqPath);
        }

        //Update
        void Idal.updateGuestReq(GuestRequest ReqToUpdate)
        {
            GuestRequest_FileLoad();
            try
            {
               
                if (GuestRequestExiest(ReqToUpdate.GuestRequestKey))
                {
                    (from it in GuestReqRoot.Elements()
                     where it.Element("GuestRequestKey").Value == ReqToUpdate.GuestRequestKey
                     select it).FirstOrDefault().Remove();
                    GuestReqRoot.Add(GuestRequest_To_XElementGuestRequest(ReqToUpdate));
                    GuestReqRoot.Save(GuestReqPath);
                    //UpdConfig();
                    isConfigToUpd = true;
                }
                else
                    throw new KeyNotFoundException("מפתח לא קיים");
            }
            catch (FileLoadException a)
            {
                throw a;
            }
            catch (FileNotFoundException a)
            {
                throw a;
            }
        }
           
        
        //Delete
        void Idal.DeleteGuestReq(GuestRequest ReqToDel)
        {
            GuestRequest_FileLoad();
            try
            {

                if (GuestRequestExiest(ReqToDel.GuestRequestKey))
                {
                    (from it in GuestReqRoot.Elements()
                     where it.Element("GuestRequestKey").Value == ReqToDel.GuestRequestKey
                     select it).FirstOrDefault().Remove();

                    Configuration.NumOfAllReqs--;
                    // UpdConfig();
                    isConfigToUpd = true;
                    GuestReqRoot.Save(GuestReqPath);
                }
                else
                    throw new KeyNotFoundException("מפתח לא קיים");
            }
            catch (FileLoadException a)
            {
                throw a;
            }
            catch (FileNotFoundException a)
            {
                throw a;
            }

        }

        //GET
        GuestRequest Idal.getGuestReq(GuestRequest req)
        {
            try
            {
                GuestRequest_FileLoad();
                if (GuestRequestExiest(req.GuestRequestKey))
                {
                  foreach(var it in GuestReqRoot.Elements())
                    {
                        if (it.Element("GuestRequestKey").Value == req.GuestRequestKey)
                            return XElementGuestRequest_To_GuestRequest(it);
                    }
                }
                throw new KeyNotFoundException("מפתח לא קיים");

            }
            catch (FileLoadException a)
            {
                throw a;
            }
        }

        //-----------------------------------------------------------------------------------       
        //BankBranch------------------------------------------------------------------------

        //CONVERTING
        BankBranch XElementBankBranch_To_BankBranch(XElement ELM)
        {
            return new BankBranch()
            {
                BankNumber = ELM.Element("BankNumber").Value,
                BankName = ELM.Element("BankName").Value,
                BranchNumber = ELM.Element("BranchNumber").Value,
                BranchAddress = ELM.Element("BranchAddress").Value,
                BranchCity = ELM.Element("BranchCity").Value

            };
        }

        XElement Branch_To_XelementBankBranch(BankBranch branch)
        {
            XElement _bankNum = new XElement("BankNumber", branch.BankNumber);
            XElement _bankName = new XElement("BankName", branch.BankName);
            XElement _branchNum = new XElement("BranchNumber", branch.BranchNumber);
            XElement _branchAddress = new XElement("BranchAddress", branch.BranchAddress);
            XElement _BranchCity = new XElement("BranchCity", branch.BranchCity);

            XElement obj = new XElement("HostBankAccuont", _bankNum, _bankName, _branchNum, _branchAddress, _BranchCity);
            return obj;
        }

        void BankBranch_FileLoad()
        {
            try
            {
                BankBranchRoot = XElement.Load(BankBranchPath);
            }
            catch
            {
                throw new FileLoadException(" בעיה בטעינת קובץ");
            }
        }

        //Exiest
        bool BankBranchExiest(string BankKey,string BranchKey)
        {
            try
            {
                BankBranch_FileLoad();
                foreach (var it in BankBranchRoot.Elements())
                {
                    if (it.Element("קוד_בנק").Value == BankKey&&it.Element("קוד_סניף").Value==BranchKey)
                        return true;
                }
                return false;
            }
            catch (FileLoadException a)
            {
                throw a;
            }
        }

        //GET
        BankBranch getBankBranch(BankBranch branch)
        {
            try
            {
               BankBranch_FileLoad();
                if (BankBranchExiest(branch.BankNumber,branch.BranchNumber))
                {
                    foreach (var it in BankBranchRoot.Elements())
                    {
                        if (it.Element("קוד_בנק").Value == branch.BankNumber && it.Element("קוד_סניף").Value == branch.BranchNumber)
                            return XElementBankBranch_To_BankBranch(it);
                    }
                }
                throw new KeyNotFoundException("מפתח לא קיים");

            }
            catch (FileLoadException a)
            {
                throw a;
            }
        }
//----------------------------------------------------------------------------------------------
//HostingUnit----------------------------------------------------------------------------------



//Converting
        HostingUnit XElementHostingUniut_To_HostingUnit(XElement ELM)
        {
            HostingUnit Unit = new HostingUnit();
            Unit.Owner = new Host();
            Unit.Owner.HostBankAccuont = new BankBranch();

            Unit.HostingUnitKey = ELM.Element("HostingUnitKey").Value;
            Unit.Type = (Enums.HostUnitType)Enum.Parse(typeof(Enums.HostUnitType), ELM.Element("Type").Value);
            Unit.Owner = XElementHost_To_Host(ELM.Element("Host"));
            Unit.HostingUnitName = ELM.Element("HostingUnitName").Value;
            Unit.Area= (Enums.Area)Enum.Parse(typeof(Enums.Area), ELM.Element("Area").Value);
            Unit.Price = ELM.Element("Price").Value;
            Unit.Diary = LoadFromXML(ELM.Element("Diary"));

            return Unit;
        }

        XElement HostingUnit_To_XElementHostingUnit(HostingUnit unit)
        {
            XElement _hostingUnitKey = new XElement("HostingUnitKey", unit.HostingUnitKey);
            XElement _type = new XElement("Type", unit.Type.ToString());
            XElement _owner = Host_To_XElementHost(unit.Owner);
            XElement _hostingUnitName = new XElement("HostingUnitName", unit.HostingUnitName);
            XElement _area = new XElement("Area", unit.Area.ToString());
            XElement _price = new XElement("Price", unit.Price);
            XElement _diary = new XElement("Diary");
            XElement obj = new XElement("HostingUnit", _hostingUnitKey,_type,_owner,_hostingUnitName,_area, _price,_diary);
            SaveToXML(unit.Diary,obj);
            return obj;


        }

        //File Load

        void HostingUnit_FileLoad()
        {
            try
            {
                HostingUnitRoot = XElement.Load(HostingUnitPath);
            }
            catch
            {
                throw new FileLoadException(" בעיה בטעינת קובץ");
            }
        }

        //Exiest
        bool HostingUnitExiest(string key)
        {
            try
            {
                HostingUnit_FileLoad();
                foreach (var it in HostingUnitRoot.Elements())
                {
                    if (it.Element("HostingUnitKey").Value == key)
                        return true;
                }
                return false;
            }
            catch (FileLoadException a)
            {
                throw a;
            }
        }

        //ADD
        void Idal.addHostUnit(HostingUnit newUnit)
        {
            HostingUnit_FileLoad();
            newUnit.HostingUnitKey = Configuration.HostingUnitKey.ToString();
            if (HostingUnitExiest(newUnit.HostingUnitKey))
                throw new ArgumentException("מפתח כבר קיים");
            Configuration.HostingUnitKey++;
            Configuration.NumOfUnits++;
            //UpdConfig();
            isConfigToUpd = true;
            HostingUnitRoot.Add(HostingUnit_To_XElementHostingUnit(newUnit));
            HostingUnitRoot.Save(HostingUnitPath);
        }

        //Update
        void Idal.updateUnit(HostingUnit unitToUpdate)
        {
            HostingUnit_FileLoad();
            try
            {

                if (HostingUnitExiest(unitToUpdate.HostingUnitKey))
                {
                    (from it in HostingUnitRoot.Elements()
                     where it.Element("HostingUnitKey").Value == unitToUpdate.HostingUnitKey
                     select it).FirstOrDefault().Remove();
                    HostingUnitRoot.Add(HostingUnit_To_XElementHostingUnit(unitToUpdate));
                    //UpdConfig();
                    isConfigToUpd = true;
                    HostingUnitRoot.Save(HostingUnitPath);
                }
                else
                    throw new KeyNotFoundException("מפתח לא קיים");
            }
            catch (FileLoadException a)
            {
                throw a;
            }
            catch (FileNotFoundException a)
            {
                throw a;
            }
        }


        //Delete
        void Idal.deleteUnit(HostingUnit UnitToDel)
        {
            HostingUnit_FileLoad();
            try
            {

                if (HostingUnitExiest(UnitToDel.HostingUnitKey))
                {
                    (from it in HostingUnitRoot.Elements()
                     where it.Element("HostingUnitKey").Value == UnitToDel.HostingUnitKey
                     select it).FirstOrDefault().Remove();
                    Configuration.NumOfUnits--;
                    //UpdConfig();
                    isConfigToUpd = true;
                    HostingUnitRoot.Save(HostingUnitPath);
                }
                else
                    throw new KeyNotFoundException("מפתח לא קיים");
            }
            catch (FileLoadException a)
            {
                throw a;
            }
            catch (FileNotFoundException a)
            {
                throw a;
            }

        }

        //GET
        HostingUnit Idal.GetHostingUnit(HostingUnit unit)
        {
            try
            {
                HostingUnit_FileLoad();
                if (HostingUnitExiest(unit.HostingUnitKey))
                {
                    foreach (var it in HostingUnitRoot.Elements())
                    {
                        if (it.Element("HostingUnitKey").Value == unit.HostingUnitKey)
                            return XElementHostingUniut_To_HostingUnit(it);
                    }
                }
                throw new KeyNotFoundException("מפתח לא קיים");

            }
            catch (FileLoadException a)
            {
                throw a;
            }
        }
//---------------------------------------------------------------------------------
//HOST

  //Converting
        Host XElementHost_To_Host(XElement ELM)
        {
            Host obj = new Host();

            obj.HostKey = ELM.Element("HostKey").Value;
            obj.PrivateName = ELM.Element("PrivateName").Value;
            obj.FamilyName = ELM.Element("FamilyName").Value;
            obj.PhoneNumber = ELM.Element("PhoneNumber").Value;
            obj.MailAddress = ELM.Element("MailAddress").Value;
            obj.BankAccountNumber = ELM.Element("BankAccountNumber").Value;
            obj.HostBankAccuont = XElementBankBranch_To_BankBranch(ELM.Element("HostBankAccuont"));
            obj.CollectionClearance = bool.Parse(ELM.Element("CollectionClearance").Value);
            return obj;
           
        }

        XElement Host_To_XElementHost(Host host)
        {
            XElement _hostKey = new XElement("HostKey", host.HostKey);
            XElement _pName = new XElement("PrivateName", host.PrivateName);
            XElement _fName = new XElement("FamilyName", host.FamilyName);
            XElement _phoneNum = new XElement("PhoneNumber", host.PhoneNumber);
            XElement _mail = new XElement("MailAddress", host.MailAddress);
            XElement _accountNum = new XElement("BankAccountNumber", host.BankAccountNumber);
            XElement _bank = new XElement(Branch_To_XelementBankBranch(host.HostBankAccuont));
            XElement _collectionClearance = new XElement("CollectionClearance", host.CollectionClearance);

            XElement obj = new XElement("Host", _hostKey, _pName, _fName, _phoneNum, _mail, _accountNum, _bank, _collectionClearance);
            return obj;
        }

        //Order
        //Converting
        Order XElementOrder_To_Order(XElement ELM)
        {

            Order obj = new Order();

            obj.HostingUnitKey = ELM.Element("HostingUnitKey").Value;
            obj.GuestRequestKey = ELM.Element("GuestRequestKey").Value;
            obj.OrderKey = ELM.Element("OrderKey").Value;
            obj.Status = (Enums.OrderStatus)Enum.Parse(typeof(Enums.OrderStatus), ELM.Element("Status").Value);
            obj.CreateDate = new DateTime(int.Parse(ELM.Element("CreateDate").Element("Year").Value),
                                                   int.Parse(ELM.Element("CreateDate").Element("Month").Value),
                                                   int.Parse(ELM.Element("CreateDate").Element("Day").Value));
            obj.OrderDate = new DateTime(int.Parse(ELM.Element("OrderDate").Element("Year").Value),
                                                    int.Parse(ELM.Element("OrderDate").Element("Month").Value),
                                                    int.Parse(ELM.Element("OrderDate").Element("Day").Value));
            return obj;
            
        }

        XElement Order_To_XElementOrder(Order ord)
        {
            XElement _hostingUnitKey = new XElement("HostingUnitKey", ord.HostingUnitKey);
            XElement _guestRequestKey = new XElement("GuestRequestKey", ord.GuestRequestKey);
            XElement _orderKey = new XElement("OrderKey", ord.OrderKey);
            XElement _status = new XElement("Status", ord.Status.ToString());

            XElement _createDateYear = new XElement("Year", ord.CreateDate.Year);
            XElement _createDateMonth = new XElement("Month", ord.CreateDate.Month);
            XElement _createDateDay = new XElement("Day", ord.CreateDate.Day);
            XElement _createDate = new XElement("CreateDate", _createDateYear, _createDateMonth, _createDateDay);

            XElement _orderDateYear = new XElement("Year", ord.OrderDate.Year);
            XElement _orderDateMonth = new XElement("Month", ord.OrderDate.Month);
            XElement _orderDateDay = new XElement("Day", ord.OrderDate.Day);
            XElement _orderDate = new XElement("OrderDate", _orderDateYear, _orderDateMonth, _orderDateDay);

            XElement obj = new XElement("Order", _hostingUnitKey,_guestRequestKey,_orderKey,_status,_createDate,_orderDate);
            return obj;
        }


        //File Load

        void Order_FileLoad()
        {
            try
            {
                OrderRoot = XElement.Load(OrderPath);
            }
            catch
            {
                throw new FileLoadException(" בעיה בטעינת קובץ");
            }
        }

        //Exiest
        bool OrderExiest(string key)
        {
            try
            {
                Order_FileLoad();
                foreach (var it in OrderRoot.Elements())
                {
                    if (it.Element("OrderKey").Value == key)
                        return true;
                }
                return false;
            }
            catch (FileLoadException a)
            {
                throw a;
            }
        }

        //ADD
        void Idal.addOrder(Order newOrder)
        {
            Order_FileLoad();
            newOrder.OrderKey = Configuration.OrderKey.ToString();
            if (OrderExiest(newOrder.OrderKey))
                throw new ArgumentException("מפתח כבר קיים");
            Configuration.OrderKey++;
            isConfigToUpd = true;
            //UpdConfig();
            OrderRoot.Add(Order_To_XElementOrder(newOrder));
            OrderRoot.Save(OrderPath);
        }

        //Update
        void Idal.updateOrder(Order OrderToUpdate)
        {
            Order_FileLoad();
            try
            {

                if (OrderExiest(OrderToUpdate.OrderKey))
                {
                    (from it in OrderRoot.Elements()
                     where it.Element("OrderKey").Value == OrderToUpdate.OrderKey
                     select it).FirstOrDefault().Remove();
                    OrderRoot.Add(Order_To_XElementOrder(OrderToUpdate));
                    //UpdConfig();
                    isConfigToUpd = true;
                    OrderRoot.Save(OrderPath);
                }
                else
                    throw new KeyNotFoundException("מפתח לא קיים");
            }
            catch (FileLoadException a)
            {
                throw a;
            }
            catch (FileNotFoundException a)
            {
                throw a;
            }
        }


        //Delete
        void Idal.DeleteOrder(Order OrderToDel)
        {
            Order_FileLoad();
            try
            {

                if (OrderExiest(OrderToDel.OrderKey))
                {
                    (from it in OrderRoot.Elements()
                     where it.Element("OrderKey").Value == OrderToDel.OrderKey
                     select it).FirstOrDefault().Remove();
                    //UpdConfig();
                    isConfigToUpd = true;
                    OrderRoot.Save(OrderPath);
                }
                else
                    throw new KeyNotFoundException("מפתח לא קיים");
            }
            catch (FileLoadException a)
            {
                throw a;
            }
            catch (FileNotFoundException a)
            {
                throw a;
            }

        }

        //GET
        Order Idal.getOrder(Order ord)
        {
            try
            {
                Order_FileLoad();
                if (OrderExiest(ord.OrderKey))
                {
                    foreach (var it in OrderRoot.Elements())
                    {
                        if (it.Element("OrderKey").Value == ord.OrderKey)
                            return XElementOrder_To_Order(it);
                    }
                }
                throw new KeyNotFoundException("מפתח לא קיים");

            }
            catch (FileLoadException a)
            {
                throw a;
            }
        }

        //----------------------------------------------------------------------------------------------
        //Config

        void Config_FileLoad()
        {
            try
            {
               ConfigRoot = XElement.Load(ConfigPath);
            }
            catch
            {
                throw new FileLoadException(" בעיה בטעינת קובץ");
            }
        }


        public void UpdConfig()
        {

            while (true)
            {
                if (isConfigToUpd)
                {
                    isConfigToUpd = false;
                    Config_FileLoad();
                    ConfigRoot.Element("GuestRequestKey").Value = Configuration.GuestRequestKey.ToString();
                    ConfigRoot.Element("AdminPassword").Value = Configuration.AdminPassword.ToString();
                    ConfigRoot.Element("commission").Value = Configuration.commission.ToString();
                    ConfigRoot.Element("HostingUnitKey").Value = Configuration.HostingUnitKey.ToString();
                    ConfigRoot.Element("NumClosedOrders").Value = Configuration.NumClosedOrders.ToString();
                    ConfigRoot.Element("NumOfAllReqs").Value = Configuration.NumOfAllReqs.ToString();
                    ConfigRoot.Element("NumOfUnits").Value = Configuration.NumOfUnits.ToString();
                    ConfigRoot.Element("OrderExpired").Value = Configuration.OrderExpired.ToString();
                    ConfigRoot.Element("OrderKey").Value = Configuration.OrderKey.ToString();
                    ConfigRoot.Element("Profits").Value = Configuration.Profits.ToString();
                    ConfigRoot.Element("RequestExpired").Value = Configuration.RequestExpired.ToString();

                    ConfigRoot.Save(ConfigPath);
                }
                Thread.Sleep(1000);
            }
        }
            
        void UpdConfigClass()
        {
            Config_FileLoad();
            Configuration.AdminPassword = ConfigRoot.Element("AdminPassword").Value;
            Configuration.commission = Convert.ToInt32(ConfigRoot.Element("commission").Value);
            Configuration.GuestRequestKey = Convert.ToInt32(ConfigRoot.Element("GuestRequestKey").Value);
            Configuration.HostingUnitKey = Convert.ToInt32(ConfigRoot.Element("HostingUnitKey").Value);
            Configuration.NumClosedOrders = Convert.ToInt32(ConfigRoot.Element("NumClosedOrders").Value);
            Configuration.NumOfAllReqs = Convert.ToInt32(ConfigRoot.Element("NumOfAllReqs").Value);
            Configuration.NumOfUnits = Convert.ToInt32(ConfigRoot.Element("NumOfUnits").Value);
            Configuration.OrderExpired = Convert.ToInt32(ConfigRoot.Element("OrderExpired").Value);
            Configuration.OrderKey = Convert.ToInt32(ConfigRoot.Element("OrderKey").Value);
            Configuration.Profits = Convert.ToInt32(ConfigRoot.Element("Profits").Value);
            Configuration.RequestExpired = Convert.ToInt32(ConfigRoot.Element("RequestExpired").Value);

            ConfigRoot.Save(ConfigPath);
        }
        










        //ALL data return

        List<HostingUnit> Idal.getAllUnits()
        {
            HostingUnit_FileLoad();
            List<HostingUnit> lst = new List<HostingUnit>();
            foreach (var it in HostingUnitRoot.Elements())
            {
                HostingUnit obj = XElementHostingUniut_To_HostingUnit(it);
                lst.Add(obj);
            }
            return lst;




        }



        List<GuestRequest> Idal.getAllGuestReqs()
        {
            GuestRequest_FileLoad();
            List<GuestRequest> lst = new List<GuestRequest>();
            foreach(var it in GuestReqRoot.Elements())
            {
                GuestRequest obj = XElementGuestRequest_To_GuestRequest(it);
                lst.Add(obj);
            }
            return lst;
        }




        List<Order> Idal.getAllOrders()
        {
            Order_FileLoad();
            List<Order> lst = new List<Order>();
            foreach (var it in OrderRoot.Elements())
            {
                Order obj = XElementOrder_To_Order(it);
                lst.Add(obj);
            }
            return lst;

        }


        List<BankBranch> Idal.getAllBankBranchs()
        {
            BankBranch_FileLoad();
            return new List<BankBranch>(from it in BankBranchRoot.Elements()
                                   select new BankBranch()
                                   {
                                       BankNumber = it.Element("קוד_בנק").Value,
                                       BankName = it.Element("שם_בנק").Value,
                                       BranchNumber = it.Element("קוד_סניף").Value,
                                       BranchAddress = it.Element("כתובת_ה-ATM").Value,
                                       BranchCity = it.Element("ישוב").Value
                                   });
        }




        List<Host> Idal.getAllHosts()
        {


            HostingUnit_FileLoad();
            List<Host> lst = new List<Host>();
            foreach (var it in HostingUnitRoot.Elements())
            {
                if (!lst.Exists(x => x.HostKey == it.Element("Owner").Element("HostKey").Value))
                    lst.Add(XElementHost_To_Host(it));
            }
            return lst;
        }









      
    }
}
