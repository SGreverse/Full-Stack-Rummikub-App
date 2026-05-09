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
using RummiKubUI.ServiceReference1;

namespace RummiKubUI
{
    /// <summary>
    /// Interaction logic for GameHistoryPage.xaml
    /// </summary>
    public partial class GameHistoryPage : Page
    {
        public GameHistoryPage()
        {
            InitializeComponent();
        }
        public GameHistoryPage(Player player) : this()
        {
            RummiKubServiceClient service = new RummiKubServiceClient();
            GameWinnerOrderList lst = service.GetAllGamesByUser(player.Id).ReturnedObject;
            lst.Reverse();
            lstview.ItemsSource = lst;
        }
    }
}
