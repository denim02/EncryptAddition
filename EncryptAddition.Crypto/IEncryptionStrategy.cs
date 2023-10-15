using System.Numerics;

namespace EncryptAddition.Crypto
{
    public interface IEncryptionStrategy
    {
        CipherText Encrypt(BigInteger input);

        BigInteger Decrypt(CipherText input);

        public CipherText Add(params CipherText[] ciphers);
    }
}
