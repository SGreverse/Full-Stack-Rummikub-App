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
    /// Interaction logic for LeaderBoardPage.xaml
    /// </summary>

    public class LeaderboardItem
    {
        public int Position { get; set; }
        public string Username { get; set; }
        public int Score { get; set; }
        public bool IsCurrentUser { get; set; }
    }
    public partial class LeaderBoardPage : Page
    {
        public LeaderBoardPage()
        {
            InitializeComponent();
            HeadBackButtonPanel.Children.Add(new HeadBackButtonUserControl());
            RummiKubServiceClient service = new RummiKubServiceClient();
            PlayerList players = service.GetLeaderBoard().ReturnedObject;
            LoadLeaderboard(players);
        }
        private void LoadLeaderboard(List<Player> players)
        {
            var topPlayers = players
                .Select((player, index) => new LeaderboardItem
                {
                    Position = index + 1,
                    Username = player.Username,
                    Score = player.Score,
                    IsCurrentUser = player.Id == MainWindow.CurrentUser.Id
                })
                .ToList();

            LeaderboardList.ItemsSource = topPlayers;
        }
    }
}
