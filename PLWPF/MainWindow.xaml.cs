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
using System.Threading;

using BE;
using BL;
namespace PLWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            IBL bL = Factory.GetBL();
            InitializeComponent();

            ProgFrame.NavigationService.Navigate(new MainPage());
            //UPDATE the data in the start of program
            (new Thread(() =>
            {
                foreach (var it in bL.getAllGuestReqs())
                {
                    if (BL.BL.isRequestExpired(it)||it.Status==Enums.GuestReqStatus.סגורה)
                        bL.DeleteGuestReq(it);
                }
                foreach (var it in bL.getAllOrders())
                {
                    if (BL.BL.OrderExp(it))
                        bL.DeleteOrder(it);
                    if(BL.BL.isReqClosedByWeb(BL.BL.getReqByKey(it.GuestRequestKey)))
                    {
                        bL.DeleteOrder(it);
                        bL.DeleteGuestReq(BL.BL.getReqByKey(it.GuestRequestKey));
                    }
                }

            }
          )).Start();

        }
    }
}
