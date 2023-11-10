using System.Numerics;

namespace EncryptAddition.Crypto
{
    /// <summary>
    /// IEncryptionStrategy defines a common set of methods to be implemented
    /// by any homomorphic encryption algorithms (i.e. ElGamal and Paillier).
    /// Objects that implement this interface can be used for benchmarks
    /// using the Strategy design pattern.
    /// </summary>
    public interface IEncryptionStrategy
    {
        CipherText Encrypt(BigInteger input);

        BigInteger Decrypt(CipherText input);

        CipherText Add(params CipherText[] ciphers);

        void RegenerateKeys();

        string PrintKeys();
    }
}
