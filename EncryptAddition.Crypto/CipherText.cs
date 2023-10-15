using System.Numerics;

namespace EncryptAddition.Crypto
{
    public readonly struct CipherText
    {
        public readonly BigInteger? SharedSecret { get; }
        public BigInteger EncryptedMessage { get; }

        public CipherText(BigInteger encryptedMessage)
        {
            SharedSecret = null;
            EncryptedMessage = encryptedMessage;
        }

        public CipherText(BigInteger encryptedMessage, BigInteger sharedSecret)
        {
            SharedSecret = sharedSecret;
            EncryptedMessage = encryptedMessage;
        }
    }
}
