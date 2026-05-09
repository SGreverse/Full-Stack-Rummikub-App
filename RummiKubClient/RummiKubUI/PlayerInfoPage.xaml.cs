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
using RummiKubUI.UtilClasses;

namespace RummiKubUI
{
    /// <summary>
    /// Interaction logic for PlayerInfoPage.xaml
    /// </summary>
    public partial class PlayerInfoPage : Page
    {
        Player Player;
        public PlayerInfoPage()
        {
            InitializeComponent();

            HoverArea.MouseEnter += IconEllipse_MouseEnter;
            HoverArea.MouseLeave += IconEllipse_MouseLeave;
        }
        public PlayerInfoPage(Player player) : this()
        {
            this.Player = player;
            this.DataContext = Player;

        }

        private void IconEllipse_MouseLeave(object sender, MouseEventArgs e)
        {
            EditButton.Visibility = Visibility.Hidden;
        }

        private void IconEllipse_MouseEnter(object sender, MouseEventArgs e)
        {
            if(Player.Id==MainWindow.CurrentUser.Id) 
                EditButton.Visibility = Visibility.Visible;
        }

        private void UpdatePlayer()
        {
            RummiKubServiceClient service = new RummiKubServiceClient();
            service.UpdateUser(this.Player);
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (Player.Id != MainWindow.CurrentUser.Id)
            {
                MainWindow.This_current_window.ShowErrorBox("Cannot Edit Other Players");
            }
            string filename = ImageUtils.UploadImage_Dlg(MainWindow.CurrentUser.Username);
            if (filename != null)
            {
                ImageUtils.SendImage(filename);
                Player.IconName = filename;
                UpdatePlayer();
            }
        }

        private void UsernameEdit(object sender, RoutedEventArgs e)
        {
            if(UsernameEditBox.Visibility==Visibility.Visible)
            {
                UsernameEditBox.Visibility = Visibility.Hidden;
                if (string.IsNullOrEmpty(UsernameEditBox.Text))
                {
                    MainWindow.This_current_window.ShowErrorBox("Username Cannot Be Empty");
                    return;
                }
                if (UsernameEditBox.Text.Length > 12)
                {
                    MainWindow.This_current_window.ShowErrorBox("Username Is Too Long");
                    return;
                }
                Player.Username = UsernameEditBox.Text;
                UpdatePlayer();
            }
            else
            {
                UsernameEditBox.Visibility = Visibility.Visible;
                UsernameEditBox.Focus();
            }
        }

        private void FirstNameEdit(object sender, RoutedEventArgs e)
        {
            if(FirstNameEditBox.Visibility == Visibility.Visible)
            {
                FirstNameEditBox.Visibility = Visibility.Hidden;

                if (string.IsNullOrEmpty(FirstNameEditBox.Text))
                {
                    MainWindow.This_current_window.ShowErrorBox("First Name Cannot Be Empty");
                    return;
                }
                Player.FirstName = FirstNameEditBox.Text;
                UpdatePlayer();
            }
            else
            {
                FirstNameEditBox.Visibility = Visibility.Visible;
                FirstNameEditBox.Focus();
            }
        }

        private void LastNameEdit(object sender, RoutedEventArgs e)
        {
            if(LastNameEditBox.Visibility == Visibility.Visible)
            {
                LastNameEditBox.Visibility = Visibility.Hidden;
                if (string.IsNullOrEmpty(LastNameEditBox.Text))
                {
                    MainWindow.This_current_window.ShowErrorBox("Last Name Cannot Be Empty");
                    return;
                }
                Player.LastName = LastNameEditBox.Text.Trim();
                UpdatePlayer();
            }
            else
            {
                LastNameEditBox.Visibility = Visibility.Visible;
                LastNameEditBox.Focus();
            }
        }

        private void EmailEdit(object sender, RoutedEventArgs e)
        {
            if(EmailEditBox.Visibility == Visibility.Visible)
            {
                EmailEditBox.Visibility = Visibility.Hidden;
                if (!EmailEditBox.Text.Trim().EndsWith("@gmail.com"))
                {
                    MainWindow.This_current_window.ShowErrorBox("Email must end with @gmail.com");
                    return;
                }
                if (EmailEditBox.Text.Trim().StartsWith("@gmail.com"))
                {
                    MainWindow.This_current_window.ShowErrorBox("Wrong Email Format ");
                    return;
                }
                Player.Email = EmailEditBox.Text;
                UpdatePlayer();
            }
            else
            {
                EmailEditBox.Visibility = Visibility.Visible;
                EmailEditBox.Focus();
            }
        }

    }
}
