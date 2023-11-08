using EncryptAddition.Crypto.Utils;
using System.Numerics;

namespace EncryptAddition.Crypto.ElGamal
{
    public class ElGamalEncryption : IEncryptionStrategy
    {
        private KeyPair KeyPair { get; }
        public KeyGenerator? KeyGenerator { get; }

        public ElGamalEncryption(BigInteger prime, BigInteger generator)
        {
            KeyGenerator = new KeyGenerator(prime, generator);
            KeyPair = KeyGenerator.GenerateKeyPair();
        }

        public ElGamalEncryption(int primeBitLength)
        {
            BigInteger prime = Primality.GenerateSafePrime(primeBitLength);
            BigInteger generator = CyclicMath.FindGeneratorForSafePrime(prime);
            KeyGenerator = new KeyGenerator(prime, generator);
            KeyPair = KeyGenerator.GenerateKeyPair();
        }

        public ElGamalEncryption(KeyPair keyPair)
        {
            KeyPair = keyPair;
        }

        public CipherText Encrypt(BigInteger input)
        {
            int k = 100;
            BigInteger sharedSecret = BigInteger.ModPow(KeyPair.PublicKey.Generator, k, KeyPair.PublicKey.Prime);
            BigInteger encryptedMessage = Helpers.ModMul(
                BigInteger.ModPow(
                    KeyPair.PublicKey.Beta,
                    k,
                    KeyPair.PublicKey.Prime),
                BigInteger.ModPow(KeyPair.PublicKey.Generator, input, KeyPair.PublicKey.Prime)
                , KeyPair.PublicKey.Prime);

            return new CipherText(sharedSecret, encryptedMessage);
        }

        public BigInteger Decrypt(CipherText cipher)
        {
            if (!cipher.SharedSecret.HasValue)
                throw new ArgumentException("Invalid cipher text entered. ElGamal requires the cipher to contain a shared secret.");

            BigInteger val = BigInteger.Multiply(
                cipher.EncryptedMessage % KeyPair.PublicKey.Prime,
                CyclicMath.PrimeModInverse(
                BigInteger.ModPow(
                    cipher.SharedSecret.Value,
                    KeyPair.PrivateKey,
                    KeyPair.PublicKey.Prime
                    ),
                KeyPair.PublicKey.Prime)
                ) % KeyPair.PublicKey.Prime;

            return CyclicMath.SolveDiscreteLogarithm(KeyPair.PublicKey.Prime, KeyPair.PublicKey.Generator, val);
        }

        public CipherText Add(params CipherText[] ciphers)
        {
            if (ciphers.Length == 0)
                throw new InvalidOperationException("At least one value must be passed to the function.");
            if (ciphers.Length == 1)
                return ciphers[0];

            if (ciphers.Any((cipher) => !cipher.SharedSecret.HasValue))
                throw new ArgumentException("Invalid cipher text entered. ElGamal requires the cipher to contain a shared secret.");

            BigInteger sharedSecret = ciphers.Select(cipher => cipher.SharedSecret.Value).Aggregate((secret1, secret2) => BigInteger.Multiply(secret1 % KeyPair.PublicKey.Prime, secret2 % KeyPair.PublicKey.Prime) % KeyPair.PublicKey.Prime);

            BigInteger encryptedMessage = ciphers.Select(cipher => cipher.EncryptedMessage).Aggregate((encryptedMsg1, encryptedMsg2) => BigInteger.Multiply(encryptedMsg1 % KeyPair.PublicKey.Prime, encryptedMsg2 % KeyPair.PublicKey.Prime) % KeyPair.PublicKey.Prime);

            return new CipherText(sharedSecret, encryptedMessage);
        }

    }
}
