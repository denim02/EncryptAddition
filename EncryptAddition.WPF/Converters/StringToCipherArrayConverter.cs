using EncryptAddition.Crypto;
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace EncryptAddition.WPF.Converters
{
    public class StringToCipherArrayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var inputString = value as string;
            if (string.IsNullOrWhiteSpace(inputString))
                return Array.Empty<CipherText>();

            return inputString.Split(',')
                              .Select(part => CipherText.Deserialize(part.Trim()))
                              .ToArray();
        }
    }
}
