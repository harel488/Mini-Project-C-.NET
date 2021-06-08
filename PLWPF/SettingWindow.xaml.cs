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
using System.ComponentModel;
using System.Windows.Shapes;
using BE;
using BL;
namespace PLWPF
{
    /// <summary>
    /// Interaction logic for SettingWindow.xaml
    /// </summary>
    public partial class SettingWindow : Window
    {
        Configuration config = new Configuration();
        public SettingWindow()
        {
            InitializeComponent();
            AdminPasswordTB.IsEnabled = false;
            commissionTB.IsEnabled = false;
            RequestExpiredTB.IsEnabled = false;
            SettingData.DataContext = config;
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            AdminPasswordTB.IsEnabled = true;
            commissionTB.IsEnabled = true;
            RequestExpiredTB.IsEnabled = true;
            OrderExpiredTB.IsEnabled = true;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            BE.Configuration.AdminPassword = AdminPasswordTB.Text;
            BE.Configuration.commission = int.Parse(commissionTB.Text);
            BE.Configuration.RequestExpired = int.Parse(RequestExpiredTB.Text);
            BE.Configuration.OrderExpired = int.Parse(OrderExpiredTB.Text);
            BL.BL.ChangeConfig();
            MessageBox.Show("השינויים נשמרו");
            this.Close();
        }
    }
}
