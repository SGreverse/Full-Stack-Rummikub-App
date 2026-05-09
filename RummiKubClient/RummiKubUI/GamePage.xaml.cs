
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
using System.Windows.Threading;
using RummiKubUI.ServiceReference1;
using RummiKubUI.UserControls;
namespace RummiKubUI
{
    /// <summary>
    /// Interaction logic for GamePage.xaml
    /// </summary>
    public partial class GamePage : Page
    {
        #region Properties
        //Player Current_Player;
        //List<Player> OtherPlayers;

        BoardPage boardPage;



        public List<TileUserControl> AddingToSeries = new List<TileUserControl>();

        PlayerBoardPage playerboardPage;

        DispatcherTimer dispatcherTimer;

        public static PlayerInGame playing_Player;

        public ObservableCollection<VisualDisplayOfPlayer> PlayersInGame = new ObservableCollection<VisualDisplayOfPlayer>();
        public static bool IsThePlayerTurn
        {
            get
            {
                return playing_Player.This_PlayerState == PlayerState.Playing;
            }
        }

        #endregion

        #region Constructor
        public GamePage()
        {
            InitializeComponent();
            if (MainWindow.CurrentUser.IsAdmin)
            {
                DropTileAdmin.Visibility = Visibility.Visible;
            }
            else
            {
                DropTileAdmin.Visibility = Visibility.Collapsed;
            }

        }
        public GamePage(VisualDisplayOfGame visualgame) : this()
        {
            MainWindow.This_current_window.dispatcherTimer.Stop();
            playing_Player = new PlayerInGame { Id = MainWindow.CurrentUser.Id, playerBoard = new PlayerBoard() };
            foreach (VisualDisplayOfPlayer visualplayer in visualgame.visualDisplayOfPlayers)
            {
                UsersPanel.Children.Add(new PlayerDisplayUserContorl(visualplayer));
            }
            StartGame();

        }
        #endregion

        #region Methods

