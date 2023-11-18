using EncryptAddition.Crypto.Exceptions;

namespace EncryptAddtion.Tests.Crypto.Exceptions
{
    [TestClass]
    public class EncryptionOverflowExceptionTests
    {
        [TestMethod]
        public void EncryptionOverflowException_DefaultConstructor()
        {
            var exception = new EncryptionOverflowException(123, 231);
            Assert.AreEqual(exception.OverflowValue, new BigInteger(123));
            Assert.AreEqual(exception.MaxPlaintextSize, new BigInteger(231));
        }

        [TestMethod]
        public void EncryptionOverflowException_MessageConstructor()
        {
            var exception = new EncryptionOverflowException(123, 231, "Test Message");
            Assert.AreEqual(exception.OverflowValue, new BigInteger(123));
            Assert.AreEqual(exception.MaxPlaintextSize, new BigInteger(231));
            Assert.AreEqual("Test Message", exception.Message);
        }

        [TestMethod]
        public void EncryptionOverflowException_MessageInnerExceptionConstructor()
        {
            var innerException = new Exception("Inner Exception");
            var exception = new EncryptionOverflowException(123, 231, "Test Message", innerException);
            Assert.AreEqual(exception.OverflowValue, new BigInteger(123));
            Assert.AreEqual(exception.MaxPlaintextSize, new BigInteger(231));
            Assert.AreEqual("Test Message", exception.Message);
            Assert.AreEqual(innerException, exception.InnerException);
        }
    }
}
