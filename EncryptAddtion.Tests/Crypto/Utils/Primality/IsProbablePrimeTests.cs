﻿using EncryptAddition.Crypto.Utils;

namespace EncryptAddtion.Tests.Crypto.Utils
{
    [TestClass]
    public class IsProbablePrimeTests
    {

        #region Trivial Cases
        [TestMethod]
        public void IsProbablePrime_ZeroAndOne()
        {
            int[] nonPrimes = { 0, 1 };
            Assert.IsFalse(nonPrimes.Any(candidate => Primality.IsProbablePrime(candidate)));
        }

        [TestMethod]
        public void IsProbablePrime_TwoAndThree()
        {
            int[] primes = { 2, 3 };
            Assert.IsTrue(primes.All(candidate => Primality.IsProbablePrime(candidate)));
        }

        [TestMethod]
        public void IsProbablePrime_ArgumentOutOfRange()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => Primality.IsProbablePrime(1, 0));
        }

        [TestMethod]
        public void IsProbablePrime_RangeOfPrimeNegativeValues()
        {
            int[] nonPrimes = { -2, -3, -5, -7, -11, -13, -17, -19, -23, -29 };
            Assert.IsFalse(nonPrimes.Any(candidate => Primality.IsProbablePrime(candidate)));
        }

        [TestMethod]
        public void IsProbablePrime_RangeOfCompositeNegativeValues()
        {
            int[] nonPrimes = { -4, -6, -8, -9, -10, -12, -14, -15, -16 };
            Assert.IsFalse(nonPrimes.Any(candidate => Primality.IsProbablePrime(candidate)));
        }

        [TestMethod]
        public void IsProbablePrime_RangeOfEvenValues()
        {
            int[] nonPrimes = { 4, 6, 8, 10, 12, 14, 16 };
            Assert.IsFalse(nonPrimes.Any(candidate => Primality.IsProbablePrime(candidate)));
        }
        #endregion

        #region Larger Cases
        [TestMethod]
        public void IsProbablePrime_RangeOfPrimesFromTwoToThirty()
        {
            int[] primes = { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29 };

            Assert.IsTrue(primes.All(candidate => Primality.IsProbablePrime(candidate)));
        }

        [TestMethod]
        public void IsProbablePrime_PrimesFrom10To30Digits()
        {
            BigInteger[] primes =
            {
                new(9576890767), new(3628273133),   // 10-digit
                BigInteger.Parse("48112959837082048697"),   // 20-digit
                BigInteger.Parse("54673257461630679457"),
                BigInteger.Parse("671998030559713968361666935769"),   // 30-digit
                BigInteger.Parse("671998030559713968361666935769")
            };

            Assert.IsTrue(primes.All(candidate => Primality.IsProbablePrime(candidate)));
        }

        [TestMethod]
        public void IsProbablePrime_CompositesFrom10To30Digits()
        {
            BigInteger[] composites =
            {
                new(3994795224), new(8345018695),   // 10-digit
                BigInteger.Parse("61056461902374819960"),   // 20-digit
                BigInteger.Parse("16599146813000153465"),
                BigInteger.Parse("938849082853396730084751520432"),   // 30-digit
                BigInteger.Parse("547199442217976879844192511900"),
            };

            Assert.IsFalse(composites.Any(candidate => Primality.IsProbablePrime(candidate)));
        }
        #endregion
    }
}
