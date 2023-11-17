using EncryptAddition.Crypto;
using System.Numerics;

namespace EncryptAddition.WPF.Models.Services
{
    public interface IEncryptionService
    {
        void PrepareService();
        CipherText[] Encrypt(BigInteger[] inputs);
        BigInteger[] Decrypt(CipherText[] ciphers);
        string PrintKeys();
        EncryptionChoice GetEncryptionChoice();
        BigInteger GetMaxPlaintextSize();
    }
}
