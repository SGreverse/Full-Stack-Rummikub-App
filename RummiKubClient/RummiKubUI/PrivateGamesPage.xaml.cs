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
    /// Interaction logic for PrivateGamesPage.xaml
    /// </summary>
    public partial class PrivateGamesPage : Page
    {
        public PrivateGamesPage()
        {
            InitializeComponent();
            RummiKubServiceClient service= new RummiKubServiceClient();
            HeadBackButtonPanel.Children.Add(new HeadBackButtonUserControl());
            JoinPrivateGamePageFrame.Navigate(new JoinPrivateGameOage());
            CreatePrivateGamePageFrame.Navigate(new CreatePrivateGamePage());

            //lstview.ItemsSource = service.GetAllGameInvites(MainWindow.CurrentUser.Id).ReturnedObject;
        }

        //private void AcceptInvite(object sender, RoutedEventArgs e)
        //{
        //    RummiKubServiceClient service = new RummiKubServiceClient();
        //    Player clicked_player = (sender as Button).DataContext as Player;
        //    var statuscode = service.InviteResponse(MainWindow.CurrentUser, clicked_player.Id, true);
        //    if (statuscode.Code != 0)
        //    {
        //        MainWindow mainWindow = ParentConverter.GetParentPage<MainWindow>(this) as MainWindow;
        //        mainWindow.ShowErrorBox(statuscode.Message);
        //    }
        //}
    }
}
