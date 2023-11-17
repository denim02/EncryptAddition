using EncryptAddition.WPF.DataTypes;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace EncryptAddition.WPF.Converters
{
    public class OperationChoiceToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is OperationChoice choice && parameter is string operation)
            {
                if (choice == OperationChoice.ENCRYPTION && operation == "ENCRYPTION")
                    return Visibility.Visible;
                else if (choice == OperationChoice.DECRYPTION && operation == "DECRYPTION")
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
