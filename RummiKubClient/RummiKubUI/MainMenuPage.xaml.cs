using RummiKubUI.ServiceReference1;
using RummiKubUI.UserControls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;
using System.IO;
namespace RummiKubUI
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenuPage : Page
    {
        public RummiKubServiceClient service = new RummiKubServiceClient();

        public Player player;
        public MainMenuPage()
        {
            InitializeComponent();
            UserDisplayPanel.Children.Add(new PlayerDisplayUserContorl(MainWindow.CurrentUser));
            if(MainWindow.IsLoggedIn)
            {
                this.UserInfoButton.IsEnabled = true;
                this.UserInfoButton.Visibility = Visibility.Visible;
                this.LoginButton.Visibility = Visibility.Collapsed;
                this.RegisterButton.Visibility = Visibility.Collapsed;
                this.LogOutButton.Visibility = Visibility.Visible;
            }
            else
            {
                this.UserInfoButton.IsEnabled = false;
                this.UserInfoButton.Visibility = Visibility.Hidden;
                this.LoginButton.Visibility = Visibility.Visible;
                this.RegisterButton.Visibility = Visibility.Visible;
                this.LogOutButton.Visibility = Visibility.Collapsed;

            }

        }
        private void JoinGame(object sender, RoutedEventArgs e)
        {
            ConnectToGame();
        }

        private void HeadToLoginPage(object sender, RoutedEventArgs e)
        {
            MainWindow.This_current_window.GameFrame.Navigate(new LoginPage());
        }

        public void HeadBack()
        {
            NavigationService.GoBack();
        }

        private void HeadToUserInfo(object sender, RoutedEventArgs e)
        {
            if (!MainWindow.IsLoggedIn)
            {
                MainWindow.This_current_window.ShowErrorBox("Invalid Command");
                return;
            }
            MainWindow.This_current_window.GameFrame.Navigate(new UserMenuPage(MainWindow.CurrentUser));
        }

        private void HeadToRegister(object sender, RoutedEventArgs e)
        {
            MainWindow.This_current_window.GameFrame.Navigate(new RegisterPage());
        }

        private void HeadToFriendPage(object sender, RoutedEventArgs e)
        {
            if (!MainWindow.IsLoggedIn)
            {
                MainWindow.This_current_window.ShowErrorBox("Cannot View Friends Until You Are Logged In");
                return;
            }
            MainWindow.This_current_window.GameFrame.Navigate(new FriendListpage());
        }
        public void ConnectToGame()
        {
            if (!MainWindow.IsLoggedIn)
            {
                MainWindow.This_current_window.ShowErrorBox("Cannot Join A Game Until You Are Logged In");
                return;
            }
            MainWindow.This_current_window.GameFrame.Navigate(new PlayerAmountSelectionPage());



        }

        private void HeadToPrivateGamePage(object sender, RoutedEventArgs e)
        {
            if (!MainWindow.IsLoggedIn)
            {
                MainWindow.This_current_window.ShowErrorBox("Cannot Join A Game Until You Are Logged In");
                return;
            }
            MainWindow.This_current_window.GameFrame.Navigate(new PrivateGamesPage());
        }

        private void OpenRuleBookInBrowser(object sender, RoutedEventArgs e)//rule book page
        {
            string relativePath = Path.Combine("..", "..", "..", "RummiKubWebSite", "RummikubManual.html");
            string fullPath = Path.GetFullPath(relativePath);

            if (File.Exists(fullPath))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = fullPath,
                    UseShellExecute = true
                });
            }
            else
            {
                MainWindow.This_current_window.ShowErrorBox("Manual not found at: " + fullPath);
            }


        }

        private void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            RummiKubServiceClient service=new RummiKubServiceClient();
            var statuscode = service.Logout(MainWindow.CurrentUser.Id);
            if (statuscode.Code == 0)
            {
                MainWindow.CurrentUser = null;

                MainWindow.This_current_window.GameFrame.Navigate(new MainMenuPage());
            }

        }

        private void HeadToLeaderBoard(object sender, RoutedEventArgs e)
        {
            if (!MainWindow.IsLoggedIn)
            {
                MainWindow.This_current_window.ShowErrorBox("Cannot View The LeaderBoard Until You Are Logged in");
                return;
            }
            MainWindow.This_current_window.GameFrame.Navigate(new LeaderBoardPage());
        }

        private void HeadToContact_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.This_current_window.GameFrame.Navigate(new ContactUsPage());
        }
    }
}
