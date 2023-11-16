using System.Globalization;
using System.Windows.Controls;

namespace EncryptAddition.WPF.ViewModels.ValidationRules
{
    public class BitLengthRangeRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is string stringValue && int.TryParse(stringValue, out int result))
            {
                if (result > 2)
                {
                    return ValidationResult.ValidResult;
                }
                else
                {
                    return new ValidationResult(false, "The bit length value must be a positive integer greater than 2.");
                }
            }
            return new ValidationResult(false, "Invalid input. The bit length must be an integer greater than 2.");
        }

    }
}
