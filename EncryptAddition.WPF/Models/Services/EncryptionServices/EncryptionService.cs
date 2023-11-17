using EncryptAddition.Crypto;
using EncryptAddition.Crypto.ElGamal;
using EncryptAddition.Crypto.Paillier;
using System.Linq;
using System.Numerics;

namespace EncryptAddition.WPF.Models.Services
{
    public class EncryptionService : IEncryptionService
    {
        private IEncryptionStrategy _encryptionStrategy;
        private EncryptionChoice _algorithmChoice;
        private string _serializedKey;
        private int _bitLength;

        public EncryptionService(EncryptionChoice algorithmChoice, string serializedKey)
        {
            _algorithmChoice = algorithmChoice;
            _serializedKey = serializedKey;
        }

        public EncryptionService(EncryptionChoice algorithmChoice, int bitLength)
        {
            _algorithmChoice = algorithmChoice;
            _bitLength = bitLength;
        }

        public void PrepareService()
        {
            if (_serializedKey != null)
                _encryptionStrategy = _algorithmChoice == EncryptionChoice.ELGAMAL ? new ElGamalEncryption(Crypto.ElGamal.KeyPair.Deserialize(_serializedKey)) : new PaillierEncryption(Crypto.Paillier.KeyPair.Deserialize(_serializedKey));
            else
                _encryptionStrategy = _algorithmChoice == EncryptionChoice.ELGAMAL ? new ElGamalEncryption(_bitLength) : new PaillierEncryption(_bitLength);
        }

        public CipherText[] Encrypt(BigInteger[] inputs)
        {
            return inputs.Select(input => _encryptionStrategy.Encrypt(input)).ToArray();
        }

        public BigInteger[] Decrypt(CipherText[] ciphers)
        {
            return ciphers.Select(cipher => _encryptionStrategy.Decrypt(cipher)).ToArray();
        }

        public string PrintKeys()
        {
            return _encryptionStrategy.PrintKeys();
        }

        public EncryptionChoice GetEncryptionChoice()
        {
            return _algorithmChoice;
        }

        public BigInteger GetMaxPlaintextSize()
        {
            return _encryptionStrategy.MaxPlaintextSize;
        }
    }
}
