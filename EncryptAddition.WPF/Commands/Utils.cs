using EncryptAddition.Crypto;
using EncryptAddition.Crypto.Exceptions;
using System;
using System.Linq;
using System.Numerics;
using System.Windows;

namespace EncryptAddition.WPF.Commands
{
    public static class Utils
    {
        public static BigInteger[] ParseStringToBigIntegerArray(string input)
        {
            return input.Split(',')
                .Select(part => BigInteger.TryParse(part.Trim(), out var number) ? number : BigInteger.Zero)
                .ToArray();
        }

        public static CipherText[] ParseStringToCipherTextArray(string input)
        {
            return input.Split(',')
                .Select(part => CipherText.Deserialize(part.Trim()))
                .ToArray();
        }

        internal static void HandleBenchmarkException(Exception ex)
        {

            string errorMessage;

            if (ex is EncryptionOverflowException encryptionException)
            {
                errorMessage = encryptionException.Message;
            }
            else if (ex is InvalidDecryptionException decryptionException)
            {
                errorMessage = decryptionException.Message;
            }
            else if (ex is InvalidKeyPairException invalidKeyPairException)
            {
                errorMessage = invalidKeyPairException.Message;
            }
            else
            {
                errorMessage = ex.ToString();
            }

            MessageBox.Show(errorMessage);
        }
    }
}
