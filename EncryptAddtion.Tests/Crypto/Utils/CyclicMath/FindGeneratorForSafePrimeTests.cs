using EncryptAddition.Crypto.Utils;

namespace EncryptAddtion.Tests.Crypto.Utils
{
    [TestClass]
    public class FindGeneratorForSafePrimeTests
    {
        [TestMethod]
        public void FindGeneratorForSafePrime_ArgumentOutOfRange()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => CyclicMath.FindGeneratorForSafePrime(0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => CyclicMath.FindGeneratorForSafePrime(-1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => CyclicMath.FindGeneratorForSafePrime(4));
        }

        [TestMethod]
        public void FindGeneratorForSafePrime_Prime5()
        {
            // Generators for the multiplicative group modulo 5 are 2 and 3
            // Since this method finds them randomly, it may be either one.
            BigInteger gen = CyclicMath.FindGeneratorForSafePrime(5);

            Assert.IsTrue(gen == 2 || gen == 3);
        }

        [TestMethod]
        public void FindGeneratorForSafePrime_Prime23()
        {
            // Create list which contains generators for 23.
            BigInteger[] options = { 5, 7, 10, 11, 14, 15, 17, 19, 20, 21 };

            BigInteger gen = CyclicMath.FindGeneratorForSafePrime(23);

            Assert.IsTrue(options.Contains(gen), $"{gen}");
        }

        [TestMethod]
        public void FindGeneratorForSafePrime_Prime167()
        {
            // Create list which contains generators for 167.
            BigInteger[] options = { 5, 10, 13, 15, 17, 20, 23, 26, 30, 34, 35, 37,
                39, 40, 41, 43, 45, 46, 51, 52, 53, 55, 59, 60, 67, 68, 69, 70, 71,
                73, 74, 78, 79, 80, 82, 83, 86, 90, 91, 92, 95, 101, 102, 103, 104,
                105, 106, 109, 110, 111, 113, 117, 118, 119, 120, 123, 125, 129, 131,
                134, 135, 136, 138, 139, 140, 142, 143, 145, 146, 148, 149, 151, 153,
                155, 156, 158, 159, 160, 161, 163, 164, 165 };

            BigInteger gen = CyclicMath.FindGeneratorForSafePrime(167);

            Assert.IsTrue(options.Contains(gen), $"{gen}");
        }
    }
}
