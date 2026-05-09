using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using RummiKubUI.ServiceReference1;

namespace RummiKubUI.ValidationRules   
{
    public class UsernameRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string username = value as string;
            if (string.IsNullOrEmpty(username))
            {
                return new ValidationResult(false, "Must Enter a Username.");
            }
            if (username.Length > 12)
               return new ValidationResult(false, "Username must be less than 13 characters.");

            if (username.Contains(' '))
            {
                return new ValidationResult(false, "Username cannot contain spaces.");
            }
            if (username.Length < 3)
            {
                return new ValidationResult(false, "Username must be at least 3 characters long.");
            }
            return ValidationResult.ValidResult;
        }
    }
}
