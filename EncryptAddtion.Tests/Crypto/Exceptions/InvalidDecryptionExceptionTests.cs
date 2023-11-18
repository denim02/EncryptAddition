using EncryptAddition.Crypto.Exceptions;

namespace EncryptAddtion.Tests.Crypto.Exceptions
{
    [TestClass]
    public class InvalidDecryptionExceptionTests
    {
        [TestMethod]
        public void InvalidDecryptionException_DefaultConstructor()
        {
            CipherText cipher = new(123, 231);
            var exception = new InvalidDecryptionException(cipher);
            Assert.AreEqual(exception.CipherText, cipher);
        }

        [TestMethod]
        public void InvalidDecryptionException_MessageConstructor()
        {
            CipherText cipher = new(123, 231);
            var exception = new InvalidDecryptionException(cipher, "Test message");
            Assert.AreEqual(exception.CipherText, cipher);
            Assert.AreEqual("Test message", exception.Message);
        }

        [TestMethod]
        public void InvalidDecryptionException_MessageInnerExceptionConstructor()
        {
            CipherText cipher = new(123, 231);
            var innerException = new Exception("Inner Exception");
            var exception = new InvalidDecryptionException(cipher, "Test Message", innerException);
            Assert.AreEqual(exception.CipherText, cipher);
            Assert.AreEqual("Test Message", exception.Message);
            Assert.AreEqual(innerException, exception.InnerException);
        }
    }
}
