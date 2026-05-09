using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Security.Policy;
using System.IO;
using RummiKubUI.ServiceReference1;
namespace RummiKubUI
{
    public class TileConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Tile tile = (Tile)value;
            string imagesource;
            if (tile is Joker)
            {
                if (MainWindow.CurrentUser.Username.ToLower() == "abanatam")
                {
                    imagesource = "MatanJonkler.jpg";
                }
                else
                {
                    imagesource = "joker.png";
                }
            }
            else
            {
                 imagesource = $"Tile{tile.Value}.png";
            }
            string path = Path.Combine(Environment.CurrentDirectory, @"..\..\" + "/Assets/" + imagesource);
            return new BitmapImage(new Uri(path));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;  // not in use
        }
    }
}
