using EncryptAddition.Crypto;
using System;
using System.Globalization;
using System.Windows.Data;

namespace EncryptAddition.WPF.Converters
{
    public class StringToEncryptionChoiceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is EncryptionChoice choice)
            {
                switch (choice)
                {
                    case EncryptionChoice.Paillier:
                        return "Paillier";
                    case EncryptionChoice.ElGamal:
                        return "ElGamal";
                    default:
                        return null;
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string content)
            {
                switch (content)
                {
                    case "Paillier":
                        return EncryptionChoice.Paillier;
                    case "ElGamal":
                        return EncryptionChoice.ElGamal;
                    default:
                        return null;
                }
            }
            return null;
        }
    }
}