using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using RummiKubUI.Security;
using RummiKubUI.ServiceReference1;
using RummiKubUI.UserControls;
using RummiKubUI.UtilClasses;
using static System.Collections.Specialized.BitVector32;

namespace RummiKubUI
{
    /// <summary>
    /// Interaction logic for RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {

        private int currentPanelIndex = 0;
        private Grid[] panels;
        Player new_player;

        private List<string> validationErrors = new List<string>();

        public RegisterPage()
        {
            InitializeComponent();
            this.AddHandler(Validation.ErrorEvent, new RoutedEventHandler(OnValidationError));

            new_player = new Player();
            IconNavUC.Children.Add(new IconNavigationUserControl(new_player));
            Backbutton.Children.Add(new HeadBackButtonUserControl());
            panels = new Grid[] { Panel1, Panel2, Panel3 };
            this.DataContext = new_player;
            RummiKubServiceClient service = new RummiKubServiceClient();
            this.Cities.ItemsSource = service.GetAllCities().ReturnedObject;

        }



        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            SwitchPanel((currentPanelIndex - 1 + panels.Length) % panels.Length);

        }

        private void SwitchPanel(int newIndex)
        {
            Grid currentPanel = panels[currentPanelIndex];
            Grid nextPanel = panels[newIndex];

            nextPanel.Visibility = Visibility.Visible;
            nextPanel.Opacity = 0;

            double newHeight = nextPanel.Height;

            Storyboard sb = new Storyboard();

            DoubleAnimation heightAnimation = new DoubleAnimation
            {
                From = PanelContainer.ActualHeight,
                To = newHeight,
                Duration = TimeSpan.FromSeconds(0.3),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseInOut }
            };
            Storyboard.SetTarget(heightAnimation, PanelContainer);
            Storyboard.SetTargetProperty(heightAnimation, new PropertyPath(HeightProperty));
            sb.Children.Add(heightAnimation);


            DoubleAnimation fadeOutAnimation = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.2)
            };
            Storyboard.SetTarget(fadeOutAnimation, currentPanel);
            Storyboard.SetTargetProperty(fadeOutAnimation, new PropertyPath(UIElement.OpacityProperty));
            sb.Children.Add(fadeOutAnimation);

            DoubleAnimation fadeInAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                BeginTime = TimeSpan.FromSeconds(0.3),
                Duration = TimeSpan.FromSeconds(0.2)
            };
            Storyboard.SetTarget(fadeInAnimation, nextPanel);
            Storyboard.SetTargetProperty(fadeInAnimation, new PropertyPath(UIElement.OpacityProperty));
            sb.Children.Add(fadeInAnimation);

            sb.Completed += (s, e) =>
            {
                currentPanel.Visibility = Visibility.Collapsed;
                nextPanel.Opacity = 1;
                switch (currentPanelIndex)
                {
                    case 0:
                        NextButton.Click -= FirstPanelValidation;
                        break;
                    case 1:
                        NextButton.Click -= SecondPanelValidation;
                        break;

                }
                switch (newIndex)
                {
                    case 0:
                        NextButton.Click += FirstPanelValidation;
                        break;
                    case 1:
                        NextButton.Click += SecondPanelValidation;
                        break;
                }
                currentPanelIndex = newIndex;

                PreviousButton.Visibility = (currentPanelIndex > 0) ? Visibility.Visible : Visibility.Hidden;
                NextButton.Visibility = (currentPanelIndex < panels.Length - 1) ? Visibility.Visible : Visibility.Collapsed;
                RegisterButton.Visibility = (currentPanelIndex == panels.Length - 1) ? Visibility.Visible : Visibility.Collapsed;


            };

            sb.Begin();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            RummiKubServiceClient service = new RummiKubServiceClient();
            ImageUtils.SendImage(new_player.IconName);

            new_player.Username = new_player.Username.Trim();
            new_player.Password = RSASecurity.Encrypt(new_player.Password.Trim());
            var Status = service.Register(new_player);
            if (Status.Code == 0)
            {

                MainWindow.CurrentUser = Status.ReturnedObject;
                MainWindow.This_current_window.GameFrame.Navigate(new MainMenuPage());
            }

 
        }
        private void OnTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox box)
            {
                TextBlock textblock = (box.Parent as Grid).Children[1] as TextBlock;
                if (string.IsNullOrEmpty(box.Text))
                    textblock.Visibility = Visibility.Visible;
                else
                    textblock.Visibility = Visibility.Collapsed;
            }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox box)
            {
                if (box == PasswordBox)
                    new_player.Password = box.Password;

                TextBlock textblock = (box.Parent as Grid).Children[1] as TextBlock;
                if (string.IsNullOrEmpty(box.Password))
                    textblock.Visibility = Visibility.Visible;
                else
                    textblock.Visibility = Visibility.Collapsed;
            }
        }

        private void FirstPanelValidation(object sender, RoutedEventArgs e)
        {
            bool hasError = Validation.GetHasError(UsernameTextBox);
            bool NotEqual = (PasswordBox.Password != ConfirmPasswordBox.Password);

            if (hasError)
            {
                return;
            }

            RummiKubServiceClient service = new RummiKubServiceClient();
            var status = service.IsUsernameUnique(UsernameTextBox.Text);
            if (status.Code != 0)
            {
                ErrorTextBlock.Text = status.Message;
                ErrorTextBlock.Visibility = Visibility.Visible;
                return;
            }
            if (string.IsNullOrEmpty(PasswordBox.Password))
            {
                ErrorTextBlock.Text = "Password Cannot Be Empty";
                ErrorTextBlock.Visibility = Visibility.Visible;
                return;
            }
            if (NotEqual)
            {
                ErrorTextBlock.Text = "You Have Not Confirmed Your Password";
                ErrorTextBlock.Visibility = Visibility.Visible;
                return;
            }
            ErrorTextBlock.Text = "";
            ErrorTextBlock.Visibility = Visibility.Collapsed;
            SwitchPanel((currentPanelIndex + 1) % panels.Length);
        }
        private void SecondPanelValidation(object sender, RoutedEventArgs e)
        {
            bool HasErrors = Validation.GetHasError(EmailTextBox) || Validation.GetHasError(FirstNameTextBox)
                || Validation.GetHasError(LastNameTextBox);

            if (HasErrors)
            {
                return;
            }
            RummiKubServiceClient service=new RummiKubServiceClient();
            var status = service.IsEmailUnique(EmailTextBox.Text);
            if (status.Code != 0)
            {
                ErrorTextBlock.Text = status.Message;
                ErrorTextBlock.Visibility = Visibility.Visible;
                return;
            }
            if (Cities.SelectedItem == null)
            {
                ErrorTextBlock.Text = "Please Select A City";
                ErrorTextBlock.Visibility = Visibility.Visible;
                return;
            }
            ErrorTextBlock.Text = "";
            ErrorTextBlock.Visibility = Visibility.Collapsed;
            SwitchPanel((currentPanelIndex + 1) % panels.Length);
        }
        private void OnValidationError(object sender, RoutedEventArgs e)
        {
            ValidationErrorEventArgs validationEvent = (ValidationErrorEventArgs)e;

            if (validationEvent.Action == ValidationErrorEventAction.Added)
            {
                validationErrors.Add(validationEvent.Error.ErrorContent.ToString());
            }
            else if (validationEvent.Action == ValidationErrorEventAction.Removed)
            {
                validationErrors.Remove(validationEvent.Error.ErrorContent.ToString());
            }

            // Update the error TextBlock
            UpdateErrorDisplay();
        }
        private void UpdateErrorDisplay()
        {
            if (validationErrors.Count > 0)
            {
                ErrorTextBlock.Text = validationErrors[0];
                ErrorTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                ErrorTextBlock.Text = string.Empty;
                ErrorTextBlock.Visibility = Visibility.Collapsed;
            }
        }


    }

}
