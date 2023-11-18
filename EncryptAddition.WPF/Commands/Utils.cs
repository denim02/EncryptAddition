using EncryptAddition.Crypto;
using System.Linq;
using System.Numerics;

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
    }
}
