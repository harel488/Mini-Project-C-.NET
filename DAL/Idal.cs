using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace DAL
{
    /// <summary>
    /// The function interface works against the database
    /// </summary>

    public interface Idal  //Add,Delete,Get,Update
    {
         void AddGuestReq(GuestRequest newRequest); 
         void updateGuestReq(GuestRequest ReqToUpdate);
         void DeleteGuestReq(GuestRequest ReqToDel);
         GuestRequest getGuestReq(GuestRequest req);
        //--------------------------------------------
        HostingUnit GetHostingUnit(HostingUnit Unit);
        void addHostUnit(HostingUnit newUnit);
        void updateUnit(HostingUnit UpdUnit);
        void deleteUnit(HostingUnit unitToDel);
        //---------------------------------------------
        void addOrder(Order newOrder);
        void updateOrder(Order orderToUpd);
        void DeleteOrder(Order orderToDel);
        Order getOrder(Order order);
       //----------------------------------------------
        List<HostingUnit> getAllUnits();
        List<GuestRequest> getAllGuestReqs();
        List<Order> getAllOrders();
        List<BankBranch> getAllBankBranchs();
        List<Host> getAllHosts();

       
    }
}
