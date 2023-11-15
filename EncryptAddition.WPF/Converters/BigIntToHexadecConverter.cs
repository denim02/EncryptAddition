using System;
using System.Globalization;
using System.Numerics;
using System.Windows.Data;

namespace EncryptAddition.WPF.Converters
{
    public class BigIntToHexadecConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is BigInteger bigInt)
            {
                // If greater than 9 digits, write in hexadecimal notation
                if (bigInt.ToString().Length > 9)
                {
                    return $"{bigInt:X}";
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