        private void StartTimer()//gets a function to run using the dispatcher timer
        {
            dispatcherTimer = new DispatcherTimer(DispatcherPriority.Normal, Dispatcher.CurrentDispatcher);
            dispatcherTimer.Tick += new EventHandler(IsMyTurn);
            dispatcherTimer.Interval = TimeSpan.FromSeconds(0.5);
            dispatcherTimer.Start();
        }
        public void StartGame()
        {
            this.boardPage = new BoardPage();
            BoardFrame.Navigate(this.boardPage);
            this.playerboardPage = new PlayerBoardPage();
            PlayerHandFrame.Navigate(this.playerboardPage);

            StartTimer();
        }
        public void AddToListClicked(object sender, EventArgs e) //add to the list of tiles to be used for making sets and adding cards
        {
            if (IsThePlayerTurn)
            {
                TileUserControl uc = sender as TileUserControl;
                if (uc != null)
                {
                    this.AddingToSeries.Add(uc);
                }
            }

        }
        public void RemoveFromListClicked(object sender, EventArgs e) // remove a tile from the list
        {
            if (IsThePlayerTurn)
            {
                TileUserControl uc = sender as TileUserControl;
                if (uc != null)
                {
                    int index = (this.AddingToSeries.FindIndex(t => t.Equals(uc)));
                    if (index != -1)
                    {
                        this.AddingToSeries.RemoveAt(index);
                    }
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)//try to add a new series to the board
        {

            AddSeries(this.AddingToSeries.Select(uc => uc.tile).ToList(), playing_Player.Id);
        }
        private void AddSeries(List<Tile> series, int playerId)
        {


            List<Tile> tiles_from_player = new List<Tile>();
            List<Tuple<Tile, int>> tiles_from_board = new List<Tuple<Tile, int>>();

            SortBetweenTilesFromboardAndPlayer(tiles_from_player, tiles_from_board);

            RummiKubServiceClient service = new RummiKubServiceClient();

            InsertingSeriesAction action = new InsertingSeriesAction() { All_Tiles = series, tiles_from_player = tiles_from_player, tiles_from_board = tiles_from_board };
            var statuscode = service.ExecuteActionInGame(action, playerId); //send to server the action

            if (statuscode.Code != 0)
            {
                MainWindow.This_current_window.ShowErrorBox(statuscode.Message);
                return;
            }
            UpdatePlayerHand();

            AddingToSeries.Clear();
        }
        public void SortBetweenTilesFromboardAndPlayer(List<Tile> tiles_from_player, List<Tuple<Tile, int>> tiles_from_board)
        {//distinguish between tiles from board and from the player hand
            foreach (TileUserControl TileUC in AddingToSeries)
            {
                SeriesUserControl Parent_series_uc = ParentConverter.GetParentPage<SeriesUserControl>(TileUC) as SeriesUserControl;
                if (Parent_series_uc != null)
                {
                    tiles_from_board.Add(new Tuple<Tile, int>(TileUC.tile, Parent_series_uc.series.SeriesId));
                }
                else
                {
                    tiles_from_player.Add(TileUC.tile);
                }
            }
        }
        public void UpdatePlayerHand()//update the visual of the players hand upon putting or recieving a tile
        {
            RummiKubServiceClient service = new RummiKubServiceClient();
            playing_Player.playerBoard.TileList = service.GetCurrentPlayerHand(playing_Player.Id).ReturnedObject as List<Tile>;
            playerboardPage.UpdateVisuals();

        }
        private void EndTurn_Click(object sender, RoutedEventArgs e)

        {

            if (!IsThePlayerTurn)
            {
                MainWindow.This_current_window.ShowErrorBox("It Is Not Your Turn Yet");
                return;
            }
            this.AddingToSeries.Clear();

            var statuscode = boardPage.EndTurn();


            if (statuscode.Code != 0)
            {
                MainWindow.This_current_window.ShowErrorBox(statuscode.Message);
                return;
            }

            UpdatePlayerHand();


        }


        private void IsMyTurn(object sender, EventArgs e)
        {
            RummiKubServiceClient service = new RummiKubServiceClient();
            var statuscode = service.PlayerStateInGame(playing_Player.Id);

            if (statuscode.Code != 0 || statuscode.ReturnedObject.GameState == GameStates.Ended)
            {
                UpdatePlayerStatistics();
                dispatcherTimer.Stop();
                MainWindow.This_current_window.StartTimer();
            }
            else
            {
                playing_Player.This_PlayerState = statuscode.ReturnedObject.visualDisplayOfPlayers.FirstOrDefault(x => x.Id == playing_Player.Id).PlayerState;
                UpdateAllPlayersVisual(statuscode.ReturnedObject.visualDisplayOfPlayers);
                AskForBoard();
                RecieveRecentChatMessage();
            }
        }

        private void UpdateAllPlayersVisual(List<VisualDisplayOfPlayer> players)
        {
            for (int i = 0; i < UsersPanel.Children.Count; i++)
            {
                PlayerDisplayUserContorl playerdisplay = (UsersPanel.Children[i] as PlayerDisplayUserContorl);
                VisualDisplayOfPlayer visual_player = playerdisplay.DataContext as VisualDisplayOfPlayer;
                visual_player.TileAmount = players[i].TileAmount;
                playerdisplay.UpdatePlayerStatus(players[i].PlayerState);

            }
        }
        private void UpdatePlayerStatistics()
        {
            RummiKubServiceClient service = new RummiKubServiceClient();
            GameEndOverlay.Visibility = Visibility.Visible;
            Chat.Visibility = Visibility.Collapsed;
            MenuOverlay.Visibility = Visibility.Collapsed;

            Player newplayer = service.GetPlayer(playing_Player.Id).ReturnedObject;
            ScoreDisplay.Text += (newplayer.Score - MainWindow.CurrentUser.Score).ToString() + " Points";
            MainWindow.CurrentUser = newplayer;
            dispatcherTimer.Stop();
        }
        private void AskForBoard()//if the board has changed, update the visuals
        {
            RummiKubServiceClient service = new RummiKubServiceClient();
            if (service.HasBoardChanged(playing_Player.Id).ReturnedObject)
            {
                boardPage.GetBoardFromServer();
            }
        }
        private void RecieveRecentChatMessage()// display all the unread chat messages
        {
            RummiKubServiceClient service = new RummiKubServiceClient();
            var status = service.RecieveRecentGameChatMessage(playing_Player.Id);
            if (status.Code == 0)
            {
                foreach (Message message in status.ReturnedObject)
                {
                    MessagesContainer.Children.Add(new MessageUserControl(message));
                }
            }
        }
        private void ResetTurn_Click(object sender, RoutedEventArgs e)
        {
            if (!IsThePlayerTurn)
            {
                return;
            }
            RummiKubServiceClient service = new RummiKubServiceClient();
            service.ResetTurn(playing_Player.Id);

            UpdatePlayerHand();

            AddingToSeries.Clear();
        }

        private void DropTile_Click(object sender, RoutedEventArgs e)//only available for admins!
        {
            RummiKubServiceClient service = new RummiKubServiceClient();
            foreach (TileUserControl TileUC in AddingToSeries)
            {
                service.DropTileFromHand(playing_Player.Id, TileUC.tile);
            }
            UpdatePlayerHand();

            AddingToSeries.Clear();
        }

        private void PlayAgain_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.This_current_window.GameFrame.Navigate(new PlayerAmountSelectionPage());
        }

        private void ReturnToMenu_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.This_current_window.GameFrame.Navigate(new MainMenuPage());
        }

        private void Message_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                SendMessage();

        }
        private void SendMessage()
        {
            if (Message.Text == "" || Message.Text == null)
                return;
            RummiKubServiceClient service = new RummiKubServiceClient();

            service.AddMessageToGameChat(Message.Text, playing_Player.Id);
            Message.Text = "";
        }

        private void ShowChat_Click(object sender, RoutedEventArgs e)
        {
            if (Chat.Visibility == Visibility.Visible)
            {
                Chat.Visibility = Visibility.Collapsed;
                MessageIconBackground.Fill = new SolidColorBrush(Colors.Black);
                MessageIcon.Foreground = new SolidColorBrush(Colors.White);

            }
            else
            {
                Chat.Visibility = Visibility.Visible;
                MessageIconBackground.Fill = new SolidColorBrush(Colors.White);
                MessageIcon.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void ShowMenu_Click(object sender, RoutedEventArgs e)
        {
            if (MenuOverlay.Visibility == Visibility.Visible)
            {
                MenuOverlay.Visibility = Visibility.Collapsed;
                MenuIconBackground.Fill = new SolidColorBrush(Colors.Black);
                MenuIcon.Foreground = new SolidColorBrush(Colors.White);

            }
            else
            {
                MenuOverlay.Visibility = Visibility.Visible;
                MenuIconBackground.Fill = new SolidColorBrush(Colors.White);
                MenuIcon.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void Surrender_Click(object sender, RoutedEventArgs e)
        {
            if (playing_Player.This_PlayerState == PlayerState.Surrender || playing_Player.This_PlayerState == PlayerState.Finished)
            {
                MainWindow.This_current_window.ShowErrorBox("Cannot Surrender After You Finished ");
                return;
            }
            RummiKubServiceClient service = new RummiKubServiceClient();
            service.PlayerSurrended(playing_Player.Id);
        }


        #endregion

    }
}
