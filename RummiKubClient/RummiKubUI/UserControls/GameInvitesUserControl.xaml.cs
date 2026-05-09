using RummiKubUI.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RummiKubUI.UserControls
{
    /// <summary>
    /// Interaction logic for GameInvitedUserControl.xaml
    /// </summary>
    public partial class GameInvitesUserControl : UserControl
    {
        public ObservableCollection<Player> GameInvites { get; set; }

        public GameInvitesUserControl()
        {
            InitializeComponent();
            RummiKubServiceClient service=new RummiKubServiceClient();
            GameInvites = new ObservableCollection<Player>(service.GetAllGameInvites(MainWindow.CurrentUser.Id).ReturnedObject);
            DataContext = this;
        }
        private void ApproveButton_Click(object sender, RoutedEventArgs e)
        {
            HandleButtonClick(sender);
            RummiKubServiceClient service= new RummiKubServiceClient();
            var button = sender as Button;
            Player player=button.DataContext as Player;
            var statuscode = service.InviteResponse(MainWindow.CurrentUser.Id, player.Id, true);
            if (statuscode.Code != 0)
            {
                MainWindow.This_current_window.ShowErrorBox(statuscode.Message);
                return;
            }
        }

        private void DeclineButton_Click(object sender, RoutedEventArgs e)
        {
            HandleButtonClick(sender);
            RummiKubServiceClient service = new RummiKubServiceClient();
            var button = sender as Button;
            Player player = button.DataContext as Player;
            var statuscode=service.InviteResponse(MainWindow.CurrentUser.Id, player.Id, false);
            if(statuscode.Code != 0)
            {
                MainWindow.This_current_window.ShowErrorBox(statuscode.Message);
                return;
            }
        }

        private void HandleButtonClick(object sender)
        {
            var button = sender as Button;
            var container = button?.Parent as Grid;
            var border = container?.Parent as Border;

            if (border != null)
            {
                var inviteToRemove = border.DataContext as Player;

                if (inviteToRemove != null)
                {
                    var storyboard = FindResource("RemoveItemAnimation") as Storyboard;

                    Storyboard.SetTarget(storyboard.Children[0], border); // Height animation
                    Storyboard.SetTarget(storyboard.Children[1], border); // Margin animation
                    Storyboard.SetTarget(storyboard.Children[2], border); // Opacity animation

                    storyboard.Completed += (s, args) => GameInvites.Remove(inviteToRemove);

                    storyboard.Begin();
                }
            }
        }
    }
}
