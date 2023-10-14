using System.Numerics;

namespace EncryptAddition.Crypto
{
    public interface IAsymmetricAlgorithm<KeyPair, CipherText>
    {
        CipherText Encrypt(BigInteger input);

        BigInteger Decrypt(CipherText input);
    }
}
