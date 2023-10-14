using System.Numerics;

namespace EncryptAddition.Crypto.ElGamal
{
    public readonly struct ElGamalCipherText
    {
        public BigInteger SharedSecret { get; }
        public BigInteger EncryptedMessage { get; }

        public ElGamalCipherText(BigInteger sharedSecret, BigInteger encryptedMessage)
        {
            SharedSecret = sharedSecret;
            EncryptedMessage = encryptedMessage;
        }
    }
}
