

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices.WindowsRuntime;
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
    /// Interaction logic for PlayerBoardPage.xaml
    /// </summary>
    public partial class PlayerBoardPage : Page
    {

        #region Constructor
        public PlayerBoardPage()
        {
            InitializeComponent();
            Get14Tiles();
        }
        #endregion

        #region Methods
        public void Get14Tiles()
        {
            GamePage.playing_Player.playerBoard.TileList=new List<Tile>();

            List<Tile> tiles = null;

            RummiKubServiceClient service = new RummiKubServiceClient();
            var statuscode = service.TakeInitialTiles(GamePage.playing_Player.Id);
            if (statuscode.Code != 0)
            {
                MessageBox.Show(statuscode.Message);
                return;
            }
            tiles = statuscode.ReturnedObject;
            foreach(Tile t in tiles)
            {
                GamePage.playing_Player.playerBoard.TileList.Add(t);
            }
            UpdateVisuals();
        }
        public void AddTile()
        {
            RummiKubServiceClient service = new RummiKubServiceClient();
            GamePage.playing_Player.playerBoard.TileList.Add(service.TakeTileFromBank(GamePage.playing_Player.Id).ReturnedObject as Tile);
            UpdateVisuals();

        }
        public void UpdateVisuals()
        {
            AllPlayerTiles.Children.Clear();
            foreach (Tile t in GamePage.playing_Player.playerBoard.TileList)
            {
                AllPlayerTiles.Children.Add(new TileUserControl(t));
            }
        }


        public void RemoveChild(TileUserControl tile_uc)
        {
            GamePage.playing_Player.playerBoard.TileList.Remove(tile_uc.tile);
            AllPlayerTiles.Children.Remove(tile_uc);
        }
        #endregion
    }
}
