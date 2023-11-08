namespace EncryptAddtion.Tests
{
    [TestClass]
    public class NextBigIntegerTests
    {
        #region Trivial Cases
        [TestMethod]
        public void NextBigInteger_MinBoundGreaterThanMax()
        {
            Assert.ThrowsException<ArgumentException>(() =>
            {
                CyclicMath.NextBigInteger(new BigInteger(2000), new BigInteger(1000));
            });
        }

        [TestMethod]
        public void NextBigInteger_MinBoundEqualToMax()
        {
            Assert.AreEqual(new BigInteger(1000), CyclicMath.NextBigInteger(new BigInteger(1000), new BigInteger(1000)));
        }

        [TestMethod]
        public void NextBigInteger_NegativeMinBoundValue()
        {
            Assert.ThrowsException<ArgumentException>(() =>
            {
                CyclicMath.NextBigInteger(new BigInteger(-1000), new BigInteger(1000));
            });

        }
        #endregion

        #region Random Cases

        #endregion
    }
}
