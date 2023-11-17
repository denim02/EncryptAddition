using EncryptAddition.Crypto;
using System;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Windows.Data;

namespace EncryptAddition.WPF.Converters
{
    public class EncryptionResultsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Tuple<object, object>[] results)
            {
                bool isInvalid = false;

                var processedResults = results.Select(pair =>
                {
                    if (pair.Item1 is BigInteger plain && pair.Item2 is CipherText cipher)
                        return new Tuple<string, string>(
                            (plain).ToString(),
                            (cipher).ToString()
                            );
                    else
                    {
                        isInvalid = true;
                        return new Tuple<string, string>("", "");
                    }
                }
                ).ToArray();

                if (isInvalid)
                    return null;
                else
                    return processedResults;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
