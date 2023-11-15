using EncryptAddition.Analysis.ResultTypes;
using System;
using System.Globalization;
using System.Windows.Data;

namespace EncryptAddition.WPF.Converters
{
    public class BenchmarkTupleToArrayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var tuple = value as Tuple<BenchmarkResult, BenchmarkResult?>;
            if (tuple != null)
            {
                if (tuple.Item2.HasValue)
                    return new BenchmarkResult[] { tuple.Item1, tuple.Item2.Value };
                else
                    return new BenchmarkResult[] { tuple.Item1 };
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}