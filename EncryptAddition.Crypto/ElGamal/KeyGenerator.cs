using System.Numerics;

namespace EncryptAddition.Crypto.ElGamal
{
    internal class KeyGenerator : IKeyGenerator<KeyPair>
    {
        private readonly BigInteger _prime;
        private readonly BigInteger _generator;

        public KeyGenerator(BigInteger prime, BigInteger generator)
        {
            _prime = prime;
            _generator = generator;
        }

        public KeyPair GenerateKeyPair()
        {
            var privateKey = Helpers.NextBigInteger(2, _prime - 1);
            var beta = BigInteger.ModPow(_generator, privateKey, _prime);
            PublicKey publicKey = new PublicKey(_prime, _generator, beta);
            return new KeyPair(privateKey, publicKey);
        }
    }
}
