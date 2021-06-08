using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DS;

namespace DAL
{
    public class imp_Dal:Idal
    {

        /// <summary>
        /// Gets the variable according to the desired realization
        /// the "Factory" send to this function
        /// </summary>
        static imp_Dal instance = null;
        public static imp_Dal GetInstance()
        {
            if (instance == null)
                instance = new imp_Dal();
            return instance;
        }
        //==============================================================

        /// <summary>
        /// checking if a a "GuestRequest" is in List
        /// </summary>
        /// <param name="req">the request to check</param>
        /// <returns> a boolean </returns>
        public bool GuestReqIsExiest(GuestRequest req)
        {
            bool found = false;
            foreach (var item in DataSource.GuestRequestList)
            {
                if (item.GuestRequestKey == req.GuestRequestKey)
                    found = true;
            }
            return found;
        }
        /// <summary>
        /// checking if a a "HostingUnit" is in List
        /// </summary>
        /// <param name="unit">the request to check</param>
        /// <returns> a boolean </returns>
        public bool HostUnitExiest(HostingUnit unit)
        {
            bool found = false;
            foreach (var item in DataSource.HostingUnitsList)
            {
                if (item.HostingUnitKey == unit.HostingUnitKey)
                    found = true;
            }
            return found;
        }
        /// <summary>
        /// checking if a a "Order" is in List
        /// </summary>
        /// <param name="order">the request to check</param>
        /// <returns> a boolean </returns>
        public bool orderExiest(Order order)
        {
            foreach (var item in DataSource.OrdersList)
            {
                if (item.OrderKey == order.OrderKey)
                    return true;
            }
            return false;
        }
        //=====================================================================
        //CRUD

        //GuestRequest
        void Idal.AddGuestReq(GuestRequest newRequest)
        {
            newRequest.GuestRequestKey = Configuration.GuestRequestKey.ToString();
            foreach (var Req in DataSource.GuestRequestList)
                {
                
                if (GuestReqIsExiest(newRequest))
                        throw new DuplicateWaitObjectException("Request already exist");
                }
                
                Configuration.GuestRequestKey++;
                DataSource.GuestRequestList.Add(Cloning.Clone(newRequest));
        }
        
         void Idal.updateGuestReq(GuestRequest ReqToUpdate)
        {
            int count = DataSource.GuestRequestList.RemoveAll(item => item.GuestRequestKey == ReqToUpdate.GuestRequestKey);
            if (count == 0)
                throw new KeyNotFoundException("request is not exiest");
            DataSource.GuestRequestList.Add(Cloning.Clone(ReqToUpdate));
        }
        
        void Idal.DeleteGuestReq(GuestRequest ReqToDel)
        {
            int count = DataSource.GuestRequestList.RemoveAll(item => item.GuestRequestKey == ReqToDel.GuestRequestKey);
            if (count == 0)
                throw new KeyNotFoundException("request is not exiest");
        }
        
         GuestRequest Idal.getGuestReq(GuestRequest req)
        {
            if (!GuestReqIsExiest(req))
                throw new KeyNotFoundException("the request is not exiest");
            return Cloning.Clone((from target in DataSource.GuestRequestList
                                  where target.GuestRequestKey == req.GuestRequestKey
                                  select target).FirstOrDefault());
        }
        //-------------------------------------------------------------------------------------------    
        //HostingUnit 
        void Idal.addHostUnit(HostingUnit newUnit)
        {
            if (HostUnitExiest(newUnit))
                throw new DuplicateWaitObjectException("hosting unit already exiest");
            newUnit.HostingUnitKey = Configuration.HostingUnitKey.ToString();
            Configuration.HostingUnitKey++;
            DataSource.HostingUnitsList.Add(Cloning.Clone(newUnit));
        }

        void Idal.updateUnit(HostingUnit UpdUnit)
        {
            int count = DataSource.HostingUnitsList.RemoveAll(item => item.HostingUnitKey == UpdUnit.HostingUnitKey);
            if (count == 0)
                throw new KeyNotFoundException("Hosting Unit is not exiest");
            DataSource.HostingUnitsList.Add(Cloning.Clone(UpdUnit));
        }
        
