using EncryptAddition.Crypto.ElGamal;

namespace EncryptAddtion.Tests.Crypto.ElGamal
{
    [TestClass]
    public class ElGamalAddTests
    {
        #region Test Cases
        [TestMethod]
        public void ElGamalAdd_NoParametersPassed()
        {
            var elGamal = new ElGamalEncryption(5);

            Assert.ThrowsException<InvalidOperationException>(() => elGamal.Add());
        }

        [TestMethod]
        public void ElGamalAdd_OneParameterPassed()
        {
            var elGamal = new ElGamalEncryption(5);
            var cipher = elGamal.Encrypt(4);

            Assert.AreEqual(elGamal.Add(cipher), cipher);
        }

        [TestMethod]
        public void ElGamalAdd_InvalidCipherTextPassed()
        {
            var elGamal = new ElGamalEncryption(5);
            var cipher = new CipherText(200);

            Assert.ThrowsException<ArgumentException>(() => elGamal.Add(cipher));
        }

        [TestMethod]
        public void ElGamalAdd_Add3And5()
        {
            var elGamal = new ElGamalEncryption(6);
            CipherText[] ciphers = { elGamal.Encrypt(3), elGamal.Encrypt(5) };

            Assert.AreEqual(8, elGamal.Decrypt(elGamal.Add(ciphers)));
        }

        [TestMethod]
        public void ElGamalAdd_AddFourValues()
        {
            var elGamal = new ElGamalEncryption(16);
            CipherText[] ciphers = { elGamal.Encrypt(3), elGamal.Encrypt(4), elGamal.Encrypt(5), elGamal.Encrypt(7) };

            Assert.AreEqual(3 + 4 + 5 + 7, elGamal.Decrypt(elGamal.Add(ciphers)));
        }
        #endregion
    }
}
