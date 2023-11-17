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
                    case EncryptionChoice.PAILLIER:
                        return "Paillier";
                    case EncryptionChoice.ELGAMAL:
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
                        return EncryptionChoice.PAILLIER;
                    case "ElGamal":
                        return EncryptionChoice.ELGAMAL;
                    default:
                        return null;
                }
            }
            return null;
        }
    }
}