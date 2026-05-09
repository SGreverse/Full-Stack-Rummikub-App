using RummiKubUI.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;


namespace RummiKubUI.Converters
{
    public class PositionToColorConverter :  IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int position = (int)value;
            switch (position)
            {
                case 1:
                    return new SolidColorBrush(Color.FromRgb(255, 215, 0)); // Gold
                case 2:
                    return new SolidColorBrush(Color.FromRgb(192, 192, 192)); // Silver
                case 3:
                    return new SolidColorBrush(Color.FromRgb(205, 127, 50)); // Bronze
                default:
                    return new SolidColorBrush(Color.FromRgb(30, 61, 89)); // Dark blue
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    // Converter to show medals only for positions 1, 2, and 3
    public class PositionToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int position = (int)value;
            int targetPosition = int.Parse(parameter.ToString());

            return position == targetPosition ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    // Converter for highlighting the current user's row
    public class BoolToHighlightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool isCurrentUser = (bool)value;
            string highlightColor = parameter.ToString();

            return isCurrentUser
                ? new SolidColorBrush((Color)ColorConverter.ConvertFromString(highlightColor))
                : new SolidColorBrush(Colors.White);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    // Converter for border color when highlighting current user
    public class BoolToBorderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool isCurrentUser = (bool)value;
            string borderColor = parameter.ToString();

            return isCurrentUser
                ? new SolidColorBrush((Color)ColorConverter.ConvertFromString(borderColor))
                : new SolidColorBrush(Colors.LightGray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    // Converter for border thickness when highlighting current user
    public class BoolToBorderThicknessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool isCurrentUser = (bool)value;
            int thickness = int.Parse(parameter.ToString());

            return isCurrentUser ? thickness : 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    // Converter for making current user username bold
    public class BoolToFontWeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool isCurrentUser = (bool)value;
            return isCurrentUser ? FontWeights.Bold : FontWeights.Normal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
