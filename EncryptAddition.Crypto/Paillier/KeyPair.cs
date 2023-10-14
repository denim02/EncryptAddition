using System.Numerics;

namespace EncryptAddition.Crypto.Paillier
{
    public readonly struct KeyPair
    {
        public PublicKey PublicKey { get; }
        public PrivateKey PrivateKey { get; }

        public KeyPair(PublicKey publicKey, PrivateKey privateKey)
        {
            PublicKey = publicKey;
            PrivateKey = privateKey;
        }
    }

    public readonly struct PublicKey
    {
        public BigInteger N { get; }
        public BigInteger G { get; }

        public PublicKey(BigInteger primeMultiple, BigInteger randomValue)
        {
            N = primeMultiple;
            G = randomValue;
        }
    }

    public readonly struct PrivateKey
    {
        public BigInteger Lambda { get; }
        public BigInteger Mu { get; }

        public PrivateKey(BigInteger lambda, BigInteger mu)
        {
            Lambda = lambda;
            Mu = mu;
        }
    }
}
