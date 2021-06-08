using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DAL;

namespace BL
{
    public interface IBL
    {
        void addGuestReq(GuestRequest newRequest);
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
       
        //---------------------------------------------
        List<HostingUnit> getAllUnits();
        List<GuestRequest> getAllGuestReqs();
        List<Order> getAllOrders();
        List<Host> getAllHosts();
        List<BankBranch> getAllBankBranchs();
    }
}
