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
    /// Interaction logic for HeadBackButtonUserControl.xaml
    /// </summary>
    public partial class HeadBackButtonUserControl : UserControl
    {
        public HeadBackButtonUserControl()
        {
            InitializeComponent();
        }
        private void HeadBackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.This_current_window.GameFrame.Navigate(new MainMenuPage());

        }
    }
}
