using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace EncryptAddition.WPF.ViewModels.ValidationRules
{
    public class SerializedCustomKeyRule : ValidationRule
    {
        private readonly Regex _correctFormat = new Regex(@"^\d+\|\d+\|\d+;\d+$|^\d+\|\d+;\d+\|\d+$");

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is string stringValue)
            {
                if (_correctFormat.IsMatch(stringValue))
                    return ValidationResult.ValidResult;
                else
                    return new ValidationResult(false, "Invalid custom key pair format.");
            }

            return new ValidationResult(false, "The custom key pair should be a string value.");
        }
    }
}
