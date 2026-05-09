using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using RummiKubUI.ServiceReference1;

namespace RummiKubUI.Converters
{
    public class UsernameConverter:IValueConverter
    {
         public object Convert(object value, Type targetType,
                              object parameter, CultureInfo culture)
        {
            return ConvertToUserName((string)value);
        }
        public static string ConvertToUserName(string username)
        {
            if (username == null)
                return "None";
            return username;
        }
        public object ConvertBack(object value, Type targetType,
                                  object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
