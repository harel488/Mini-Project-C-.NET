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
using BE;
using BL;

namespace PLWPF
{
    /// <summary>
    /// Interaction logic for UnitCalender.xaml
    /// </summary>
    public partial class UnitCalender : Window
    {
        IBL bL = Factory.GetBL();
        public UnitCalender(string key)
        {
            InitializeComponent();
            HostingUnit thisUnit= BL.BL.getUnitByKey(key);
            this.DataContext = thisUnit;
            for (int i=0;i<12;i++)
                for(int j=0;j<31;j++)
                {
                    
                    if (thisUnit.Diary[i,j]==true)
                    {
                        unitCalender.BlackoutDates.Add(new CalendarDateRange(new DateTime(DateTime.Today.Year,i+1,j+1)));
                    }
                }
        }

        private void Finish_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
