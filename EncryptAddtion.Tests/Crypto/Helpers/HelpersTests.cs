//using System;
//using System.Numerics;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using EncryptAddition.Crypto;

//namespace EncryptAddition.Tests
//{
//    [TestClass]
//    public class HelpersTests
//    {
//        [TestMethod]
//        public void TestNextBigInteger()
//        {

//        }

//        [TestMethod]
//        [DataRow(3)]
//        [DataRow(2)]
//        [DataRow(11)]
//        [DataRow(1000000007)]
//        [DataRow(500000003)]
//        public void TestIsProbablyPrime(Int32 candidate)
//        {
//            BigInteger possiblePrime = new(candidate);
//            Assert.IsTrue(Helpers.IsProbablePrime(possiblePrime, 10));
//        }

//        [TestMethod]
//        [DataRow(1)]
//        [DataRow(9)]
//        [DataRow(1054020324)]
//        [DataRow(203020)]
//        public void TestIsNotProbablyPrime(Int32 candidate)
//        {
//            BigInteger possiblePrime = new(candidate);
//            Assert.IsFalse(Helpers.IsProbablePrime(candidate, 10));
//        }


//        [TestMethod]
//        public void TestFindGenerator()
//        {
//            // Arrange
//            BigInteger safePrime = BigInteger.Parse("104729"); // Example safe prime

//            // Act
//            BigInteger generator = Helpers.FindGeneratorForSafePrime(safePrime);

//            // Assert
//            Assert.IsTrue(generator > BigInteger.Zero);
//            Assert.IsTrue(generator < safePrime);
//            Assert.IsTrue(BigInteger.ModPow(generator, 2, safePrime) != BigInteger.One);
//            Assert.IsTrue(BigInteger.ModPow(generator, (safePrime - 1) / 2, safePrime) != BigInteger.One);
//        }

//        [TestMethod]
//        public void TestGenerateSafePrime()
//        {
//            // Arrange
//            int bitLength = 128; // Example bit length

//            // Act
//            BigInteger safePrime = Helpers.GenerateSafePrime(bitLength);

//            // Assert
//            Assert.IsTrue(Helpers.IsProbablyPrime(safePrime));
//            Assert.IsTrue(safePrime == 2 * Helpers.GeneratePrime(bitLength) + 1);
//        }

//        [TestMethod]
//        public void TestGeneratePrime()
//        {
//            // Arrange
//            int bitLength = 128; // Example bit length

//            // Act
//            BigInteger prime = Helpers.GeneratePrime(bitLength);

//            // Assert
//            Assert.IsTrue(Helpers.IsProbablyPrime(prime));
//        }

//        [TestMethod]
//        public void TestModInverse()
//        {
//            // Arrange
//            BigInteger value = BigInteger.Parse("17"); // Example value
//            BigInteger mod = BigInteger.Parse("23");   // Example modulus

//            // Act
//            BigInteger inverse = Helpers.ModInverse(value, mod);

//            // Assert
//            Assert.AreEqual(BigInteger.Multiply(value, inverse) % mod, BigInteger.One);
//        }
//    }
//}