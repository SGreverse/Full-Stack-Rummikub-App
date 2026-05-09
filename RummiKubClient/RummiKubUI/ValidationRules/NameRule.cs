using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RummiKubUI.ValidationRules
{
    public class NameRule: ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string name = value as string;
            if (string.IsNullOrWhiteSpace(name))
            {
                return new ValidationResult(false, "Name cannot be empty.");
            }
            return ValidationResult.ValidResult;
        }
    }
}
