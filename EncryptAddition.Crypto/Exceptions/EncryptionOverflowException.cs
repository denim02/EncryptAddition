using System.Numerics;

namespace EncryptAddition.Crypto.Exceptions
{
    public class EncryptionOverflowException : Exception
    {
        public BigInteger OverflowValue { get; set; }
        public BigInteger MaxPlaintextSize { get; set; }

        public EncryptionOverflowException(BigInteger overflowValue, BigInteger maxPlaintextSize)
        {
            OverflowValue = overflowValue;
            MaxPlaintextSize = maxPlaintextSize;
        }

        public EncryptionOverflowException(BigInteger overflowValue, BigInteger maxPlaintextSize, string message) : base(message)
        {
            OverflowValue = overflowValue;
            MaxPlaintextSize = maxPlaintextSize;
        }

        public EncryptionOverflowException(BigInteger overflowValue, BigInteger maxPlaintextSize, string message, Exception innerException) : base(message, innerException)
        {
            OverflowValue = overflowValue;
            MaxPlaintextSize = maxPlaintextSize;
        }
    }
}
