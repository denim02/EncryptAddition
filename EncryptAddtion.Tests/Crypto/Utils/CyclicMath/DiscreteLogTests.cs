using EncryptAddition.Crypto.Utils;

namespace EncryptAddtion.Tests.Crypto.Utils
{
    [TestClass]
    public class DiscreteLogTests
    {
        #region Trivial Cases
        [TestMethod]
        public void DiscreteLog_ElementIsOne()
        {
            Assert.AreEqual(CyclicMath.DiscreteLog(3, 1, 5), BigInteger.Zero);
        }

        [TestMethod]
        public void DiscreteLog_ElementEqualsGenerator()
        {
            Assert.AreEqual(CyclicMath.DiscreteLog(3, 3, 5), BigInteger.One);
        }

        [TestMethod]
        public void DiscreteLog_ArgumentOutOfRange()
        {
            // Each parameter must be positive.
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => CyclicMath.DiscreteLog(-1, 3, 5));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => CyclicMath.DiscreteLog(3, -1, 5));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => CyclicMath.DiscreteLog(3, 3, -5));

            // The generator and element must fall within the group.
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => CyclicMath.DiscreteLog(6, 3, 5));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => CyclicMath.DiscreteLog(3, 6, 5));
        }

        [TestMethod]
        public void DiscreteLog_Modulo5()
        {
            BigInteger generator = 3;
            BigInteger power = 4; // 3^2 mod 5
            BigInteger order = 5 - 1;

            Assert.AreEqual(CyclicMath.DiscreteLog(generator, power, order), new BigInteger(2));
        }

        public void DiscreteLog_Modulo13()
        {
            BigInteger generator = 7;
            BigInteger power = 16807; // 7^11 mod 13
            BigInteger order = 13 - 1;

            Assert.AreEqual(CyclicMath.DiscreteLog(generator, power, order), new BigInteger(11));
        }

        public void DiscreteLog_Modulo29()
        {
            BigInteger generator = 26;
            BigInteger power = 5; // 26^10 mod 29
            BigInteger order = 29 - 1;

            Assert.AreEqual(CyclicMath.DiscreteLog(generator, power, order), new BigInteger(10));
        }
        #endregion

        #region Random Cases
        [TestMethod]
        public void DiscreteLog_16BitSafePrimeModulus()
        {
            BigInteger modulus = Primality.GenerateSafePrime(16);
            BigInteger generator = CyclicMath.FindGeneratorForSafePrime(modulus);
            BigInteger power = BigInteger.ModPow(generator, 8, modulus);

            Assert.AreEqual(CyclicMath.DiscreteLog(generator, power, modulus - 1), new BigInteger(8));
        }

        [TestMethod]
        public void DiscreteLog_16BitSafePrimeModulusWithLargeExponent()
        {
            BigInteger modulus = Primality.GenerateSafePrime(16);
            BigInteger generator = CyclicMath.FindGeneratorForSafePrime(modulus);
            BigInteger power = BigInteger.ModPow(generator, modulus - 4, modulus);

            Assert.AreEqual(CyclicMath.DiscreteLog(generator, power, modulus - 1), modulus - 4);
        }
        #endregion
    }
}
