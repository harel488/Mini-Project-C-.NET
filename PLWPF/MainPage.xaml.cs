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
using BE;
using BL;
namespace PLWPF
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
       
        public MainPage()
        {
            InitializeComponent();
        }

        private void GoHostMenu_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new HostSignin());

        }

        private void GoGuestMenu_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new GuestMenu());
        }

        private void AdminButton_Click(object sender, RoutedEventArgs e)
        {
            passTB.Visibility = Visibility.Visible;
            GoAdminBUtton.Visibility = Visibility.Visible;
            
        }

      

        private void GoAdminBUtton_Click(object sender, RoutedEventArgs e)
        {
            if (passTB.Password == BE.Configuration.AdminPassword)
                this.NavigationService.Navigate(new HostMenu("", true));
            else
            {
                MessageBox.Show("הקוד שגוי");
            }
        }
    }
}
