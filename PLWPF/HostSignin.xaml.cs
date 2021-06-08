using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using System.Drawing;
using Microsoft.Win32;
using BL;
namespace PLWPF
{
    /// <summary>
    /// Interaction logic for HostSignin.xaml
    /// </summary>
    public partial class HostSignin : Page
    {
        IBL bl = BL.Factory.GetBL();
        public HostSignin()
        {
            InitializeComponent();
        }

   

        private void NewHostButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new HostRegister("", true));
        }

        private void HostTB_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            HostTB.Clear();
        }

        private void GoButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               if (BL.BL.HostExist(HostTB.Text))
                    this.NavigationService.Navigate(new HostMenu(HostTB.Text,false));
                else throw new ArgumentException("מספר מארח לא קיים במערכת");
            }
            catch(ArgumentException a)
            {
                MessageBox.Show(a.Message);
            }


        }
        private void ValidationNumbers_PreviewTextInput(object sender, TextCompositionEventArgs e)//Enter only numbers
        {
            Regex regex = new Regex("[^0-9]+");
            if (regex.IsMatch(e.Text) && e.Text != "\r")
            {
                e.Handled = true;
                MessageBox.Show("נא להכניס מספרים");
            }
        }
    }
}
