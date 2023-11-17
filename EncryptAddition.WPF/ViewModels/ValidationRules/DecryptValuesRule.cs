using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace EncryptAddition.WPF.ViewModels.ValidationRules
{
    public class DecryptValuesRule : ValidationRule
    {
        private readonly static Regex correctFormat = new(@"^(\d+(\|\d+)?)(\s*,\s*\d+(\|\d+)?)*$");

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is string stringValue && correctFormat.IsMatch(stringValue))
            {
                return ValidationResult.ValidResult;
            }
            else
            {
                return new ValidationResult(false, "The input values must be entered as a comma-separated array of ciphertexts.");
            }
        }
    }
}
