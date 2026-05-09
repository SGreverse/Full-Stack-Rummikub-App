using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RummiKubUI.ValidationRules
{
        public class EmailRule : ValidationRule
        {
            public override ValidationResult Validate(object value, CultureInfo cultureInfo)
            {
                if(IsValidEmail(value as string))
                {
                    return ValidationResult.ValidResult;
                }
                else
                {
                    return new ValidationResult(false, "Invalid email format. Please enter a valid email address.");
                }
            }
            private bool IsValidEmail(string email)
            {
                string regex = @"^[^@\s]+@[^@\s]+\.(com|net|org|gov|co\.il|gmail\.com)$";
                return Regex.IsMatch(email, regex, RegexOptions.IgnoreCase);
            }
        }
}
