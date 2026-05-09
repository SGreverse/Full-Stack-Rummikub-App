using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for JoiningGamePage.xaml
    /// </summary>
    public partial class JoiningGamePage : Page
    {
        public ObservableCollection<int> PlayerWaitingCount { get => MainWindow.PlayerCountInList; set => MainWindow.PlayerCountInList = value; }

        public JoiningGamePage()
        {
            InitializeComponent();
            lstview.ItemsSource = PlayerWaitingCount;

        }

        private void ExitQueue(object sender, RoutedEventArgs e)
        {
            RummiKubServiceClient service=new RummiKubServiceClient();
            var statuscode=service.ExitWaitingQueue(MainWindow.CurrentUser.Id,MainWindow.CurrentGameCode);
            if(statuscode.Code == 0)
            {
                MainWindow.IsWaitingForGame = false;
                MainWindow.This_current_window.dispatcherTimer.Stop();
                MainWindow.This_current_window.GameFrame.Navigate(new MainMenuPage());
            }
            else
            {
                MainWindow.This_current_window.ShowErrorBox(statuscode.Message);
            }
        }
    }
}
