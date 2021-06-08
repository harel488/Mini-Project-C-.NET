using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace DAL
{
    /// <summary>
    /// The role of the class is to make copies of objects 
    /// so as not to use the original Data during a run-time
    /// </summary>
    public static class Cloning
    {
        public static BankBranch Clone(BankBranch original)
        {
            BankBranch target = new BankBranch();
            target.BankName = original.BankName;
            target.BankNumber = original.BankNumber;
            target.BranchAddress = original.BranchAddress;
            target.BranchCity = original.BranchCity;
            target.BranchNumber = original.BranchNumber;
           
            return target;
        }

        public static GuestRequest Clone(GuestRequest original)
        {
            GuestRequest target = new GuestRequest();
            target.Adults = original.Adults;
            target.area = original.area;
            target.Children = original.Children;
            target.ChildrensAttractions = original.ChildrensAttractions;
            target.EntryDate = original.EntryDate;
            target.FamilyName = original.FamilyName;
            target.GuestRequestKey = original.GuestRequestKey;
            target.MailAddress = original.MailAddress;
            target.Pool = original.Pool;
            target.PrivateName = original.PrivateName;
            target.RegistrationDate = original.RegistrationDate;
            target.ReleaseDate = original.ReleaseDate;
            target.Status = original.Status;
            target.Type = original.Type;
            return target;
        }

        public static HostingUnit Clone(HostingUnit original)
        {
            HostingUnit target = new HostingUnit();
            target.Area = original.Area;
            target.Diary = original.Diary;
            target.HostingUnitKey = original.HostingUnitKey;
            target.HostingUnitName = original.HostingUnitName;
            target.Owner = original.Owner;
            target.Type = original.Type;
            target.Price = original.Price;
            return target;
        }

        public static Order Clone(Order original)
        {
            Order target = new Order();
            target.CreateDate = original.CreateDate;
            target.GuestRequestKey = original.GuestRequestKey;
            target.HostingUnitKey = original.HostingUnitKey;
            target.OrderDate = original.OrderDate;
            target.OrderKey = original.OrderKey;
            target.Status = original.Status;
            
            return target;
        }

        

    }
}
