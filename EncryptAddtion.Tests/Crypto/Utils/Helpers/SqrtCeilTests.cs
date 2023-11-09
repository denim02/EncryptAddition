using EncryptAddition.Crypto.Utils;

namespace EncryptAddtion.Tests.Crypto.Utils
{
    [TestClass]
    public class SqrtCeilTests
    {
        #region Trivial Cases

        [TestMethod]
        public void SqrtCeil_NegativeNumber_ThrowsArgumentOutOfRangeException()
        {
            BigInteger negativeNumber = -1;
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => Helpers.SqrtCeil(negativeNumber));
        }

        [TestMethod]
        public void SqrtCeil_Zero()
        {
            BigInteger number = 0;
            BigInteger expected = 0;
            BigInteger actual = Helpers.SqrtCeil(number);
            Assert.AreEqual(expected, actual, "The square root of 0 should be 0.");
        }

        [TestMethod]
        public void SqrtCeil_One()
        {
            BigInteger number = 1;
            BigInteger expected = 1;
            BigInteger actual = Helpers.SqrtCeil(number);
            Assert.AreEqual(expected, actual, "The square root of 1 should be 1.");
        }

        [TestMethod]
        public void SqrtCeil_SmallerOrEqualTo4()
        {
            Assert.AreEqual(new BigInteger(2), Helpers.SqrtCeil(3), "The ceiling square root of 3 should be 2.");
            Assert.AreEqual(new BigInteger(2), Helpers.SqrtCeil(4), "The ceiling square root of 4 should be 2.");
        }

        [TestMethod]
        public void SqrtCeil_SmallerOrEqualTo9()
        {
            Assert.AreEqual(new BigInteger(3), Helpers.SqrtCeil(8), "The square root ceiling of 8 should be 3.");
            Assert.AreEqual(new BigInteger(3), Helpers.SqrtCeil(9), "The square root ceiling of 9 should be 3.");
        }

        [TestMethod]
        public void SqrtCeil_NumbersUnderOrEqualTo20()
        {
            BigInteger[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 };
            BigInteger[] sqRoots = { 1, 2, 2, 2, 3, 3, 3, 3, 3, 4, 4, 4, 4, 4, 4, 4, 5, 5, 5, 5 };

            for (var i = 0; i < numbers.Length; i++)
            {
                Assert.AreEqual(sqRoots[i], Helpers.SqrtCeil(numbers[i]), $"The square root ceiling of {numbers[i]} should be {sqRoots[i]}");
            }
        }
        #endregion

        #region Complex Cases
        [TestMethod]
        public void Sqrt_LargeNonPerfectSquare_ReturnsCorrectCeiling()
        {
            BigInteger number = new BigInteger(1234567890);
            BigInteger expected = 35137; // Ceiling of the true square root
            BigInteger actual = Helpers.SqrtCeil(number);
            Assert.AreEqual(expected, actual, "The square root ceiling of 1234567890 should be 35137.");
        }

        [TestMethod]
        public void Sqrt_LargePerfectSquare_ReturnsExactRoot()
        {
            BigInteger number = new BigInteger(1234567890) * new BigInteger(1234567890);
            BigInteger expected = 1234567890;
            BigInteger actual = Helpers.SqrtCeil(number);
            Assert.AreEqual(expected, actual, "The square root ceiling of the square of 1234567890 should be 1234567890.");
        }

        [TestMethod]
        public void Sqrt_VeryLargeNumber_ReturnsCorrectCeiling()
        {
            BigInteger number = BigInteger.Pow(new BigInteger(int.MaxValue), 10);
            BigInteger expected = BigInteger.Pow(new BigInteger(int.MaxValue), 5);
            BigInteger actual = Helpers.SqrtCeil(number);
            Assert.AreEqual(expected, actual, "The square root ceiling of int.MaxValue^10 should be int.MaxValue^5.");
        }
        #endregion
    }
}
