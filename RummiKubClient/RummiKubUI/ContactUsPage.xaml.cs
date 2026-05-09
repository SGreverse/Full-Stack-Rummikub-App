using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
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
    /// Interaction logic for ContactUsPage.xaml
    /// </summary>
    public partial class ContactUsPage : Page
    {
        private const string SMTP_SERVER = "smtp.gmail.com"; // or your SMTP server
        private const int SMTP_PORT = 587;
        private const string FROM_EMAIL = "segev2166@gmail.com"; // Your app's email
        private const string FROM_PASSWORD = "Segev2143#@"; // App password
        private const string TO_EMAIL = "segev2166@gmail.com"; // Where you want to receive messages

        public ContactUsPage()
        {
            InitializeComponent();
            HeadBackButtonPanel.Children.Add(new HeadBackButtonUserControl());
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate input
            if (!ValidateInput())
                return;

            // Disable button and show loading state
            var button = sender as Button;
            button.IsEnabled = false;
            button.Content = "Sending...";

            try
            {
                UserProblem userProblem= new UserProblem
                {
                    Email = EmailTextBox.Text,
                    Subject = SubjectTextBox.Text,
                    Message = MessageTextBox.Text
                };

                RummiKubServiceClient service=new RummiKubServiceClient();
                var status=service.SendProblemToAdmins(userProblem);

                if(status.Code>0)
                {
                    ShowStatus(status.Message, Brushes.Red);
                    return;
                }
                ShowStatus("Message sent successfully! Thank you for contacting us.", Brushes.Green);
                ClearForm();
            }
            catch (Exception ex)
            {
                ShowStatus($"Failed to send message: {ex.Message}", Brushes.Red);
            }
            finally
            {
                // Reset button
                button.IsEnabled = true;
                button.Content = CreateSendButtonContent();
            }
        }

        private bool ValidateInput()
        {

            if (string.IsNullOrWhiteSpace(EmailTextBox.Text) || !IsValidEmail(EmailTextBox.Text))
            {
                ShowStatus("Please enter a valid email address.", Brushes.Orange);
                EmailTextBox.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(SubjectTextBox.Text))
            {
                ShowStatus("Please enter a subject.", Brushes.Orange);
                SubjectTextBox.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(MessageTextBox.Text))
            {
                ShowStatus("Please enter your message.", Brushes.Orange);
                MessageTextBox.Focus();
                return false;
            }

            return true;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }


        


        private void ShowStatus(string message, Brush color)
        {
            StatusTextBlock.Text = message;
            StatusTextBlock.Foreground = color;
            StatusTextBlock.Visibility = Visibility.Visible;

            // Auto-hide success messages after 5 seconds
            if (color == Brushes.Green)
            {
                var timer = new System.Windows.Threading.DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(5);
                timer.Tick += (s, e) =>
                {
                    StatusTextBlock.Visibility = Visibility.Collapsed;
                    timer.Stop();
                };
                timer.Start();
            }
        }

        private void ClearForm()
        {
            EmailTextBox.Clear();
            SubjectTextBox.Clear();
            MessageTextBox.Clear();
        }

        private object CreateSendButtonContent()
        {
            var stackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };

            var icon = new TextBlock
            {
                Text = "✉",
                FontSize = 18,
                Margin = new Thickness(0, 0, 8, 0)
            };

            var text = new TextBlock
            {
                Text = "Send Message"
            };

            stackPanel.Children.Add(icon);
            stackPanel.Children.Add(text);

            return stackPanel;
        }

        private void MessageTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            MessageLengthTextBlock.Text = $"{MessageTextBox.Text.Length} / 500 characters";
        }
    }
}

