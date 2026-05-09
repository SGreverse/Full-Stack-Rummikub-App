using RummiKubUI.ServiceReference1;
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

namespace RummiKubUI
{
    /// <summary>
    /// Interaction logic for JoinPrivateGameOage.xaml
    /// </summary>
    public partial class JoinPrivateGameOage : Page
    {
        public JoinPrivateGameOage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.IsWaitingForGame)
            {
                MainWindow.This_current_window.ShowErrorBox("Already In Queue For A Diifferent Game");
                return;
            }
            if (GameCodeTextBlock.Text.Length !=9)
            {
                MainWindow.This_current_window.ShowErrorBox("Invalid Game Code Format");
                return;
            }
            RummiKubServiceClient service= new RummiKubServiceClient();
            var statuscode = service.ConnectToGame(MainWindow.CurrentUser.Id,GameCodeTextBlock.Text);
            if (statuscode.Code != 0)
            {
                MainWindow.This_current_window.ShowErrorBox(statuscode.Message);
                return;
            }
            MainWindow.IsWaitingForGame = true;
            PrivateGamesPage privateGamesPage = ParentConverter.GetParentPage<PrivateGamesPage>(this) as PrivateGamesPage;
            privateGamesPage.AfterPageFrame.Navigate(new PrivateGameQueuePage(GameCodeTextBlock.Text, false));

        }
    }
}
