using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for PrivateGameQueuePage.xaml
    /// </summary>
    public partial class PrivateGameQueuePage : Page
    {

        public ObservableCollection<int> PlayerWaitingCount{ get => MainWindow.PlayerCountInList; set => MainWindow.PlayerCountInList = value; }

        //public ObservableCollection<int> PlayerWaitingCount
        //{
        //    get => _playerWaitingCount;
        //    set
        //    {
        //        _playerWaitingCount = value;
        //        //OnPropertyChanged();
        //    }
        //}
        public PrivateGameQueuePage()
        {
            InitializeComponent();
        }
        public PrivateGameQueuePage(string Code,bool IsPartyLeader) : this()
        {
            MainWindow.CurrentGameCode = Code;
            GameCodeDisplay.Text = $"The Current Game Code:{MainWindow.CurrentGameCode}";
            lstview.ItemsSource = PlayerWaitingCount;
            if (IsPartyLeader)
            {
                StartButton.Visibility = Visibility.Visible;
            }
        }


        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            RummiKubServiceClient service = new RummiKubServiceClient();
            var statuscode = service.StartPrivateGame(MainWindow.CurrentGameCode, MainWindow.CurrentUser.Id);
            if (statuscode.Code != 0)
            {
                MainWindow.This_current_window.ShowErrorBox(statuscode.Message);

            }
        }

    }
}
