using EncryptAddition.Crypto.Exceptions;

namespace EncryptAddtion.Tests.Crypto.Exceptions
{
    [TestClass]
    public class InvalidKeyPairExceptionTests
    {
        [TestMethod]
        public void InvalidKeyPairException_DefaultConstructor()
        {
            var exception = new InvalidKeyPairException("123|123|123;123");
            Assert.AreEqual(exception.SerializedKeyPair, "123|123|123;123");
        }

        [TestMethod]
        public void InvalidKeyPairException_MessageConstructor()
        {
            var exception = new InvalidKeyPairException("123|123|123;123", "Test Message");
            Assert.AreEqual("Test Message", exception.Message);
            Assert.AreEqual(exception.SerializedKeyPair, "123|123|123;123");
        }

        [TestMethod]
        public void InvalidKeyPairException_MessageInnerExceptionConstructor()
        {
            var innerException = new Exception("Inner Exception");
            var exception = new InvalidKeyPairException("123|123|123;123", "Test Message", innerException);
            Assert.AreEqual("Test Message", exception.Message);
            Assert.AreEqual(exception.SerializedKeyPair, "123|123|123;123");
            Assert.AreEqual(innerException, exception.InnerException);
        }
    }
}
