using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS
{
    public class DataSource
    {
        //reques DATA (on List)
        //===============================================================================================================
        public static List<BE.GuestRequest> GuestRequestList = new List<BE.GuestRequest>()
        {
                 new BE.GuestRequest()
            {
                PrivateName="אבי",FamilyName="שלום",MailAddress="avi112@gmail.com",Status=BE.Enums.GuestReqStatus.פתוחה,
                RegistrationDate=new DateTime(2020,01,14),EntryDate=new DateTime(2019,01,01),
                ReleaseDate=new DateTime(2019,01,03),area=BE.Enums.Area.ירושלים_והסביבה,Type=BE.Enums.HostUnitType.מלון,
                Adults=2,Children=0,Pool=true,ChildrensAttractions=false,GuestRequestKey="10000001"
            },

                 new BE.GuestRequest()
            {
                PrivateName="איתמר",FamilyName="צור",MailAddress="itamr112@gmail.com",Status=BE.Enums.GuestReqStatus.סגורה,
                RegistrationDate=new DateTime(2019,04,14),EntryDate=new DateTime(2019,05,10),
                ReleaseDate=new DateTime(2019,05,16),area=BE.Enums.Area.דרום,Type=BE.Enums.HostUnitType.צימר,
                Adults=2,Children=2,Pool=true,ChildrensAttractions=true,GuestRequestKey="10000002"
            },

                new BE.GuestRequest()
            {
                PrivateName="יוסי",FamilyName="כהן",MailAddress="yossi@gmail.com",Status=BE.Enums.GuestReqStatus.נסגרה_דרך_האתר,
                RegistrationDate=new DateTime(2019,07,01),EntryDate=new DateTime(2019,08,01),
                ReleaseDate=new DateTime(2019,08,10),area=BE.Enums.Area.מרכז,Type=BE.Enums.HostUnitType.דירת_אירוח,
                Adults=2,Children=4,Pool=false,ChildrensAttractions=true,GuestRequestKey="10000003"
            },
                     new BE.GuestRequest()
            {
                PrivateName="חנן",FamilyName="לוי",MailAddress="levy444@gmail.com",Status=BE.Enums.GuestReqStatus.פתוחה,
                RegistrationDate=new DateTime(2019,06,29),EntryDate=new DateTime(2019,07,15),
                ReleaseDate=new DateTime(2019,07,22),area=BE.Enums.Area.צפון,Type=BE.Enums.HostUnitType.צימר,
                Adults=1,Children=5,Pool=true,ChildrensAttractions=true,GuestRequestKey="10000004"
            },
                          new BE.GuestRequest()
            {
                PrivateName="מתן",FamilyName="חיים",MailAddress="matanHaim@gmail.com",Status=BE.Enums.GuestReqStatus.סגורה,
                RegistrationDate=new DateTime(2019,09,01),EntryDate=new DateTime(2019,09,07),
                ReleaseDate=new DateTime(2019,09,14),area=BE.Enums.Area.ירושלים_והסביבה,Type=BE.Enums.HostUnitType.מלון,
                Adults=2,Children=0,Pool=true,ChildrensAttractions=false,GuestRequestKey="10000005"
            },
        };

        //===============================================================================================================
        //Units DATA (on List)

        public static List<BE.HostingUnit> HostingUnitsList = new List<BE.HostingUnit>()
        {

            new BE.HostingUnit()
          {
              Owner=new BE.Host()
              {
                  HostBankAccuont=new BE.BankBranch()
                  {
                      BankNumber="1",BankName="לאומי",BranchNumber="41",BranchAddress="הנביאים 5",BranchCity="ירושלים",
                  },
                  HostKey="311225551", PrivateName="הראל", FamilyName="יששכר",PhoneNumber="0509744180",
                  MailAddress="harel488@gmail.com",CollectionClearance=true,BankAccountNumber="13113",
              },
              HostingUnitName="מלונות הראל",Type=BE.Enums.HostUnitType.מלון,HostingUnitKey="10000001",Price="400",Area=BE.Enums.Area.ירושלים_והסביבה
          },


            new BE.HostingUnit()
          {
              Owner=new BE.Host()
              {
                  HostBankAccuont=new BE.BankBranch()
                  {
                     BankNumber= "10",BankName="פועלים",BranchNumber="91",BranchAddress="שטראוס 20",BranchCity="תל אביב"
                  },
                  HostKey="325698411", PrivateName="מוטי", FamilyName="שוקרון",PhoneNumber="0501111111",
                  MailAddress="moti488@gmail.com",CollectionClearance=false,BankAccountNumber="5616561",
              },
              HostingUnitName="מוטי צימרים ",Type=BE.Enums.HostUnitType.צימר,HostingUnitKey="10000002",Price="2100",Area=BE.Enums.Area.צפון
          },

            new BE.HostingUnit()
          {
              Owner=new BE.Host()
              {
                  HostBankAccuont=new BE.BankBranch()
                  {
                      BankNumber="10",BankName="דיסקונט",BranchNumber="91",BranchAddress="רקפת 7",BranchCity="רעננה",
                  },
                  HostKey="311245551", PrivateName="נועם", FamilyName="אלון",PhoneNumber="0505764410",BankAccountNumber="1516",
                  MailAddress="Noam8@gmail.com",CollectionClearance=false
              },
              HostingUnitName="החופשה הטובה",Type=BE.Enums.HostUnitType.מלון, HostingUnitKey="10000003",Price="100",Area=BE.Enums.Area.ירושלים_והסביבה
          },

            new BE.HostingUnit()
          {
              Owner=new BE.Host()
              {
                  HostBankAccuont=new BE.BankBranch()
                  {
                      BankNumber="1",BankName="לאומי",BranchNumber="14",BranchAddress="גדעון 71",BranchCity="חולון",
                  },
                  HostKey="311225551", PrivateName="אביגדור", FamilyName="ליברמן",PhoneNumber="0509999990",
                  MailAddress="liberman000@gmail.com",CollectionClearance=true,BankAccountNumber="151364"
              },
              HostingUnitName="המאהל המושלם",Type=BE.Enums.HostUnitType.צימר, HostingUnitKey="10000004",Price="300",Area=BE.Enums.Area.צפון
          },

        };

        //================================================================================================================
        // Orders DATA (on List)


        public static List<BE.Order> OrdersList = new List<BE.Order>()
        {
            new BE.Order()
            {
                HostingUnitKey=HostingUnitsList[0].HostingUnitKey ,GuestRequestKey=GuestRequestList[0].GuestRequestKey,
                OrderKey="1",Status=BE.Enums.OrderStatus.טרם_טופל,CreateDate=new DateTime(2019,1,2),OrderDate=new DateTime(2019,1,1)

            },
            new BE.Order()
            {
                HostingUnitKey=HostingUnitsList[1].HostingUnitKey,GuestRequestKey=GuestRequestList[1].GuestRequestKey,
                OrderKey="2",Status=BE.Enums.OrderStatus.נשלח_מייל,CreateDate=new DateTime(2019,2,2),OrderDate=new DateTime(2019,2,1)
            }
        };


    }







}
