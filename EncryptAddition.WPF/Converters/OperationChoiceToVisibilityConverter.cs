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
            if (value == null)
                return Visibility.Collapsed;

            if (value is OperationChoice choice)
            {
                if (parameter == null)
                    return Visibility.Collapsed;
                if (parameter is string operation)
                {
                    if (choice == OperationChoice.ENCRYPTION && operation == "ENCRYPTION")
                        return Visibility.Visible;
                    else if (choice == OperationChoice.DECRYPTION && operation == "DECRYPTION")
                        return Visibility.Visible;
                    else
                        return Visibility.Collapsed;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
