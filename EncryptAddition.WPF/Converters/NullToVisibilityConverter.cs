using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace EncryptAddition.WPF.Converters
{
    public class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is string option && option == "NEGATE")
                return value == null ? Visibility.Visible : Visibility.Collapsed;
            else
                return value == null ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}