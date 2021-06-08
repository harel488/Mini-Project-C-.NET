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
    /// Interaction logic for HostRegister.xaml
    /// </summary>
    public partial class HostRegister : Page
    {
        IBL bl = BL.Factory.GetBL();
        HostingUnit newUnit;
        public HostRegister(string Key, bool isEnable)
        {
            InitializeComponent();
           
            areaCB.ItemsSource = Enum.GetValues(typeof(Enums.Area));
            typeCB.ItemsSource = Enum.GetValues(typeof(Enums.HostUnitType));
            newUnit = new HostingUnit();
            newUnit.Owner = new Host();
            newUnit.Owner.HostBankAccuont = new BankBranch();
            
            if (isEnable == true)
            {
                hostDetails.DataContext = newUnit.Owner;
                UnitDetails.DataContext = newUnit;
                BankDetails.DataContext = newUnit.Owner.HostBankAccuont;
                finish.Content = "סיים";
            }
            else
            {
             
               
                
                    finish.Content = "הוסף יחידה";
                    newUnit.Owner = BL.BL.getHostByKey(Key);
                    hostDetails.DataContext = newUnit.Owner;
                    BankDetails.DataContext = newUnit.Owner.HostBankAccuont;
                    UnitDetails.DataContext = newUnit;
                    hostDetails.IsEnabled = false;
                    BankDetails.IsEnabled = false;
                
                
                
            }
        }

        private void Finish_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                if (keyTB.Text != "" && nameTB.Text != "" && familyTB.Text != "" && phoneTB.Text != "" && mailTB.Text != "" && priceTB.Text!=""&&
                    numAcountTB.Text != "" && numBankTB.Text != "" && numBranchTB.Text != "" && nameUnitTB.Text != "" && typeCB.SelectedItem != null && areaCB.SelectedItem != null)
                {
                  if(BL.BL.BankBranchExist(numBankTB.Text,numBranchTB.Text)==false)
                         throw new KeyNotFoundException("פרטי הבנק לא נמצאו במערכת");
                    BankBranch thisBank=bl.getAllBankBranchs().FirstOrDefault(it=>it.BankNumber==numBankTB.Text&&it.BranchNumber==numBranchTB.Text);
                    newUnit.Owner.HostBankAccuont.BankName = thisBank.BankName;
                    newUnit.Owner.HostBankAccuont.BranchAddress = thisBank.BranchAddress;
                    newUnit.Owner.HostBankAccuont.BranchCity = thisBank.BranchCity;
                    bl.addHostUnit(newUnit);
                   
                   
                    this.NavigationService.Navigate(new HostMenu(newUnit.Owner.HostKey,false));
                }
                else
                    throw new ArgumentException("אנא מלא את כל השדות");
            }
            catch (ArgumentException a)
            {
                MessageBox.Show(a.Message);
            }
            catch(KeyNotFoundException a)
            {
                MessageBox.Show(a.Message);
            }
        }


        private void ValidationLetters_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^א-ת]+");
            if (regex.IsMatch(e.Text) && e.Text != "\r")
            {
                e.Handled = true;
                MessageBox.Show("נא להכניס אותיות עברית");
            }
        }



        private void ValidationNumbers_PreviewTextInput(object sender, TextCompositionEventArgs e)
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

