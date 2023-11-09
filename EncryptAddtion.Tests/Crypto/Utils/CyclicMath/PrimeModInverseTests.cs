using EncryptAddition.Crypto.Utils;

namespace EncryptAddtion.Tests.Crypto.Utils
{
    [TestClass]
    public class PrimeModInverseTests
    {
        #region Trivial Cases
        [TestMethod]
        public void PrimeModInverse_ArgumentOutOfRange()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => CyclicMath.PrimeModInverse(3, 0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => CyclicMath.PrimeModInverse(3, -1));
        }

        [TestMethod]
        public void PrimeModInverse_ArgumentException()
        {
            // The modulus must be prime.
            Assert.ThrowsException<ArgumentException>(() => CyclicMath.PrimeModInverse(3, 4));
        }

        [TestMethod]
        public void PrimeModInverse_NotCoprime()
        {
            Assert.AreEqual(CyclicMath.PrimeModInverse(14, 7), BigInteger.MinusOne);
            Assert.AreEqual(CyclicMath.PrimeModInverse(39, 13), BigInteger.MinusOne);
            Assert.AreEqual(CyclicMath.PrimeModInverse(6, 3), BigInteger.MinusOne);
        }

        [TestMethod]
        public void PrimeModInverse_Modulus5()
        {
            Assert.AreEqual(CyclicMath.PrimeModInverse(3, 5), new BigInteger(2));
            Assert.AreEqual(CyclicMath.PrimeModInverse(4, 5), new BigInteger(4));
            Assert.AreEqual(CyclicMath.PrimeModInverse(2, 5), new BigInteger(3));
        }

        [TestMethod]
        public void PrimeModInverse_Modulus11()
        {
            Assert.AreEqual(CyclicMath.PrimeModInverse(3, 11), new BigInteger(4));
            Assert.AreEqual(CyclicMath.PrimeModInverse(5, 11), new BigInteger(9));
            Assert.AreEqual(CyclicMath.PrimeModInverse(7, 11), new BigInteger(8));
            Assert.AreEqual(CyclicMath.PrimeModInverse(24, 11), new BigInteger(6));
        }

        [TestMethod]
        public void PrimeModInverse_Modulus113()
        {
            Assert.AreEqual(CyclicMath.PrimeModInverse(87, 113), new BigInteger(13));
            Assert.AreEqual(CyclicMath.PrimeModInverse(54, 113), new BigInteger(90));
            Assert.AreEqual(CyclicMath.PrimeModInverse(367, 113), new BigInteger(109));
            Assert.AreEqual(CyclicMath.PrimeModInverse(234, 113), new BigInteger(99));
        }
        #endregion

        #region Random Cases
        [TestMethod]
        public void PrimeModInverse_16BitPrime()
        {
            BigInteger modulus = Primality.GeneratePrime(16);
            BigInteger value = Helpers.GetBigInteger(2, modulus - 1);
            BigInteger modularInverse = CyclicMath.PrimeModInverse(value, modulus);

            // a * (a^-1) = 1
            Assert.AreEqual(Helpers.ModMul(value, modularInverse, modulus), BigInteger.One);
        }

        [TestMethod]
        public void PrimeModInverse_32BitPrime()
        {
            BigInteger modulus = Primality.GeneratePrime(32);
            BigInteger value = Helpers.GetBigInteger(2, modulus - 1);
            BigInteger modularInverse = CyclicMath.PrimeModInverse(value, modulus);

            // a * (a^-1) = 1
            Assert.AreEqual(Helpers.ModMul(value, modularInverse, modulus), BigInteger.One);
        }
        #endregion
    }
}
