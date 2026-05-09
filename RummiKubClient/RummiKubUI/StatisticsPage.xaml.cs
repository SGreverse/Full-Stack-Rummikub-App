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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RummiKubUI.ServiceReference1;

namespace RummiKubUI
{
    /// <summary>
    /// Interaction logic for StatisticsPage.xaml
    /// </summary>
    public partial class StatisticsPage : Page
    {

        private const double ScrollAmount = 5;
        private readonly DoubleAnimation scrollAnimation;
        private readonly Storyboard storyboard;
        public StatisticsPage()
        {
            InitializeComponent();
            scrollAnimation = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromMilliseconds(300)), 
                EasingFunction = new QuadraticEase() { EasingMode = EasingMode.EaseOut }
            };

            storyboard = new Storyboard();
            Storyboard.SetTarget(scrollAnimation, this);
            Storyboard.SetTargetProperty(scrollAnimation, new PropertyPath("VerticalOffset"));
        }


        public StatisticsPage(Player player) : this()
        {
            if (player != null)
            {
                Score.Text = player.Score.ToString();
                Level.Text = player.Level.ToString();
                Total_Exp.Text = player.ExpierincePoints.ToString();
                Games_Won.Text = player.GamesWon.ToString();
                Games_Played.Text = player.GamesPlayed.ToString();
                Winning_ratio.Text = player.GamesPlayed == 0 ? "0%" : $"{(int)(player.GamesWon / (double)player.GamesPlayed * 100)}%";
            }
            else
            {
                Score.Text = Level.Text = Total_Exp.Text = Games_Won.Text = Games_Played.Text = Winning_ratio.Text = "N/A";
            }
        }
        private void ScrollUpButton_Click(object sender, RoutedEventArgs e)
        {
            double targetOffset = scrollViewer.VerticalOffset - ScrollAmount;
            AnimateScroll(targetOffset);
        }

        private void ScrollDownButton_Click(object sender, RoutedEventArgs e)
        {
            double targetOffset = scrollViewer.VerticalOffset + ScrollAmount;
            AnimateScroll(targetOffset);
        }

        private void AnimateScroll(double targetOffset)
        {
            scrollAnimation.To = targetOffset;
            storyboard.Completed += (s, e) => scrollViewer.ScrollToVerticalOffset(targetOffset);

            storyboard.Begin();
        }
        private void ScrollViewer_PreviewMouseWheel(object sender,MouseWheelEventArgs e)
        {
            e.Handled = true;
        }
    }
}
