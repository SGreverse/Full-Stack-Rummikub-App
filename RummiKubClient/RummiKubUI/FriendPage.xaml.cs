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
using RummiKubUI.UserControls;
using RummiKubUI.UtilClasses;

namespace RummiKubUI
{
    /// <summary>
    /// Interaction logic for FriendPage.xaml
    /// </summary>

    public abstract partial class FriendPage : Page
    {
        public Player Viewed_Player;

        protected PlayerList Base_List { get; set; }

        public FriendPage()
        {
            InitializeComponent();
            FriendsListUtil lst = GetList();
            lstview.ItemsSource = lst;
            HeadBackButtonPanel.Children.Add(new HeadBackButtonUserControl());
        }
        public abstract FriendsListUtil GetList();
        public abstract void ButtonFunctionality(Button btn);
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            ButtonFunctionality(btn);
        }
        protected void ForceRefresh()
        {
            FriendsListUtil lst = GetList();
            lstview.ItemsSource = lst;
        }
        private void ItemBorder_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border clickedItem)
            {
                var window = Window.GetWindow(this);
                if (window == null) return;

                // Get the clicked item’s position relative to the screen
                Point screenPoint = clickedItem.PointToScreen(new Point(clickedItem.ActualWidth, clickedItem.ActualHeight / 2));


                // Get the window’s screen position (including title bar and borders)
                Point windowScreenPos = window.PointToScreen(new Point(0, 0));

                // Convert to window coordinates by subtracting the window's screen position
                double relativeX = screenPoint.X;
                double relativeY = screenPoint.Y;



                // Default position (popup to the right)
                double popupX = relativeX + 10;  // Offset from item
                double popupY = relativeY;

                // Apply popup position
                ContextMenuPopup.HorizontalOffset = popupX;
                ContextMenuPopup.VerticalOffset = popupY;

                // Show popup
                ContextMenuPopup.IsOpen = true;
                Friend friend = clickedItem.DataContext as Friend;
                RummiKubServiceClient service = new RummiKubServiceClient();
                Viewed_Player = service.GetPlayer(friend.PlayerId).ReturnedObject;
            }
        }
        private void UsernameFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            ForceRefresh();
        }

        private void InviteToGame_Click(object sender, RoutedEventArgs e)
        {
            RummiKubServiceClient service = new RummiKubServiceClient();
            var statuscode = service.InviteToGame(MainWindow.CurrentUser.Id, Viewed_Player.Id);
            if (statuscode.Code != 0)
            {
                MainWindow.This_current_window.ShowErrorBox(statuscode.Message);
            }
            else
            {
                MainWindow.This_current_window.ShowErrorBox("Invite Sent", true);
            }

        }

        private void ViewProfile_Click(object sender, RoutedEventArgs e)
        {

            MainWindow window = ParentConverter.GetParentPage<MainWindow>(this) as MainWindow;
            window.GameFrame.Navigate(new UserMenuPage(Viewed_Player));
        }
        private void AddFriendButton_Checked(object sender, RoutedEventArgs e)
        {
            LoadAddFriendPage();
            FriendsButton.IsChecked = false;
            FriendRequestButton.IsChecked = false;
        }

        private void AddFriendButton_Unchecked(object sender, RoutedEventArgs e)
        {
            if(FriendRequestButton.IsChecked==false  && FriendsButton.IsChecked==false)
            {
               AddFriendButton.IsChecked = true;
            }
        }

        private void FriendsButton_Unchecked(object sender, RoutedEventArgs e)
        {
            if (FriendRequestButton.IsChecked == false && AddFriendButton.IsChecked == false)
            {
                FriendsButton.IsChecked = true;
            }
        }

        private void FriendsButton_Checked(object sender, RoutedEventArgs e)
        {
            LoadFriendsPage();
            AddFriendButton.IsChecked = false;
            FriendRequestButton.IsChecked = false;
        }

        private void FriendRequestButton_Checked(object sender, RoutedEventArgs e)
        {
            LoadRequestsFriendsPage();
            AddFriendButton.IsChecked = false;
            FriendsButton.IsChecked= false;
        }

        private void FriendRequestButton_Unchecked(object sender, RoutedEventArgs e)
        {
            if (FriendsButton.IsChecked == false && AddFriendButton.IsChecked == false)
            {
                FriendRequestButton.IsChecked = true;
            }
        }
        private void LoadAddFriendPage()
        {
            MainWindow.This_current_window.GameFrame.Navigate(new AddFriendPage());
        }
        private void LoadFriendsPage()
        {
            MainWindow.This_current_window.GameFrame.Navigate(new FriendListpage());

        }
        private void LoadRequestsFriendsPage()
        {
            MainWindow.This_current_window.GameFrame.Navigate(new FriendRequestsPage());

        }
    }
}
