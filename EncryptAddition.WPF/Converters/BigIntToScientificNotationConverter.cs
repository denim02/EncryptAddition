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
                if (bigInt.ToString().Length > 9)
                {
                    return $"{bigInt:0.0000e+0}";
                }
                else
                {
                    return $"{bigInt}";
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}