using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Mail;
using BE;
using BL;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for HostMenu.xaml
    /// </summary>
    public partial class HostMenu : Page
    {
        IBL bL = Factory.GetBL();
        BE.Configuration config = new Configuration();
        Host ThisHost;
        bool Admin = false;
        public HostMenu(string key, bool isAdmin)
        {
            Admin = isAdmin;
            InitializeComponent();
            DataGB.DataContext = config;
            if (Admin == false)
            {
                InfoTab.Visibility = Visibility.Hidden;

                HostKeyTitle.Text = key;
                ThisHost = BL.BL.getHostByKey(key);

                SortByAreaCB.ItemsSource = Enum.GetValues(typeof(Enums.Area));
                SortByTypeCB.ItemsSource = Enum.GetValues(typeof(Enums.HostUnitType));
                ReqListView.ItemsSource = bL.getAllGuestReqs();

                
                FilterUnitsByAreaCB.Visibility = Visibility.Hidden;
                UnitsListView.ItemsSource = BL.BL.getAllHostUnits(key);

                FilterOrdersByCB.ItemsSource = Enum.GetValues(typeof(Enums.OrderStatus));
                OrderListView.ItemsSource = BL.BL.getAllHostOrders(key);
                

                
                
            }
            else
            {
                HostKeyTitle.Text = "מנהל מערכת";
                AddUnitBT.Visibility = Visibility.Hidden;

                SortByAreaCB.ItemsSource = Enum.GetValues(typeof(Enums.Area));
                SortByTypeCB.ItemsSource = Enum.GetValues(typeof(Enums.HostUnitType));
                ReqListView.ItemsSource = bL.getAllGuestReqs();
                reqTab.Header = "כל הבקשות";

                
                FilterUnitsByAreaCB.ItemsSource = Enum.GetValues(typeof(Enums.Area));
                UnitsTab.Header = "כל היחידות";
                UnitsListView.ItemsSource = bL.getAllUnits();
                
               

                OrderTab.Header = "כל ההזמנות";
                OrderListView.ItemsSource = bL.getAllOrders();
                FilterOrdersByCB.ItemsSource = Enum.GetValues(typeof(Enums.OrderStatus));
                
            }
           
        }

        private void SortByAreaCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IEnumerable<IGrouping<BE.Enums.Area, GuestRequest>> Areas = BL.BL.groups_req_by_area();
            if (SortByAreaCB.SelectedIndex==0)
            {
                foreach(var it in Areas)
                {
                    if(it.Key==BE.Enums.Area.צפון)
                    {
                        List<GuestRequest> lst = it.ToList();
                        ReqListView.ItemsSource = lst;
                    }
                }
            }
            if (SortByAreaCB.SelectedIndex == 1)
            {
                foreach (var it in Areas)
                {
                    if (it.Key == BE.Enums.Area.דרום)
                    {
                        List<GuestRequest> lst = it.ToList();
                        ReqListView.ItemsSource = lst;
                    }
                }
            }
            if (SortByAreaCB.SelectedIndex == 2)
            {
                foreach (var it in Areas)
                {
                    if (it.Key == BE.Enums.Area.מרכז)
                    {
                        List<GuestRequest> lst = it.ToList();
                        ReqListView.ItemsSource = lst;
                    }
                }
            }
            if (SortByAreaCB.SelectedIndex == 3)
            {
                foreach (var it in Areas)
                {
                    if (it.Key == BE.Enums.Area.ירושלים_והסביבה)
                    {
                        List<GuestRequest> lst = it.ToList();
                        ReqListView.ItemsSource = lst;
                    }
                }
            }


        }

        private void SortByTypeCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(SortByTypeCB.SelectedIndex==0)
            {
                ReqListView.ItemsSource= BL.BL.reqByDelegat(it => it.Type == BE.Enums.HostUnitType.מלון).ToList();
            }
            if (SortByTypeCB.SelectedIndex == 1)
            {
                ReqListView.ItemsSource = BL.BL.reqByDelegat(it => it.Type == BE.Enums.HostUnitType.צימר).ToList();
            }
            if (SortByTypeCB.SelectedIndex == 2)
            {
                ReqListView.ItemsSource = BL.BL.reqByDelegat(it => it.Type == BE.Enums.HostUnitType.דירת_אירוח).ToList();
            }
            if (SortByTypeCB.SelectedIndex == 3)
            {
                ReqListView.ItemsSource = BL.BL.reqByDelegat(it => it.Type == BE.Enums.HostUnitType.מאהל).ToList();
            }
        }

        private void AddUnitBT_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new HostRegister(ThisHost.HostKey, false));
        }

        private void ReqListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (Admin == false)
                {
                    MessageBoxButton cancel = MessageBoxButton.YesNo;
                    MessageBoxResult choose;


                    choose = MessageBox.Show("? לעבור לשליחת הזמנה", "שליחת הזמנה", cancel);
                    if (choose == MessageBoxResult.Yes)
                    {
                        object obj = ReqListView.SelectedItem;
                        GuestRequest SelectedReq = obj as GuestRequest;
                        if (SelectedReq.Status == Enums.GuestReqStatus.סגורה || SelectedReq.Status == Enums.GuestReqStatus.נסגרה_דרך_האתר)
                            throw new ArgumentException("ההזמנה סגורה");
                        SendOrder NewSendWind = new SendOrder(HostKeyTitle.Text, SelectedReq.GuestRequestKey);
                        NewSendWind.Show();

                    }
                }
            }
            catch(ArgumentException a)
            {
                MessageBox.Show(a.Message);
            }
           
        }

        private void UnitsClickMenu_Delete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxButton cancel = MessageBoxButton.YesNo;
            MessageBoxResult choose;

             
             choose = MessageBox.Show( "האם אתה בטוח שברצונך למחוק ?", "מחיקה", cancel);
            try
            {
                if (choose == MessageBoxResult.Yes)
                {
                    object obj = UnitsListView.SelectedItem;
                    HostingUnit thisUnit = obj as HostingUnit;
                    bL.deleteUnit(thisUnit);
                    if (Admin == false)
                        UnitsListView.ItemsSource = BL.BL.getAllHostUnits(thisUnit.Owner.HostKey);
                    else
                        UnitsListView.ItemsSource = bL.getAllUnits();
                }
            }
           catch(ArgumentException a)
            {
                MessageBox.Show(a.Message);
            }
            catch(KeyNotFoundException a)
            {
                MessageBox.Show(a.Message);
            }
            
        }

        private void OrderDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxButton cancel = MessageBoxButton.YesNo;
            MessageBoxResult choose;


            choose = MessageBox.Show("האם אתה בטוח שברצונך למחוק ?", "מחיקה", cancel);
            try
            {
                if (choose == MessageBoxResult.Yes)
                {
                    object obj = OrderListView.SelectedItem;
                    Order thisOrder = obj as Order;
                    bL.DeleteOrder(thisOrder);
                    if (Admin == false)
                        OrderListView.ItemsSource = BL.BL.getAllHostOrders(HostKeyTitle.Text);
                    else
                        OrderListView.ItemsSource = bL.getAllOrders();
                }
            }
            catch (ArgumentException a)
            {
                MessageBox.Show(a.Message);
            }
            catch (KeyNotFoundException a)
            {
                MessageBox.Show(a.Message);
            }
        }

        private void ChangeStatus1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Order obj = OrderListView.SelectedItem as Order;
                Order thisOrder = bL.getOrder(obj);
                thisOrder.Status = BE.Enums.OrderStatus.נסגר_בהענות_לקוח;
                bL.updateOrder(thisOrder);
                Configuration.NumClosedOrders++;
                MessageBox.Show("העסקה נסגרה! חשבונך חויב בעמלה");
            }
            catch (FieldAccessException a)
            {
                MessageBox.Show(a.Message);
            }
            catch (ArgumentException a)
            {
                MessageBox.Show(a.Message);
            }
            catch (KeyNotFoundException a)
            {
                MessageBox.Show(a.Message);


            }
        }

        private void ChangeStatus2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Order obj = OrderListView.SelectedItem as Order;
                Order thisOrder = bL.getOrder(obj);
                if (thisOrder.Status == Enums.OrderStatus.נשלח_מייל)
                    throw new ArgumentException("הזמנה כבר נשלחה ללקוח");
                thisOrder.Status = BE.Enums.OrderStatus.נשלח_מייל;
                bL.updateOrder(thisOrder);
                MessageBox.Show("המייל נשלח בהצלחה");
            }
            catch (FieldAccessException a)
            {
                MessageBox.Show(a.Message);
            }
            catch (ArgumentException a)
            {
                MessageBox.Show(a.Message);
            }
            catch (KeyNotFoundException a)
            {
                MessageBox.Show(a.Message);
            }
            catch (SmtpException a)
            {
                MessageBox.Show(a.Message);
            }
            catch (InvalidOperationException a)
            {
                MessageBox.Show(a.Message);
            }
        }

        private void ChangeStatus3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Admin == false)
                    throw new FieldAccessException("אין לך גישה לעדכון זה");
                else
                {
                    Order thisOrder = OrderListView.SelectedItem as Order;
                    thisOrder.Status = BE.Enums.OrderStatus.נסגר_מחוסר_הענות_לקוח;
                    bL.updateOrder(thisOrder);
                }
            }
            catch (FieldAccessException a)
            {
                MessageBox.Show(a.Message);
            }
            catch(ArgumentException a)
            {
                MessageBox.Show(a.Message);
            }
            catch(KeyNotFoundException a)
            {
                MessageBox.Show(a.Message);
            }
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            if (Admin == false)
            {
                ReqListView.ItemsSource = bL.getAllGuestReqs();
                UnitsListView.ItemsSource = BL.BL.getAllHostUnits(HostKeyTitle.Text);
                OrderListView.ItemsSource = BL.BL.getAllHostOrders(HostKeyTitle.Text);
            }
            else
            {
                ReqListView.ItemsSource = bL.getAllGuestReqs();
                UnitsListView.ItemsSource = bL.getAllUnits();
                OrderListView.ItemsSource = bL.getAllOrders();
                DataGB.DataContext = config;
            }
        }

        private void FilterUnitsByAreaCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
                IEnumerable<IGrouping<BE.Enums.Area, HostingUnit>> Areas = BL.BL.group_units_by_area();
                if (FilterUnitsByAreaCB.SelectedIndex == 0)
                {
                    foreach (var it in Areas)
                    {
                        if (it.Key == BE.Enums.Area.צפון)
                        {
                            List<HostingUnit> lst = it.ToList();
                            UnitsListView.ItemsSource = lst;
                        }
                    }
                }
                if (FilterUnitsByAreaCB.SelectedIndex == 1)
                {
                    foreach (var it in Areas)
                    {
                        if (it.Key == BE.Enums.Area.דרום)
                        {
                            List<HostingUnit> lst = it.ToList();
                            UnitsListView.ItemsSource = lst;
                        }
                    }
                }
                if (FilterUnitsByAreaCB.SelectedIndex == 2)
                {
                    foreach (var it in Areas)
                    {
                        if (it.Key == BE.Enums.Area.מרכז)
                        {
                            List<HostingUnit> lst = it.ToList();
                            UnitsListView.ItemsSource = lst;
                        }
                    }
                }
                if (FilterUnitsByAreaCB.SelectedIndex == 3)
                {
                    foreach (var it in Areas)
                    {
                        if (it.Key == BE.Enums.Area.ירושלים_והסביבה)
                        {
                            List<HostingUnit> lst = it.ToList();
                            UnitsListView.ItemsSource = lst;
                        }
                    }
                }
            
         
        }

        private void FilterOrdersByCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IEnumerable<IGrouping<BE.Enums.OrderStatus, Order>> Grp;
            if (Admin==true)
            {
               Grp = BL.BL.group_order_by_status(bL.getAllOrders());
            }
            else
            {
               Grp = BL.BL.group_order_by_status(BL.BL.getAllHostOrders(HostKeyTitle.Text));
            }

            if (FilterOrdersByCB.SelectedIndex == 0)
            {
                foreach (var it in Grp )
                {
                    if (it.Key == BE.Enums.OrderStatus.נסגר_מחוסר_הענות_לקוח)
                    {
                        List<Order> lst = it.ToList();
                        OrderListView.ItemsSource = lst;
                    }
                }
            }
            if (FilterOrdersByCB.SelectedIndex == 1)
            {
                foreach (var it in Grp)
                {
                    if (it.Key == BE.Enums.OrderStatus.נסגר_בהענות_לקוח)
                    {
                        List<Order> lst = it.ToList();
                        OrderListView.ItemsSource = lst;
                    }
                }
            }

            if (FilterOrdersByCB.SelectedIndex == 2)
            {
                foreach (var it in Grp)
                {
                    if (it.Key == BE.Enums.OrderStatus.טרם_טופל)
                    {
                        List<Order> lst = it.ToList();
                        OrderListView.ItemsSource = lst;
                    }
                }
            }
            if (FilterOrdersByCB.SelectedIndex == 3)
            {
                foreach (var it in Grp)
                {
                    if (it.Key == BE.Enums.OrderStatus.נשלח_מייל)
                    {
                        List<Order> lst = it.ToList();
                        OrderListView.ItemsSource = lst;
                    }
                }
            }
        }

       

        private void Setting_Click(object sender, RoutedEventArgs e)
        {
            SettingWindow Set = new SettingWindow();
            Set.Show();
        }

        private void BackBt_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new MainPage());
        }

        private void UnitDetails_Click(object sender, RoutedEventArgs e)
        {
            object obj = UnitsListView.SelectedItem;
            HostingUnit unit = obj as HostingUnit;
            UnitCalender newWIN = new UnitCalender(unit.HostingUnitKey);
            newWIN.Show();
        }

        private void Clearance_Click(object sender, RoutedEventArgs e)
        {

          
                HostingUnit obj= bL.GetHostingUnit(UnitsListView.SelectedItem as HostingUnit);
                obj.Owner.CollectionClearance = true;
                bL.updateUnit(obj);
        }
    }


}
