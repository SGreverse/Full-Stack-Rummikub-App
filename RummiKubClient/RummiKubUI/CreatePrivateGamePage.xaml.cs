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
    /// Interaction logic for CreatePrivateGamePage.xaml
    /// </summary>
    public partial class CreatePrivateGamePage : Page
    {
        public CreatePrivateGamePage()
        {
            InitializeComponent();
        }

        private void Create_New_Game(object sender, RoutedEventArgs e)
        {
            if (MainWindow.IsWaitingForGame)
            {
                MainWindow.This_current_window.ShowErrorBox("Already In Queue For A Different Game");
                return;
            }
            RummiKubServiceClient service = new RummiKubServiceClient();
            var statuscode=service.CreatePrivateGame(MainWindow.CurrentUser.Id);

            if (statuscode.Code != 0)
            {
                MainWindow.This_current_window.ShowErrorBox(statuscode.Message);
                return;
            }
            MainWindow.IsWaitingForGame = true;
            PrivateGamesPage privateGamesPage=ParentConverter.GetParentPage<PrivateGamesPage>(this) as PrivateGamesPage;
            privateGamesPage.AfterPageFrame.Navigate(new PrivateGameQueuePage(statuscode.ReturnedObject,true));
        }
    }
}
