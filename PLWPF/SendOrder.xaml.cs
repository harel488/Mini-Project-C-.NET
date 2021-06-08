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
using System.Windows.Shapes;
using System.Net.Mail;
using BE;
using BL;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for SendOrder.xaml
    /// </summary>
    public partial class SendOrder : Window
    {
        IBL bl = BL.Factory.GetBL();
        Order NewOrder;
        public SendOrder(string Hostkey,string ReqKey)
        {
            InitializeComponent();
            
                NewOrder = new Order();
                HostUnitsCB.ItemsSource = BL.BL.getAllHostUnits(Hostkey);
                ReqKeyTB.Text = BL.BL.getReqByKey(ReqKey).GuestRequestKey;
                OrderKeyTB.Text = BE.Configuration.OrderKey.ToString();
                StatusCB.ItemsSource= Enum.GetValues(typeof(Enums.OrderStatus));
                StatusCB.SelectedItem = BE.Enums.OrderStatus.טרם_טופל;
                StatusCB.IsEnabled = false;
                CreateDatePicker.SelectedDate = DateTime.Today;
                CreateDatePicker.IsEnabled = false;
                MailDatePicker.SelectedDate = DateTime.Today;
                MailDatePicker.IsEnabled = false;
                OrderData.DataContext = NewOrder;
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (HostUnitsCB.SelectedItem == null)
                    throw new ArgumentException("אנא בחר יחידת אירוח להזמנה");
                NewOrder.HostingUnitKey = BL.BL.getUnitKeyByUnitName(HostUnitsCB.Text);
                bl.addOrder(NewOrder);
                NewOrder.Status = BE.Enums.OrderStatus.נשלח_מייל;
                bl.updateOrder(NewOrder);
                MessageBox.Show("המייל נשלח בהצלחה");
                this.Close();
               
            }
            catch(ArgumentException a)
            {
                MessageBox.Show(a.Message);
                this.Close();
            }
            catch(KeyNotFoundException a)
            {
                MessageBox.Show(a.Message);
                this.Close();
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
       
    }
}
