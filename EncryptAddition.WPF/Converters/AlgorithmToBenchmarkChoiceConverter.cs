using EncryptAddition.WPF.DataTypes;
using System;
using System.Globalization;
using System.Windows.Data;

namespace EncryptAddition.WPF.Converters
{
    public class AlgorithmToBenchmarkChoiceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is BenchmarkChoice choice)
            {
                switch (choice)
                {
                    case BenchmarkChoice.PAILLIER:
                        return "Paillier";
                    case BenchmarkChoice.ELGAMAL:
                        return "ElGamal";
                    case BenchmarkChoice.COMPARISON:
                        return "Comparison";
                    default:
                        return "Paillier";
                }
            }
            return "Paillier";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string content)
            {
                switch (content)
                {
                    case "Paillier":
                        return BenchmarkChoice.PAILLIER;
                    case "ElGamal":
                        return BenchmarkChoice.ELGAMAL;
                    case "Comparison":
                        return BenchmarkChoice.COMPARISON;
                    default:
                        return "Paillier";
                }
            }
            return "Paillier";
        }
    }
}