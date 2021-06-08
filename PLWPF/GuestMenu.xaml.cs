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
using BL;
using BE;
namespace PLWPF
{
    /// <summary>
    /// Interaction logic for GuestMenu.xaml
    /// </summary>
    public partial class GuestMenu : Page
    {
        IBL bl = BL.Factory.GetBL();
        bool toUpd = false;
        bool toPrint = false;
        public GuestMenu()
        {
            InitializeComponent();
            keyBoxTB.Visibility = Visibility.Collapsed;
            goUpdate.Visibility = Visibility.Collapsed;
            _label.Visibility = Visibility.Collapsed;
        }

        private void AddReq_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new AddRequest("",true));
        }

        private void UpdReq_Click(object sender, RoutedEventArgs e)
        {
            toUpd = true;
            keyBoxTB.Visibility = Visibility.Visible;
            goUpdate.Visibility = Visibility.Visible;
            _label.Visibility = Visibility.Visible ;
        }
        private void PrintReq_Click(object sender, RoutedEventArgs e)
        {
            toPrint = true;
            keyBoxTB.Visibility = Visibility.Visible;
            goUpdate.Visibility = Visibility.Visible;
            _label.Visibility = Visibility.Visible;
        }

        private void GoUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (BL.BL.getReqByKey(keyBoxTB.Text) == null)
                    throw new ArgumentException("מפתח לא קיים");
                else
                {
                    if (toUpd == true)
                    {
                        toUpd = false;
                        this.NavigationService.Navigate(new AddRequest(keyBoxTB.Text, true));
                        keyBoxTB.Clear();
                        keyBoxTB.Visibility = Visibility.Collapsed;
                        goUpdate.Visibility = Visibility.Collapsed;
                        _label.Visibility = Visibility.Collapsed;

                    }
                    if(toPrint==true)
                    {
                        toPrint = false;
                        this.NavigationService.Navigate(new AddRequest(keyBoxTB.Text, false));
                        keyBoxTB.Clear();
                        keyBoxTB.Visibility = Visibility.Collapsed;
                        goUpdate.Visibility = Visibility.Collapsed;
                        _label.Visibility = Visibility.Collapsed;
                    }
                    
                }
            }
            
            catch(ArgumentException a)
            {
                MessageBox.Show(a.Message);
            }
           
        }

        private void KeyBoxTB_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (keyBoxTB.Text == "הכנס כאן")
                keyBoxTB.Clear();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new MainPage());
        }
    }
}
