
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Linq.Expressions;
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
using RummiKubUI.ServiceReference1;
namespace  RummiKubUI
{
    /// <summary>
    /// Interaction logic for SeriesUserControl.xaml
    /// </summary>
    public partial class SeriesUserControl : UserControl
    {
        #region properties
        private bool isExpanded=false;
        public TileSeries series {  get; set; }
        #endregion

        #region Constructor
        public SeriesUserControl()
        {
            InitializeComponent();

        }
        public SeriesUserControl(TileSeries series) : this()
        {
            this.series = series;
           
            UpdateSeriesVisuals();
            UpdateExpansionButton();
        }
        #endregion

        #region Methods

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (!GamePage.IsThePlayerTurn)
                return;

            var mainpage = ParentConverter.GetParentPage<GamePage>(this) as GamePage;
            List<TileUserControl> adding_to_series = mainpage.AddingToSeries;

            List<Tile> tiles_from_player = new List<Tile>();
            List<Tuple<Tile, int>> tiles_from_board = new List<Tuple<Tile, int>>();

            mainpage.SortBetweenTilesFromboardAndPlayer(tiles_from_player, tiles_from_board);


            RummiKubServiceClient service = new RummiKubServiceClient();
            AddingTileAction action = new AddingTileAction() {NewSeriesId= this.series.SeriesId, All_Tiles = adding_to_series.Select(t => t.tile).ToList(), tiles_from_board = tiles_from_board, tiles_from_player = tiles_from_player };

            var status =service.ExecuteActionInGame(action, GamePage.playing_Player.Id);

            if (status.Code!=0) {
                MainWindow.This_current_window.ShowErrorBox(status.Message);
                return;
            }
            if(tiles_from_player.Count>0)
            {
                mainpage.UpdatePlayerHand();
            }

            adding_to_series.Clear();
        }

        public void UpdateSeriesVisuals()
        {
            TileSeries.Children.Clear();
            var items = new List<Tile>();
            if (series.set.Count > 4)
                items = (isExpanded && series.set.Count > 4) ? series.set : series.set.GetRange(0, 4);
            else
                items = series.set;
            foreach (Tile tile in items)
            {
                TileUserControl uc = new TileUserControl(tile);
                TileSeries.Children.Add(uc);
            }
        }
        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            isExpanded = !isExpanded;
            ToggleButton.Content = isExpanded ? "-" : "+";

            if (!isExpanded)
            {
                for(int i=4; i<series.set.Count; i++)
                {
                    var mainpage = ParentConverter.GetParentPage<GamePage>(this) as GamePage;
                    List<TileUserControl> adding_to_series = mainpage.AddingToSeries;
                    adding_to_series.RemoveAll(t => t.tile == series.set[i]);

                }
            }
            // Update StackPanel display

            UpdateSeriesVisuals();
        }
        private void UpdateExpansionButton()
        {
            if(this.series.set.Count>4)
                ToggleButton.Visibility = Visibility.Visible;
            else
                ToggleButton.Visibility = Visibility.Collapsed;
        }
        #endregion
    }
}
