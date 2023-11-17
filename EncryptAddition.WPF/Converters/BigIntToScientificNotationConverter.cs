using System;
using System.Globalization;
using System.Numerics;
using System.Windows.Data;

namespace EncryptAddition.WPF.Converters
{
    public class BigIntToScientificNotationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is BigInteger bigInt)
            {
                // If greater than 9 digits, write in scientific notation, cap to 4 decimal places
                return bigInt.ToString().Length > 9 ? $"{bigInt:0.0000e+0}" : $"{bigInt}";
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}