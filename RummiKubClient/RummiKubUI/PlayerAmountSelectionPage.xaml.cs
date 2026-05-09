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
using RummiKubUI.UserControls;

namespace RummiKubUI
{
    /// <summary>
    /// Interaction logic for PlayerAmountSelectionPage.xaml
    /// </summary>
    public partial class PlayerAmountSelectionPage : Page
    {
        public PlayerAmountSelectionPage()
        {
            InitializeComponent();
            HeadBackButtonPanel.Children.Add(new HeadBackButtonUserControl());
        }

        private void EnterQueue_Click(object sender, RoutedEventArgs e)
        {
            RummiKubServiceClient service = new RummiKubServiceClient();

            var selectedItem = PlayerComboBox.SelectedItem as ComboBoxItem;

            if (selectedItem == null)
            {
                MainWindow.This_current_window.ShowErrorBox("Please Select A Player Count");
                return;
            }
            MainWindow.CurrentGameCode = selectedItem.Tag.ToString();

            var statuscode = service.ConnectToGame(MainWindow.CurrentUser.Id, MainWindow.CurrentGameCode);
            if (statuscode.Code != 0)
            {
                MainWindow.This_current_window.ShowErrorBox(statuscode.Message);
                return;
            }
            MainWindow.IsWaitingForGame = true;
            MainWindow.This_current_window.GameFrame.Navigate(new JoiningGamePage());
        }
    }
}
