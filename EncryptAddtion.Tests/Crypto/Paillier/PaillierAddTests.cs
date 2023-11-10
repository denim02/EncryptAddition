using EncryptAddition.Crypto.Paillier;

namespace EncryptAddtion.Tests.Crypto.Paillier
{
    [TestClass]
    public class PaillierAddTests
    {
        #region Test Cases
        [TestMethod]
        public void PaillierAdd_NoParametersPassed()
        {
            var paillier = new PaillierEncryption(5);

            Assert.ThrowsException<InvalidOperationException>(() => paillier.Add());
        }

        [TestMethod]
        public void PaillierAdd_OneParameterPassed()
        {
            var paillier = new PaillierEncryption(5);
            var cipher = paillier.Encrypt(4);

            Assert.AreEqual(paillier.Add(cipher), cipher);
        }

        [TestMethod]
        public void PaillierAdd_Add3And5()
        {
            var paillier = new PaillierEncryption(6);
            CipherText[] ciphers = { paillier.Encrypt(3), paillier.Encrypt(5) };

            Assert.AreEqual(8, paillier.Decrypt(paillier.Add(ciphers)));
        }

        [TestMethod]
        public void PaillierAdd_AddFourValues()
        {
            var paillier = new PaillierEncryption(16);
            CipherText[] ciphers = { paillier.Encrypt(3), paillier.Encrypt(4), paillier.Encrypt(5), paillier.Encrypt(7) };

            Assert.AreEqual(3 + 4 + 5 + 7, paillier.Decrypt(paillier.Add(ciphers)));
        }
        #endregion
    }
}
