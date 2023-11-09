using EncryptAddition.Crypto.Utils;

namespace EncryptAddtion.Tests.Crypto.Utils
{
    [TestClass]
    public class GetBigIntegerTests
    {
        #region Trivial Cases
        [TestMethod]
        public void GetBigInteger_NegativeMinBound()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => Helpers.GetBigInteger(-1, 2));
        }

        [TestMethod]
        public void GetBigInteger_NegativeMaxBound()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => Helpers.GetBigInteger(1, -2));
        }

        [TestMethod]
        public void GetBigInteger_MinBoundGreaterThanMax()
        {
            Assert.ThrowsException<ArgumentException>(() => Helpers.GetBigInteger(2, 1));
        }

        [TestMethod]
        public void GetBigInteger_MinBoundEqualToMax()
        {
            Assert.AreEqual(new BigInteger(1000), Helpers.GetBigInteger(new BigInteger(1000), new BigInteger(1000)));
        }

        [TestMethod]
        public void GetBigInteger_Inclusivity()
        {
            // Must be either 1 or 2 if inclusive
            BigInteger random0 = Helpers.GetBigInteger(1, 2);

            // Must be either 2000 or 2001 if inclusive
            BigInteger random1 = Helpers.GetBigInteger(2000, 2001);

            // Must be either 400000, 400001, or 400002 if inclusive
            BigInteger random2 = Helpers.GetBigInteger(400000, 400002);

            Assert.IsTrue(random0 == 1 || random0 == 2);
            Assert.IsTrue(random1 == 2000 || random1 == 2001);
            Assert.IsTrue(random2 == 400000 || random2 == 400001 || random2 == 400002);
        }
        #endregion

        #region Random Cases
        [TestMethod]
        public void GetBigInteger_10Digits()
        {
            BigInteger minBound = 1000000000, maxBound = 9000000000;
            BigInteger value = Helpers.GetBigInteger(minBound, maxBound);

            Assert.IsTrue(minBound <= value && value <= maxBound);
        }

        [TestMethod]
        public void GetBigInteger_20Digits()
        {
            BigInteger minBound = BigInteger.Parse("10000000000000000000"), maxBound = BigInteger.Parse("90000000000000000000");
            BigInteger value = Helpers.GetBigInteger(minBound, maxBound);

            Assert.IsTrue(minBound <= value && value <= maxBound);
        }
        #endregion
    }
}
