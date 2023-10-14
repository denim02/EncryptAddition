using System.Numerics;

namespace EncryptAddition.Crypto.Paillier
{
    public readonly struct PaillierCipherText
    {
        public BigInteger encryptedMessage { get; }

        public PaillierCipherText(BigInteger encryptedMessage)
        {
            this.encryptedMessage = encryptedMessage;
        }
    }
}
