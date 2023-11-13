using EncryptAddition.Analysis.Benchmarking;
using EncryptAddition.Crypto;
using System;
using System.Globalization;
using System.Windows.Data;

namespace EncryptAddition.WPF.Converters
{
    public class ResultsTupleToRecordConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var tuple = value as (BenchmarkResult, CipherText[])?;

            return tuple?.Item1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}