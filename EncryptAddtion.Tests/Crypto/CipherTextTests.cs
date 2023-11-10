namespace EncryptAddtion.Tests.Crypto
{
    [TestClass]
    public class CipherTextTests
    {
        [TestMethod]
        public void CipherText_SharedSecretIsNull()
        {
            BigInteger encryptedMessage = 123456789;
            BigInteger? sharedSecret = null;

            CipherText cipherText = new(encryptedMessage);

            Assert.AreEqual(sharedSecret, cipherText.SharedSecret);
        }

        [TestMethod]
        public void CipherText_SharedSecretIsNotNull()
        {
            BigInteger encryptedMessage = 123456789;
            BigInteger sharedSecret = 987654321;

            CipherText cipherText = new(encryptedMessage, sharedSecret);

            Assert.AreEqual(sharedSecret, cipherText.SharedSecret);
        }

        [TestMethod]
        public void CipherText_SharedSecret_ArgumentOutOfRange()
        {
            BigInteger encryptedMessage = 123456789;
            BigInteger sharedSecret = -987654321;

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new CipherText(encryptedMessage, sharedSecret));
        }

        [TestMethod]
        public void CipherText_EncryptedMessage_ArgumentOutOfRange()
        {
            BigInteger encryptedMessage = -123456789;
            BigInteger sharedSecret = 987654321;

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new CipherText(encryptedMessage, sharedSecret));
        }
    }
}
