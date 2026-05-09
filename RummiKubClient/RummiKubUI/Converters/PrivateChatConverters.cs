using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using System.Drawing;
using System.Windows.Media;
using RummiKubUI.ServiceReference1;

namespace RummiKubUI.Converters
{
    public class BubbleBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value== MainWindow.CurrentUser.Id ?
              new SolidColorBrush(Colors.AliceBlue) :
                new SolidColorBrush(Colors.White);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class AlignmentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value == MainWindow.CurrentUser.Id ?
                HorizontalAlignment.Right :
                HorizontalAlignment.Left;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class TextColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value == MainWindow.CurrentUser.Id ?
                new SolidColorBrush(Colors.Black) :
                new SolidColorBrush(Colors.Black);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class EditButtonConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Message message = value as Message;
            if(message == null)
            {
                return Visibility.Collapsed;
            }
            if( message.SenderId!=MainWindow.CurrentUser.Id || (DateTime.Now - message.SendingTime).TotalHours > 3)
            {
                return Visibility.Collapsed;
            }
            return Visibility.Visible;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class DeleteButtonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Message message = value as Message;
            if (message == null)
            {
                return Visibility.Collapsed;
            }
            if (message.SenderId != MainWindow.CurrentUser.Id)
            {
                return Visibility.Collapsed;
            }
            return Visibility.Visible;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class MessageDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime date = (DateTime)value;
            if (date.Date == DateTime.Now.Date)
            {
                return date.ToString("HH:mm");
            }
            if ((DateTime.Today.AddDays(-1).Date==date.Date))
            {
                return $"Yesterday At {date.ToString("HH:mm")}";
            }
            if ((DateTime.Now.Date - date.Date).Days < 7)
            {
                return $"{date.ToString("HH:mm")} {(DateTime.Now.Date - date.Date).Days} Days Ago";
            }
            if((DateTime.Now.Year==date.Year))
            {
                return $"{date.ToString("MM-dd HH:mm")}";
            }
            return $"{date.ToString("yyyy-MM-dd HH:mm")}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
