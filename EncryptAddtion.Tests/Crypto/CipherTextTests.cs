namespace EncryptAddtion.Tests.Crypto
{
    [TestClass]
    public class CipherTextTests
    {
        #region Constructor Tests
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
        public void CipherText_SharedSecretAndEncryptedMessage_ArgumentOutOfRange()
        {
            BigInteger encryptedMessage = 123456789;
            BigInteger sharedSecret = -987654321;

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new CipherText(encryptedMessage, sharedSecret));
        }

        [TestMethod]
        public void CipherText_EncryptedMessageAndSharedSecret_ArgumentOutOfRange()
        {
            BigInteger encryptedMessage = -123456789;
            BigInteger sharedSecret = 987654321;

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new CipherText(encryptedMessage, sharedSecret));
        }

        [TestMethod]
        public void CipherText_EncryptedMessage_ArgumentOutOfRange()
        {
            BigInteger encryptedMessage = -123456789;

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new CipherText(encryptedMessage));
        }

        [TestMethod]
        public void CipherText_EncryptedMessage_CorrectValues()
        {
            BigInteger encryptedMessage = 1234;
            CipherText cipher = new(encryptedMessage);

            Assert.AreEqual(cipher.EncryptedMessage, encryptedMessage);
        }
        #endregion

        #region Class Methods
        [TestMethod]
        public void CipherText_ToString()
        {
            BigInteger encryptedMessage = 1234;
            BigInteger sharedSecret = 567;

            CipherText cipher1 = new(encryptedMessage);
            CipherText cipher2 = new(encryptedMessage, sharedSecret);
            Assert.AreEqual(cipher1.ToString(), $"{encryptedMessage}");
            Assert.AreEqual(cipher2.ToString(), $"{encryptedMessage}|{sharedSecret}");
        }

        [TestMethod]
        public void CipherText_Deserialize_InvalidCipher()
        {
            Assert.ThrowsException<ArgumentException>(() => CipherText.Deserialize("asd"));
            Assert.ThrowsException<ArgumentException>(() => CipherText.Deserialize("11a"));
        }

        [TestMethod]
        public void CipherText_Deserialize_CorrectEncryptedMessage()
        {
            CipherText cipher = CipherText.Deserialize("123");
            Assert.AreEqual(cipher.EncryptedMessage, 123);
            Assert.IsNull(cipher.SharedSecret);
        }

        [TestMethod]
        public void CipherText_Deserialize_CorrectBothFields()
        {
            CipherText cipher = CipherText.Deserialize("123|123");
            Assert.AreEqual(cipher.EncryptedMessage, 123);
            Assert.AreEqual(cipher.SharedSecret, 123);
        }
        #endregion
    }
}
