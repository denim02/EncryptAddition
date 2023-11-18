using EncryptAddition.Crypto.Exceptions;
using EncryptAddition.Crypto.Paillier;
using EncryptAddition.Crypto.Utils;

namespace EncryptAddtion.Tests.Crypto.Paillier
{
    [TestClass]
    public class PaillierEncryptDecryptTests
    {
        #region Trivial Cases
        // Values will simply be encrypted and then decrypted to see if the same value is found

        [TestMethod]
        public void PaillierEncryptDecrypt_Zero()
        {
            var paillier = new PaillierEncryption(4);
            BigInteger input = 0;
            var cipher = paillier.Encrypt(input);
            var output = paillier.Decrypt(cipher);
            Assert.AreEqual(input, output);
        }

        [TestMethod]
        public void PaillierEncryptDecrypt_One()
        {
            var paillier = new PaillierEncryption(4);
            BigInteger input = 1;
            var cipher = paillier.Encrypt(input);
            var output = paillier.Decrypt(cipher);
            Assert.AreEqual(input, output);
        }

        [TestMethod]
        public void PaillierEncryptDecrypt_Three()
        {
            var paillier = new PaillierEncryption(4);
            BigInteger input = 3;
            var cipher = paillier.Encrypt(input);
            var output = paillier.Decrypt(cipher);
            Assert.AreEqual(input, output);
        }

        [TestMethod]
        public void PaillierEncryptDecrypt_Encrypt_ArgumentOutOfRangeLarger()
        {
            var paillier = new PaillierEncryption(3);
            BigInteger input = paillier.MaxPlaintextSize + 1;
            Assert.ThrowsException<EncryptionOverflowException>(() => paillier.Encrypt(input));
        }

        [TestMethod]
        public void PaillierEncryptDecrypt_Encrypt_ArgumentOutOfRangeNegative()
        {
            var paillier = new PaillierEncryption(3);
            BigInteger input = -1;
            Assert.ThrowsException<EncryptionOverflowException>(() => paillier.Encrypt(input));
        }

        [TestMethod]
        public void PaillierEncryptDecrypt_Encrypt_ArgumentEqualToN()
        {
            var paillier = new PaillierEncryption(5);
            BigInteger input = paillier.KeyPair.PublicKey.N;
            Assert.ThrowsException<EncryptionOverflowException>(() => paillier.Encrypt(input));
        }

        [TestMethod]
        public void PaillierEncryptDecrypt_Encrypt_ArgumentTwoLessThanN()
        {
            var paillier = new PaillierEncryption(5);
            BigInteger input = paillier.KeyPair.PublicKey.N - 2;
            Assert.AreEqual(input, paillier.MaxPlaintextSize);
            Assert.AreEqual(input, paillier.Decrypt(paillier.Encrypt(input)));
        }

        [TestMethod]
        public void PaillierEncryptDecrypt_Decrypt_InvalidCipherText()
        {
            var paillier = new PaillierEncryption(5);
            var cipher = new CipherText(BigInteger.One, BigInteger.One);
            Assert.ThrowsException<InvalidDecryptionException>(() => paillier.Decrypt(cipher));
        }
        #endregion

        #region Complex Cases
        [TestMethod]
        public void PaillierEncryptDecrypt_RandomValues()
        {
            var paillier = new PaillierEncryption(5);

            for (int i = 0; i < 5; i++)
            {
                var input = Helpers.GetBigInteger(0, paillier.MaxPlaintextSize);
                Assert.AreEqual(input, paillier.Decrypt(paillier.Encrypt(input)));
            }
        }

        #endregion
    }
}
