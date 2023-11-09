using EncryptAddition.Crypto.Utils;

namespace EncryptAddtion.Tests
{
    [TestClass]
    public class GeneratePrimeTests
    {
        #region Trivial Cases
        [TestMethod]
        public void GeneratePrime_ArgumentOutOfRange()
        {
            // Should throw an exception for bitLength = 1, since the only 1 bit values are 0 and 1,
            // neither of which is prime.
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => Primality.GeneratePrime(1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => Primality.GeneratePrime(0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => Primality.GeneratePrime(-1));
        }

        [TestMethod]
        public void GeneratePrime_2Bits()
        {
            BigInteger prime = Primality.GeneratePrime(2);

            Assert.IsTrue(prime == 2 || prime == 3);
        }

        [TestMethod]
        public void GeneratePrime_3Bits()
        {
            // 3 bit primes include 5, 7
            BigInteger prime = Primality.GeneratePrime(3);

            Assert.IsTrue(prime == 5 || prime == 7);
        }

        [TestMethod]
        public void GeneratePrime_4Bits()
        {
            // 4 bit primes are 11, 13
            BigInteger prime = Primality.GeneratePrime(4);

            Assert.IsTrue(prime == 11 || prime == 13);
        }
        #endregion

        #region Complex Cases
        [TestMethod]
        public void GeneratePrime_BitLength()
        {
            BigInteger prime0 = Primality.GeneratePrime(2);
            BigInteger prime1 = Primality.GeneratePrime(20);
            BigInteger prime2 = Primality.GeneratePrime(64);

            Assert.AreEqual(prime0.GetBitLength(), 2L);
            Assert.AreEqual(prime1.GetBitLength(), 20L);
            Assert.AreEqual(prime2.GetBitLength(), 64L);
        }

        [TestMethod]
        public void GeneratePrime_32bits()
        {
            BigInteger prime = Primality.GeneratePrime(32);
            Assert.AreEqual(prime.GetBitLength(), 32L);

            // Establishes dependency on correctness of IsProbablePrime,
            // use 10 for certainty to guarantee 99.9999% accuracy
            Assert.IsTrue(prime.IsProbablePrime(10));
        }

        [TestMethod]
        public void GeneratePrime_64bits()
        {
            BigInteger prime = Primality.GeneratePrime(64);
            Assert.AreEqual(prime.GetBitLength(), 64L);

            // Establishes dependency on correctness of IsProbablePrime,
            // use 10 for certainty to guarantee 99.9999% accuracy
            Assert.IsTrue(prime.IsProbablePrime(10));
        }

        [TestMethod]
        public void GeneratePrime_128bits()
        {
            BigInteger prime = Primality.GeneratePrime(128);
            Assert.AreEqual(prime.GetBitLength(), 128L);

            // Establishes dependency on correctness of IsProbablePrime,
            // use 10 for certainty to guarantee 99.9999% accuracy
            Assert.IsTrue(prime.IsProbablePrime(10));
        }
        #endregion
    }
}
