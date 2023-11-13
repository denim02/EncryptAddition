using System;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Windows.Data;

namespace EncryptAddition.WPF.Converters
{
    public class StringToBigIntegerArrayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var inputString = value as string;

            if (string.IsNullOrWhiteSpace(inputString))
                return Array.Empty<BigInteger>();

            return inputString.Split(',')
                              .Select(part => BigInteger.TryParse(part.Trim(), out var number) ? number : BigInteger.Zero)
                              .ToArray();
        }
    }
}
