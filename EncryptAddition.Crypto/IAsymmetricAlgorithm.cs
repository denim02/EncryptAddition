using System.Numerics;

namespace EncryptAddition.Crypto
{
    internal interface IAsymmetricAlgorithm<KeyPair, CipherText>
    {
        KeyPair GenerateKeyPair();

        CipherText Encrypt(BigInteger input);

        BigInteger Decrypt(CipherText input);
    }
}