        void Idal.deleteUnit(HostingUnit unitToDel)
        {
            int count = DataSource.HostingUnitsList.RemoveAll(item => item.HostingUnitKey == unitToDel.HostingUnitKey);
            if (count == 0)
                throw new KeyNotFoundException("מס' יחידה לא קיים");
        }

        HostingUnit Idal.GetHostingUnit(HostingUnit Unit)
        {
            if (!HostUnitExiest(Unit))
                throw new KeyNotFoundException("Hosting Unit is not exiest");
            return Cloning.Clone((from target in DataSource.HostingUnitsList
                                  where target.HostingUnitKey == Unit.HostingUnitKey
                                  select target).FirstOrDefault());
        }
        //------------------------------------------------------------------------------------
      
        void Idal.addOrder(Order newOrder)
        {
            if (orderExiest(newOrder))
                throw new DuplicateWaitObjectException("order already exiest");
            DataSource.OrdersList.Add(Cloning.Clone(newOrder));
        }
        
        void Idal.DeleteOrder(Order orderToDel)
        {
            int count = DataSource.OrdersList.RemoveAll(item => item.OrderKey == orderToDel.OrderKey);
            if (count == 0)
                throw new KeyNotFoundException("order is not exeist");
        }
        
        void Idal.updateOrder(Order orderToUpd)
        {
            int count = DataSource.OrdersList.RemoveAll(item => item.OrderKey == orderToUpd.OrderKey);
            if(count==0)
                throw new KeyNotFoundException("order is not exeist");
            DataSource.OrdersList.Add(Cloning.Clone(orderToUpd));
        }
        
        Order Idal.getOrder(Order order)
        {
            if (!orderExiest(order))
                throw new KeyNotFoundException("order is not exeist");
            return Cloning.Clone((from target in DataSource.OrdersList
                                  where target.OrderKey == order.OrderKey
                                  select target).FirstOrDefault());
        }
        //=============================================================================
       /// <summary>
       /// all the Lists returning functions 
       /// </summary>
       
       
        List<HostingUnit> Idal.getAllUnits()
        {

            return new List<HostingUnit>(DataSource.HostingUnitsList);
        }
        //----------------------------------------------------------------
        List<GuestRequest> Idal.getAllGuestReqs()
        {
            return new List<GuestRequest>(DataSource.GuestRequestList);
        }
        //----------------------------------------------------------------
        List<Order> Idal.getAllOrders()
        {
            return new List<Order>(DataSource.OrdersList);
        }
        //----------------------------------------------------------------
        /// <summary>
        /// Right now we will set up a non-real list of bank branches for review ,
        /// below We will import from the Internet the relevant information
        /// </summary>
        List<BankBranch> Idal.getAllBankBranchs()
        {
            List<BankBranch> lst = new List<BankBranch>()
            {
                new BankBranch()
                {
                    BankNumber="1",BankName="Leumi",BranchNumber="41",BranchAddress="Rakefet 7",BranchCity="jerusalem"
                },
                new BankBranch()
                {
                    BankNumber="10",BankName="Discount",BranchNumber="91",BranchAddress="Ezov 10",BranchCity="Tel-Aviv"
                },
                new BankBranch()
                {
                     BankNumber="5",BankName="Poalim",BranchNumber="12",BranchAddress="Haneviem 70",BranchCity="Jerusalem"
                },
                new BankBranch()
                {
                     BankNumber="2",BankName="Discount",BranchNumber="12",BranchAddress="Goren 90",BranchCity="Haifa"
                },
                new BankBranch()
                {
                     BankNumber="35",BankName="Leumi",BranchNumber="55",BranchAddress="Kalanit 41",BranchCity="Tel-Aviv"
                }

            };
            return lst;
        }
        //--------------------------------------------------------------------------------------------------
        List<Host> Idal.getAllHosts()
        {
            List<Host> lst = new List<Host>();
            foreach (var it in DataSource.HostingUnitsList)
            {
                if (!lst.Exists(x => x.HostKey == it.Owner.HostKey))
                    lst.Add(it.Owner);
            }
            return lst;
        }
        //=======================================================================================================

    }
    
}
