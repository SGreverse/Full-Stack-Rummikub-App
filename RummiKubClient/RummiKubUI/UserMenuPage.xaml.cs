using RummiKubUI.ServiceReference1;
using RummiKubUI.UserControls;
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
    /// Interaction logic for UserMenuPage.xaml
    /// </summary>
    public partial class UserMenuPage : Page
    {
        StatisticsPage statisticsPage;
         GameHistoryPage gameHistoryPage;
        PlayerInfoPage playerInfoPage;
        Player Viewed_Player;
        public UserMenuPage()
        {
            InitializeComponent();
            this.HeadBackButtonPanel.Children.Add(new HeadBackButtonUserControl());

        }
        public UserMenuPage(Player player) : this()
        {
            Viewed_Player=player;
             this.statisticsPage = new StatisticsPage(player);
             this.gameHistoryPage = new GameHistoryPage(player);
            this.playerInfoPage = new PlayerInfoPage(player);
        }
        private void StatisticsButton_Checked(object sender, RoutedEventArgs e)
        {
            LoadStatisticsContent();
            GameHistoryButton.IsChecked = false;
            UserInfoButton.IsChecked = false;
            UpdateCurrentSelectionLabel("Statistics");
        }

        private void StatisticsButton_Unchecked(object sender, RoutedEventArgs e)
        {
            // Prevent unchecking if the other button is not checked
            if (!GameHistoryButton.IsChecked == true && !UserInfoButton.IsChecked==true)
            {
                StatisticsButton.IsChecked = true;
            }
        }

        private void GameHistoryButton_Checked(object sender, RoutedEventArgs e)
        {
            LoadGameHistoryContent();
            StatisticsButton.IsChecked = false;
            UserInfoButton.IsChecked = false;
            UpdateCurrentSelectionLabel("Game History");
        }

        private void GameHistoryButton_Unchecked(object sender, RoutedEventArgs e)
        {
            // Prevent unchecking if the other button is not checked
            if (!StatisticsButton.IsChecked == true && !UserInfoButton.IsChecked==true)
            {
                GameHistoryButton.IsChecked = true;
            }
        }
        private void UserInfoButton_Checked(object sender, RoutedEventArgs e)
        {
            LoadUserInfoContent();
            GameHistoryButton.IsChecked = false;
            StatisticsButton.IsChecked = false;
            UpdateCurrentSelectionLabel("User Info ");
        }

        private void UserInfoButton_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!StatisticsButton.IsChecked == true && !GameHistoryButton.IsChecked == true)
            {
                UserInfoButton.IsChecked = true;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Handle back button click (e.g., navigate to main menu)
            MainWindow.This_current_window.GameFrame.NavigationService.GoBack();
        }

        private void LoadStatisticsContent()
        {
            ContentFrame.Navigate(statisticsPage);
        }

        private void LoadGameHistoryContent()
        {
            ContentFrame.Navigate (gameHistoryPage);
        }
        private void LoadUserInfoContent()
        {
            ContentFrame.Navigate(playerInfoPage);
        }
        private void UpdateCurrentSelectionLabel(string selection)
        {
            CurrentSelectionLabel.Text = selection;
        }


    }
}
