using EncryptAddition.Crypto;
using System;
using System.Globalization;
using System.Windows.Data;

namespace EncryptAddition.WPF.Converters
{
    public class CipherArrayWithoutLastValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var array = value as CipherText[];
            if (array != null)
            {
                if (array.Length == 1)
                    return array;
                else
                    return array[..^1];
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}