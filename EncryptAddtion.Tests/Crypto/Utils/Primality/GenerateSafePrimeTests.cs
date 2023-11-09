using EncryptAddition.Crypto.Utils;

namespace EncryptAddtion.Tests
{
    [TestClass]
    public class GenerateSafePrimeTests
    {
        #region Trivial Cases
        [TestMethod]
        public void GenerateSafePrime_ArgumentOutOfRange()
        {
            // The first safe prime, 5 is 3 bits long so the minimum bit length is 3.
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => Primality.GenerateSafePrime(-1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => Primality.GenerateSafePrime(0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => Primality.GenerateSafePrime(1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => Primality.GenerateSafePrime(2));
        }

        [TestMethod]
        public void GenerateSafePrime_3Bits()
        {
            // 5 and 7 are 3 bit safe primes
            BigInteger safePrime = Primality.GenerateSafePrime(3);

            Assert.IsTrue(safePrime == 5 || safePrime == 7);
        }

        [TestMethod]
        public void GenerateSafePrime_4Bits()
        {
            // 11 is the only 4 bit safe prime
            Assert.IsTrue(Primality.GenerateSafePrime(4) == 11);
        }

        [TestMethod]
        public void GenerateSafePrime_5Bits()
        {
            // 23 is the only 5 bit safe prime
            Assert.IsTrue(Primality.GenerateSafePrime(5) == 23);
        }

        [TestMethod]
        public void GenerateSafePrime_6Bits()
        {
            // Both 47 and 59 are 6 bit safe primes
            BigInteger safePrime = Primality.GenerateSafePrime(6);

            Assert.IsTrue(safePrime == 47 || safePrime == 59);
        }
        #endregion

        #region Complex Cases
        [TestMethod]
        public void GenerateSafePrime_BitLength()
        {
            BigInteger safePrime0 = Primality.GenerateSafePrime(3);
            BigInteger safePrime1 = Primality.GenerateSafePrime(10);
            BigInteger safePrime2 = Primality.GenerateSafePrime(32);
            BigInteger safePrime3 = Primality.GenerateSafePrime(64);

            Assert.AreEqual(safePrime0.GetBitLength(), 3L);
            Assert.AreEqual(safePrime1.GetBitLength(), 10L);
            Assert.AreEqual(safePrime2.GetBitLength(), 32L);
            Assert.AreEqual(safePrime3.GetBitLength(), 64L);
        }

        [TestMethod]
        public void GenerateSafePrime_32Bits()
        {
            BigInteger candidate = Primality.GenerateSafePrime(32);

            // Safe prime is one of the form 2p + 1 where p is also prime, therefore
            // check if it fulfills these conditions
            BigInteger sophieGermainPrime = (candidate - 1) / 2;
            Assert.IsTrue(candidate.IsProbablePrime() && sophieGermainPrime.IsProbablePrime());

            Assert.AreEqual(candidate.GetBitLength(), 32L);
        }

        [TestMethod]
        public void GenerateSafePrime_64Bits()
        {
            BigInteger candidate = Primality.GenerateSafePrime(64);

            // Safe prime is one of the form 2p + 1 where p is also prime, therefore
            // check if it fulfills these conditions
            BigInteger sophieGermainPrime = (candidate - 1) / 2;
            Assert.IsTrue(candidate.IsProbablePrime() && sophieGermainPrime.IsProbablePrime());

            Assert.AreEqual(candidate.GetBitLength(), 64L);
        }
        #endregion
    }
}
