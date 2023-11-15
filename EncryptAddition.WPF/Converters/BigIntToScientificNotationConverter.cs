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
                // If greater than 9 digits, write in scientific notation
                if (bigInt.ToString().Length > 9)
                {
                    return bigInt.ToString("E");
                }
                else
                {
                    return bigInt.ToString();
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