using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using RummiKubUI.ServiceReference1;
using RummiKubUI.UserControls;
using RummiKubUI.UtilClasses;

namespace RummiKubUI
{
    /// <summary>
    /// Interaction logic for PrivateChatPage.xaml
    /// </summary>
    public partial class PrivateChatPage : Page
    {
        Friend OtherPlayer;
        DispatcherTimer DispatcherTimer=new DispatcherTimer();

        Message SelectedMessage;

        ObservableCollection<Message> Messages = new ObservableCollection<Message>();
        public PrivateChatPage()
        {
            InitializeComponent();
            EditButton.DataContext = SelectedMessage;
            DeleteButton.DataContext = SelectedMessage;
            HeadBackButtonPanel.Children.Add(new HeadBackButtonUserControl());
            this.Unloaded += Page_Unloaded;

        }
        public PrivateChatPage(Friend otherplayer) : this()
        {
            RummiKubServiceClient service = new RummiKubServiceClient();
            service.EnterPrivateChat(MainWindow.CurrentUser.Id, otherplayer.PlayerId);
            this.OtherPlayer = otherplayer;
            this.DataContext=this.OtherPlayer;
            var statuscode=service.GetAllChatMessages(MainWindow.CurrentUser.Id, otherplayer.PlayerId,true);
            
            Messages=new ObservableCollection<Message>( statuscode.ReturnedObject.Select(_=>_.message));

            lstview.ItemsSource = Messages;

            DispatcherTimer.Interval = TimeSpan.FromSeconds(0.5);
            DispatcherTimer.Tick += new EventHandler(LookForMessages);
            DispatcherTimer.Start();


        }
        public void LookForMessages(object sender, EventArgs e)
        {
            RummiKubServiceClient service = new RummiKubServiceClient();
            var statuscode = service.GetAllChatMessages(MainWindow.CurrentUser.Id, OtherPlayer.PlayerId,false);
            if (statuscode.Code == 1) return;

            foreach(MessageAction messageAction in statuscode.ReturnedObject)
            {
                switch (messageAction.type)
                {
                    case MessageActionType.Insert:
                        Messages.Add(messageAction.message);
                        break;

                    case MessageActionType.Delete:
                         Message msg = Messages.First(_=>messageAction.message.Id==_.Id);
                        Messages.Remove(msg);
                        break;

                    case MessageActionType.Edit:
                        Message message = Messages.First(_ => messageAction.message.Id == _.Id);
                        int index=Messages.IndexOf(message);
                        Messages.RemoveAt(index);
                        Messages.Insert(index, messageAction.message);
                        break;

                }
                
            }

        }
        private void Message_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                SendMessage();

        }
        private void SendMessage()
        {
            if (TextMessage.Text == "" || TextMessage.Text == null )
                return;
            RummiKubServiceClient service = new RummiKubServiceClient();
            Message message=new Message() { 
                SenderId=MainWindow.CurrentUser.Id,
                RecieverId=OtherPlayer.PlayerId,
                Msg=TextMessage.Text,
                IsEdited=false,
                SendingTime=DateTime.Now,
                SentBy=MainWindow.CurrentUser.Username 
            };
            service.SendMessageToPrivateChat(message);
            TextMessage.Text = "";

        }

        private void Options_Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button clickedItem)
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
                double popupX = relativeX - 10;  // Offset from item
                double popupY = relativeY;

                // Apply popup position
                ContextMenuPopup.HorizontalOffset = popupX;
                ContextMenuPopup.VerticalOffset = popupY;

                // Show popup
                Message message = clickedItem.DataContext as Message;

                ContextMenuPopup.IsOpen = true;
                SelectedMessage = message;
                EditButton.DataContext = SelectedMessage;
                DeleteButton.DataContext = SelectedMessage;
            }
        }

        private void EditMessage(object sender, RoutedEventArgs e)
        {
            if((DateTime.Now - SelectedMessage.SendingTime).TotalHours > 3)
            {
                MainWindow.This_current_window.ShowErrorBox("Message Is Not New Enough To Edit");
                return;
            }
            EditBox.Visibility = Visibility.Visible;
            EditText.DataContext = SelectedMessage;
            ContextMenuPopup.IsOpen = false;

        }

        private void DeleteMessage(object sender, RoutedEventArgs e)
        {
            RummiKubServiceClient service = new RummiKubServiceClient();
            service.DeleteMessageFromPrivateChat(SelectedMessage);
            ContextMenuPopup.IsOpen=false;
        }

        private void ConfirmEdit(object sender, RoutedEventArgs e)
        {
            SelectedMessage.IsEdited = true;
            RummiKubServiceClient service = new RummiKubServiceClient();
            service.EditMessageInPrivateChat(SelectedMessage);
            EditBox.Visibility = Visibility.Hidden;

        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            DispatcherTimer.Stop();
            RummiKubServiceClient service = new RummiKubServiceClient();
            service.ExitPrivateChat(MainWindow.CurrentUser.Id);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            MessageDescriptionBox.Visibility = Visibility.Collapsed;
        }

        private void ShowDescription(object sender, RoutedEventArgs e)
        {
            MessageDescriptionBox.Visibility= Visibility.Visible;
            MessageDescriptionBox.DataContext = SelectedMessage;
            ContextMenuPopup.IsOpen = false;

        }
    }
}
