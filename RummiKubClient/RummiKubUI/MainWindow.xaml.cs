using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;
using RummiKubUI.ServiceReference1;
namespace RummiKubUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window //game window
    {
        #region Properties
        public DispatcherTimer dispatcherTimer = new DispatcherTimer();
        public RummiKubServiceClient service;

        public static bool IsLoggedIn
        {
            get
            {
                return CurrentUser != null;
            }
        }

        public static bool IsWaitingForGame = false;

        public static Player CurrentUser;

        public static MainWindow This_current_window;//  every page have access to the main window's elements and Methods

        public static string CurrentGameCode;

        private static ObservableCollection<int> playerCountInList=new ObservableCollection<int>();

        public static ObservableCollection<int> PlayerCountInList {//display for the amount of players in queue.
            get{
              return playerCountInList;
            }
            set => playerCountInList = value; }


        #endregion

        #region Constructor

        public MainWindow()
        {
            InitializeComponent();
            GameFrame.Navigate(new MainMenuPage());
            This_current_window = this;
            service = new RummiKubServiceClient();
            StartTimer();

        }
        #endregion

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F11) // Change F11 to any key you want
            {
                this.WindowState = (this.WindowState == WindowState.Maximized) ? WindowState.Normal : WindowState.Maximized;
            }
        }

        public void StartTimer( )
        {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(ServerPing_Tick);
            dispatcherTimer.Interval = TimeSpan.FromSeconds(2);
            dispatcherTimer.Start();
        }
        public void ServerPing_Tick(object sender, EventArgs e)
        {
            if(!IsLoggedIn)
            {
                return;
            }
            var statusCode = service.WaitForGame(CurrentUser.Id);
            if (statusCode.Code == 0)
            {
                PlayerCountInList.Clear();
                IsWaitingForGame = false;
                GameFrame.Navigate(new GamePage(statusCode.ReturnedObject));
                dispatcherTimer.Stop();
            }
            else if (statusCode.Code > 1)
            {
                This_current_window.ShowErrorBox(statusCode.Message);
            }
            else
            {
                if (IsWaitingForGame)
                {
                    PlayerCountInList.Clear();
                    int count = service.GetGameQueuePlayerAmount(CurrentGameCode).ReturnedObject;
                    for (int i = 0; i < count; i++)
                    {
                        PlayerCountInList.Add(i);
                    }

                }
            }
        }

        
        public void ShowErrorBox(string message,bool IsInformative=false)
        {
            if (IsInformative)
            {
                BoxSign.Text = "ℹ️";
                BoxSign.Margin = new Thickness(10, 10, 10, 10);
                BoxSign.Foreground = new SolidColorBrush(Colors.Black);
            }
            else
            {
                BoxSign.Text = "⚠";
                BoxSign.Margin = new Thickness(0, 0, 0, 0);
                BoxSign.Foreground = new SolidColorBrush(Colors.Red);

            }
            ErrorBox.BeginAnimation(UIElement.OpacityProperty, null);
            ErrorBox.Visibility=Visibility.Visible;
            ErrorBox.Opacity = 1;
            ErrorMessage.Text = message;

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (s, e) =>
            {

                Storyboard sb = new Storyboard();
                DoubleAnimation doubleAnimation = new DoubleAnimation()
                {
                    From = 1,
                    To = 0,
                    Duration = TimeSpan.FromSeconds(0.5),
                };

                // Set the target of the animation (ErrorBox)
                Storyboard.SetTarget(doubleAnimation, ErrorBox);
                Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath(UIElement.OpacityProperty));

                sb.Children.Add(doubleAnimation);
                sb.Completed += (a, b) =>
                {
                    ErrorBox.Visibility = Visibility.Collapsed;
                };
                sb.Begin();
                timer.Stop();
            };

            // Start the timer
            timer.Start();
        }
    }
}
