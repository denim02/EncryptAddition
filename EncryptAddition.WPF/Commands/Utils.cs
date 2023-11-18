using System.Linq;
using System.Numerics;

namespace EncryptAddition.WPF.Commands
{
    public static class Utils
    {
        public static BigInteger[] ParseStringToBigInteger(string input)
        {
            return input.Split(',')
                .Select(part => BigInteger.TryParse(part.Trim(), out var number) ? number : BigInteger.Zero)
                .ToArray();
        }
    }
}
