using EncryptAddition.Crypto.Utils;

namespace EncryptAddtion.Tests.Crypto.Utils
{
    [TestClass]
    public class ModMulTests
    {
        #region Test Cases
        [TestMethod]
        public void ModMul_ArgumentOutOfRange()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => Helpers.ModMul(2, 3, -1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => Helpers.ModMul(2, 3, 0));
        }

        [TestMethod]
        public void ModMul_TwoAndThreeMod7()
        {
            Assert.AreEqual(Helpers.ModMul(2, 3, 7), new BigInteger(6));
        }

        [TestMethod]
        public void ModMul_ThreeAndThreeMod7()
        {
            Assert.AreEqual(Helpers.ModMul(3, 3, 7), new BigInteger(2));
        }

        [TestMethod]
        public void ModMul_EightAndThreeMod7()
        {
            Assert.AreEqual(Helpers.ModMul(8, 3, 7), new BigInteger(3));
        }

        [TestMethod]
        public void ModMul_LargeValues()
        {
            BigInteger operand1 = 200000, operand2 = 300000, modulus = 11;

            // Amounts to be 9 * 8 mod 11 = 6
            Assert.AreEqual(Helpers.ModMul(operand1, operand2, modulus), new BigInteger(6));
        }

        [TestMethod]
        public void ModMul_ResultIsZero()
        {
            Assert.AreEqual(Helpers.ModMul(100, 200, 10), BigInteger.Zero);
        }
        #endregion
    }
}
