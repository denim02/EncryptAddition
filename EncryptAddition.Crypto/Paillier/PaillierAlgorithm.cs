using System.Numerics;

namespace EncryptAddition.Crypto.Paillier
{
    public class PaillierAlgorithm : IAsymmetricAlgorithm<KeyPair, PaillierCipherText>, IAdditivelyHomomorphic<PaillierCipherText>
    {
        private KeyPair KeyPair { get; }
        private readonly BigInteger nSquared;

        public PaillierAlgorithm(BigInteger p, BigInteger q)
        {
            KeyGenerator keyGenerator = new(p, q);
            KeyPair = keyGenerator.GenerateKeyPair();
            nSquared = BigInteger.Pow(KeyPair.PublicKey.N, 2);
        }
        public PaillierAlgorithm(int primeBitLength)
        {
            BigInteger p = Helpers.GeneratePrime(primeBitLength);
            BigInteger q = Helpers.GeneratePrime(primeBitLength);
            while (p == q)
                q = Helpers.GeneratePrime(primeBitLength);

            KeyGenerator keyGenerator = new(p, q);
            KeyPair = keyGenerator.GenerateKeyPair();
            nSquared = BigInteger.Pow(KeyPair.PublicKey.N, 2);
        }

        public PaillierAlgorithm(KeyPair keyPair)
        {
            KeyPair = keyPair;
            nSquared = BigInteger.Pow(KeyPair.PublicKey.N, 2);
        }

        public PaillierCipherText Encrypt(BigInteger input)
        {
            BigInteger randomVal;

            do
            {
                randomVal = Helpers.NextBigInteger(1, KeyPair.PublicKey.N - 1);
            } while (BigInteger.GreatestCommonDivisor(randomVal, KeyPair.PublicKey.N) != BigInteger.One);

            return new PaillierCipherText(BigInteger.Multiply(BigInteger.ModPow(KeyPair.PublicKey.G, input, nSquared), BigInteger.ModPow(randomVal, KeyPair.PublicKey.N, nSquared)) % (nSquared));
        }

        public BigInteger Decrypt(PaillierCipherText cipher)
        {

            BigInteger power = BigInteger.ModPow(cipher.encryptedMessage, KeyPair.PrivateKey.Lambda, nSquared);
            BigInteger ratio = L(power);
            BigInteger result = BigInteger.Multiply(ratio, KeyPair.PrivateKey.Mu) % KeyPair.PublicKey.N;

            return result;
        }

        public PaillierCipherText Add(params PaillierCipherText[] ciphers)
        {
            if (ciphers.Length == 0)
                throw new InvalidOperationException("At least one value must be passed to the function.");
            if (ciphers.Length == 1)
                return ciphers[0];

            return ciphers.Aggregate((cipher1, cipher2) => new PaillierCipherText(BigInteger.Multiply(cipher1.encryptedMessage, cipher2.encryptedMessage) % nSquared));
        }

        private BigInteger L(BigInteger input)
        {
            return (input - 1) / KeyPair.PublicKey.N;
        }
    }
}
