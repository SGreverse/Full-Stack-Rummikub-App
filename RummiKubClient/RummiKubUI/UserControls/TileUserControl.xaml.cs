using RummiKubUI.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RummiKubUI
{
    /// <summary>
    /// Interaction logic for TileUserControl.xaml
    /// </summary>

    public partial class TileUserControl : UserControl
    {
        #region properties
        static SolidColorBrush[] Tilecolors;
        public bool IsPressed=false;
        public Tile tile;
        #endregion

        #region constructor
        static TileUserControl()
        {
            Tilecolors = new SolidColorBrush[] {
                new SolidColorBrush(Colors.Black),
                new SolidColorBrush(Colors.Orange),
                new SolidColorBrush(Colors.Red),
                new SolidColorBrush(Colors.Blue)
            };

        }
        public TileUserControl()
        {
            InitializeComponent();
            this.MaxHeight = 50;
            this.MaxWidth = 30;

        }
        public TileUserControl(Tile tile):this()
        {
            
            this.tile = tile;
            this.DataContext = tile;
            this.Background = Tilecolors[(int)tile.Color];
        }
        #endregion

        #region methods
        private void ClickB1(object sender, RoutedEventArgs e)
        {
            if (GamePage.IsThePlayerTurn)
            {

                GamePage mainwindow = ParentConverter.GetParentPage<GamePage>(this) as GamePage;

                if (IsPressed)
                {

                    this.BorderThickness = new Thickness(0);
                    IsPressed = false;
                    mainwindow?.RemoveFromListClicked(this, e);
                }
                else
                {

                    this.BorderThickness = new Thickness(2);
                    this.BorderBrush = new SolidColorBrush(Colors.Black);
                    IsPressed = true;
                    mainwindow?.AddToListClicked(this, e);
                }
            }
        }

        #endregion

    }
}
