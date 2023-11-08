using EncryptAddition.Crypto.Utils;
using System.Numerics;
using System.Security.Cryptography;

namespace EncryptAddition.Crypto.ElGamal
{
    public class KeyGenerator : IKeyGenerator<KeyPair>
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
            var rng = RandomNumberGenerator.Create();

            var privateKey = Helpers.GetBigInteger(2, _prime - 1);
            var beta = BigInteger.ModPow(_generator, privateKey, _prime);

            PublicKey publicKey = new PublicKey(_prime, _generator, beta);

            rng.Dispose();

            return new KeyPair(privateKey, publicKey);
        }
    }
}
