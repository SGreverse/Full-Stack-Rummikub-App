using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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
using RummiKubService;
using System.Windows.Threading;
using RummikubBusinessLogic;
namespace RummiServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer dispatcherTimer = new DispatcherTimer(DispatcherPriority.Normal,Dispatcher.CurrentDispatcher);
        public MainWindow()
        {
            
            InitializeComponent();
            ServiceHost host = new ServiceHost(typeof(RummiKubService.RummiKubService1));
            host.Open();
            dispatcherTimer.Interval = TimeSpan.FromSeconds(5);
            dispatcherTimer.Tick += new EventHandler(OnServerHeartBeat);
            dispatcherTimer.Start();
        }
        private void OnServerHeartBeat(object sender, EventArgs e)
        {
            RummiKubLogic.RemoveNotLoadedPlayers();
            RummiKubLogic.RemoveOfflinePlayersFromQueue();
            RummiKubLogic.ClearEmptyChats();
        }
    }
}
