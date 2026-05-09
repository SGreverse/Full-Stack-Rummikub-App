
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
namespace RummiKubUI
{
    /// <summary>
    /// Interaction logic for BoardPage.xaml
    /// </summary>
    public partial class BoardPage : Page
    {
        #region Properties
        public Board board;
        #endregion

        #region Constructor
        public BoardPage()
        {
            InitializeComponent();

            RummiKubServiceClient service = new RummiKubServiceClient();
            this.board = new Board() { OnBoardSeries = new List<TileSeries>(),Series_On_Board_Amount=0 };
        }

        #endregion

        #region Methods
        public PostRequestStatusCode EndTurn()//ask the server if the game board the client put is correct
        {
            this.board.OnBoardSeries.Clear();
            RummiKubServiceClient service = new RummiKubServiceClient();

            return service.EndTurn(GamePage.playing_Player.Id);
        }

        public void GetBoardFromServer()
        {
            RummiKubServiceClient service = new RummiKubServiceClient();
            this.board = service.GetCurrentBoard(GamePage.playing_Player.Id).ReturnedObject;
            UpdateVisuals();
        }
        public void UpdateVisuals()
        {
            AllSeries.Children.Clear();
            foreach (TileSeries series in this.board.OnBoardSeries)
            {
                AllSeries.Children.Add(new SeriesUserControl(series));
            }
            GamePage gamePage = ParentConverter.GetParentPage<GamePage>(this) as GamePage;
            gamePage.AddingToSeries.Clear();

        }

        #endregion
    }
}
