using System.Numerics;

namespace EncryptAddition.Crypto.ElGamal
{

    public class ElGamalAlgorithm : IAsymmetricAlgorithm<KeyPair, ElGamalCipherText>, IAdditivelyHomomorphic<ElGamalCipherText>
    {
        private KeyPair KeyPair { get; }

        public ElGamalAlgorithm(BigInteger prime, BigInteger generator)
        {
            KeyGenerator keyGenerator = new KeyGenerator(prime, generator);
            KeyPair = keyGenerator.GenerateKeyPair();
        }

        public ElGamalAlgorithm(int primeBitLength)
        {
            BigInteger prime = Helpers.GenerateSafePrime(primeBitLength);
            BigInteger generator = Helpers.FindGeneratorForSafePrime(prime);
            KeyGenerator keyGenerator = new KeyGenerator(prime, generator);
            KeyPair = keyGenerator.GenerateKeyPair();
        }

        public ElGamalAlgorithm(KeyPair keyPair)
        {
            KeyPair = keyPair;
        }

        public ElGamalCipherText Encrypt(BigInteger input)
        {
            int k = 100;
            BigInteger sharedSecret = BigInteger.ModPow(KeyPair.PublicKey.Generator, k, KeyPair.PublicKey.Prime);
            BigInteger encryptedMessage = BigInteger.Multiply(
                BigInteger.ModPow(
                    KeyPair.PublicKey.Beta,
                    k,
                    KeyPair.PublicKey.Prime),
                BigInteger.ModPow(KeyPair.PublicKey.Generator, input, KeyPair.PublicKey.Prime)
                ) % KeyPair.PublicKey.Prime;

            return new ElGamalCipherText(sharedSecret, encryptedMessage);
        }

        public BigInteger Decrypt(ElGamalCipherText cipher)
        {
            BigInteger val = BigInteger.Multiply(
                cipher.EncryptedMessage % KeyPair.PublicKey.Prime,
                Helpers.PrimeModInverse(
                BigInteger.ModPow(
                    cipher.SharedSecret,
                    KeyPair.PrivateKey,
                    KeyPair.PublicKey.Prime
                    ),
                KeyPair.PublicKey.Prime)
                ) % KeyPair.PublicKey.Prime;

            return Helpers.SolveDiscreteLogarithm(KeyPair.PublicKey.Prime, KeyPair.PublicKey.Generator, val);
        }

        public ElGamalCipherText Add(params ElGamalCipherText[] ciphers)
        {
            if (ciphers.Length == 0)
                throw new InvalidOperationException("At least one value must be passed to the function.");
            if (ciphers.Length == 1)
                return ciphers[0];

            BigInteger sharedSecret = ciphers.Select(cipher => cipher.SharedSecret).Aggregate((secret1, secret2) => BigInteger.Multiply(secret1 % KeyPair.PublicKey.Prime, secret2 % KeyPair.PublicKey.Prime) % KeyPair.PublicKey.Prime);

            BigInteger encryptedMessage = ciphers.Select(cipher => cipher.EncryptedMessage).Aggregate((encryptedMsg1, encryptedMsg2) => BigInteger.Multiply(encryptedMsg1 % KeyPair.PublicKey.Prime, encryptedMsg2 % KeyPair.PublicKey.Prime) % KeyPair.PublicKey.Prime);

            return new ElGamalCipherText(sharedSecret, encryptedMessage);
        }
    }
}
