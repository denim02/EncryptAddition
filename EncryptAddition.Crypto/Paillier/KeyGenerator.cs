using System.Numerics;

namespace EncryptAddition.Crypto.Paillier
{
    internal class KeyGenerator : IKeyGenerator<KeyPair>
    {
        private readonly BigInteger _p;
        private readonly BigInteger _q;

        public KeyGenerator(BigInteger p, BigInteger q)
        {
            _p = p;
            _q = q;
        }

        public KeyPair GenerateKeyPair()
        {
            BigInteger n = BigInteger.Multiply(_p, _q);
            BigInteger g = BigInteger.Add(n, BigInteger.One);
            BigInteger lambda = BigInteger.Multiply(_p - 1, _q - 1);
            BigInteger mu = BigInteger.ModPow(lambda, lambda - 1, n);

            PublicKey publicKey = new(n, g);
            PrivateKey privateKey = new(lambda, mu);
            return new KeyPair(publicKey, privateKey);
        }
    }
}
