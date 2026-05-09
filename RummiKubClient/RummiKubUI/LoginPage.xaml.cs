using RummiKubUI.Security;
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
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
            this.HeadBackButtonPanel.Children.Add(new HeadBackButtonUserControl());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string Username =this.UsernameTextBox.Text;
            string password = this.PasswordTextBox.Password;
            
            string Encryptedpass = RSASecurity.Encrypt(password);
            RummiKubServiceClient service = new RummiKubServiceClient();
            var statuscode =service.Login(Username, Encryptedpass);

            if (statuscode.Code!=0)
            {
                ErrorMessage.Text = statuscode.Message;
                ErrorMessage.Visibility = Visibility.Visible;

            }
            else
            {
                MainWindow.CurrentUser = statuscode.ReturnedObject;
                MainWindow.This_current_window.GameFrame.Navigate(new MainMenuPage());
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow.This_current_window.GameFrame.Navigate(new RegisterPage());

        }
    }
}
