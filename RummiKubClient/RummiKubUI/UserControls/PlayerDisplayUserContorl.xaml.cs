using RummiKubUI.ServiceReference1;
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

namespace RummiKubUI.UserControls
{
    /// <summary>
    /// Interaction logic for PlayerDisplayUserContorl.xaml
    /// </summary>
    public partial class PlayerDisplayUserContorl : UserControl
    {
        public PlayerDisplayUserContorl()
        {
            InitializeComponent();
        }
        public PlayerDisplayUserContorl(VisualDisplayOfPlayer player):this()
        {
            this.DataContext= player;
        }
        public PlayerDisplayUserContorl(Player player) : this()
        {
            this.DataContext= player;
            this.TileAmount.Visibility = Visibility.Collapsed;

        }
        public void UpdatePlayerStatus(PlayerState playerState)
        {
            switch(playerState)
            {
                case PlayerState.Playing:
                    PlayerBorder.BorderBrush=new SolidColorBrush(Colors.White);
                    break;
                case PlayerState.Finished:
                    InGameOverlay.Visibility = Visibility.Visible;
                    PlayerStateOverlay.Text = "Finished";
                    PlayerBorder.BorderBrush = new SolidColorBrush(Colors.Black);
                    break;
                case PlayerState.Surrender:
                    InGameOverlay.Visibility = Visibility.Visible;
                    PlayerStateOverlay.Text = "Surrendered";
                    PlayerBorder.BorderBrush = new SolidColorBrush(Colors.Black);
                    break;

                default:
                    PlayerBorder.BorderBrush = new SolidColorBrush(Colors.Black);
                    break;

            }
        }
    }
}
