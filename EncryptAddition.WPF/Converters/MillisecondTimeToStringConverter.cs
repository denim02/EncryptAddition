using System;
using System.Globalization;
using System.Windows.Data;

namespace EncryptAddition.WPF.Converters
{
    public class MillisecondTimeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double time)
            {
                // Process time (in case it is greater than 1000ms, express in seconds)
                return time > 1000 ? $"{time / 1000:0.00000} s" : $"{time:0.00000} ms";
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}