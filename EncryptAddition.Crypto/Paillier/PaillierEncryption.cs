using System.Numerics;

namespace EncryptAddition.Crypto.Paillier
{
    public class PaillierEncryption : IEncryptionStrategy
    {
        private KeyPair KeyPair { get; }
        private KeyGenerator? KeyGenerator { get; }
        private readonly BigInteger nSquared;

        public PaillierEncryption(BigInteger p, BigInteger q)
        {
            KeyGenerator keyGenerator = new(p, q);
            KeyPair = keyGenerator.GenerateKeyPair();
            nSquared = BigInteger.Pow(KeyPair.PublicKey.N, 2);
        }

        public PaillierEncryption(int primeBitLength)
        {
            BigInteger p = Helpers.GeneratePrime(primeBitLength);
            BigInteger q = Helpers.GeneratePrime(primeBitLength);
            while (p == q)
                q = Helpers.GeneratePrime(primeBitLength);

            KeyGenerator = new KeyGenerator(p, q);
            KeyPair = KeyGenerator.GenerateKeyPair();
            nSquared = BigInteger.Pow(KeyPair.PublicKey.N, 2);
        }

        public PaillierEncryption(KeyPair keyPair)
        {
            KeyPair = keyPair;
            nSquared = BigInteger.Pow(KeyPair.PublicKey.N, 2);
        }

        public CipherText Encrypt(BigInteger input)
        {
            BigInteger randomVal;

            do
            {
                randomVal = Helpers.NextBigInteger(1, KeyPair.PublicKey.N - 1);
            } while (BigInteger.GreatestCommonDivisor(randomVal, KeyPair.PublicKey.N) != BigInteger.One);

            return new CipherText(BigInteger.Multiply(BigInteger.ModPow(KeyPair.PublicKey.G, input, nSquared), BigInteger.ModPow(randomVal, KeyPair.PublicKey.N, nSquared)) % (nSquared));
        }

        public BigInteger Decrypt(CipherText cipher)
        {

            BigInteger power = BigInteger.ModPow(cipher.EncryptedMessage, KeyPair.PrivateKey.Lambda, nSquared);
            BigInteger ratio = L(power);
            BigInteger result = BigInteger.Multiply(ratio, KeyPair.PrivateKey.Mu) % KeyPair.PublicKey.N;

            return result;
        }

        public CipherText Add(params CipherText[] ciphers)
        {
            if (ciphers.Length == 0)
                throw new InvalidOperationException("At least one value must be passed to the function.");
            if (ciphers.Length == 1)
                return ciphers[0];

            return ciphers.Aggregate((cipher1, cipher2) => new CipherText(BigInteger.Multiply(cipher1.EncryptedMessage, cipher2.EncryptedMessage) % nSquared));
        }

        private BigInteger L(BigInteger input)
        {
            return (input - 1) / KeyPair.PublicKey.N;
        }
    }
}
