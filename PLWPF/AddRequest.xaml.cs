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

using BE;
using BL;
namespace PLWPF
{
    /// <summary>
    /// Interaction logic for AddRequest.xaml
    /// </summary>
    
    public partial class AddRequest : Page
    {
        IBL bL = Factory.GetBL();
        GuestRequest newReq;
        bool toUpd = false;
        public AddRequest(string Key,bool isEnable)
        {
            InitializeComponent();
            areaCB.ItemsSource = Enum.GetValues(typeof(Enums.Area));
            typeCB.ItemsSource = Enum.GetValues(typeof(Enums.HostUnitType));
            if (Key == "")
            {
                newReq = new GuestRequest();
                newReq.RegistrationDate = DateTime.Today;
                subTitle.Content = "הוספה";
                Go.Content = "הוסף";
                newReq.EntryDate = DateTime.Today;
                newReq.ReleaseDate = DateTime.Today;
                cancel.Visibility = Visibility.Hidden;
            }
            else
            {
                if (isEnable == true)
                {
                    newReq = BL.BL.getReqByKey(Key);
                    int areaIndex = (int)newReq.area;
                    int typeIndex = (int)newReq.Type;
                    areaCB.SelectedIndex = areaIndex;
                    typeCB.SelectedIndex = typeIndex;
                    subTitle.Content = "עדכון";
                    Go.Content = "עדכן";
                    toUpd = true;
                }
                else
                {
                    cancel.Visibility = Visibility.Hidden;
                    subTitle.Content = "";
                    newReq = BL.BL.getReqByKey(Key);
                    int areaIndex = (int)newReq.area;
                    int typeIndex = (int)newReq.Type;
                    areaCB.SelectedIndex = areaIndex;
                    typeCB.SelectedIndex = typeIndex;
                    this.IsEnabled = false;
                    Go.Visibility = Visibility.Collapsed;
                }

                
            }
            this.DataContext = newReq;
         
        }

        private void Go_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (nameTB.Text != "" && familyTB.Text != "" && mailTB.Text != "" && areaCB.SelectedItem != null &&
                          typeCB.SelectedItem != null && childrensTB.Text != null && adultsTB.Text != null &&
                          entDate.SelectedDate != null && relDate.SelectedDate != null)
                {
                    switch (toUpd)
                    {
                        case true:
                            bL.updateGuestReq(newReq);
                            this.NavigationService.Navigate(new GuestMenu());
                            break;
                        case false:
                            newReq.Status = Enums.GuestReqStatus.פתוחה;
                            bL.addGuestReq(newReq);
                            
                            this.NavigationService.Navigate(new GuestMenu());
                            break;
                        default:
                            break;
                    }
                }
                else
                    throw new ArgumentException("נא למלא את כל השדות");
            }
            catch(ArgumentException a)
            {
                MessageBox.Show(a.Message);
            }
      

        }
        private void ValidationLetters_PreviewTextInput(object sender, TextCompositionEventArgs e)//Enter only Hebrew letters
        {
            Regex regex = new Regex("[^א-ת]+");
            if (regex.IsMatch(e.Text) && e.Text != "\r")
            {
                e.Handled = true;
                MessageBox.Show("נא להכניס אותיות עברית");
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

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
           MessageBoxButton cancel = MessageBoxButton.YesNo;
            MessageBoxResult choose;
            choose = MessageBox.Show("לבטל את הבקשה?", "מחיקה", cancel);
            if(choose==MessageBoxResult.Yes)
            {
                newReq.Status = BE.Enums.GuestReqStatus.סגורה;
                bL.updateGuestReq(newReq);
                this.NavigationService.Navigate(new GuestMenu());
            }
           
        }
    }

   
}
