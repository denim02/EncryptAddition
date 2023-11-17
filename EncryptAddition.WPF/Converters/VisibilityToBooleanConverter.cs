using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace EncryptAddition.WPF.Converters
{
    public class VisibilityToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility visibility)
            {
                if (parameter is string option && option == "NEGATE")
                    return (visibility != Visibility.Visible);
                return (visibility == Visibility.Visible);
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
