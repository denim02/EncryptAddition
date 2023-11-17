using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace EncryptAddition.WPF.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is string option && option == "NEGATE")
                return ((bool)value) == false ? Visibility.Visible : Visibility.Collapsed;
            else
                return ((bool)value) == false ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}