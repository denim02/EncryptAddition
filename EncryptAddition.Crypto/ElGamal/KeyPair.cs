using System.Numerics;

namespace EncryptAddition.Crypto.ElGamal
{
    public readonly struct KeyPair
    {
        public BigInteger PrivateKey { get; }
        public PublicKey PublicKey { get; }
        public KeyPair(BigInteger privateKey, PublicKey publicKey)
        {
            PrivateKey = privateKey;
            PublicKey = publicKey;
        }
    }

    public readonly struct PublicKey
    {
        public BigInteger Prime { get; }
        public BigInteger Generator { get; }
        public BigInteger Beta { get; }

        public PublicKey(BigInteger prime, BigInteger generator, BigInteger beta)
        {
            Prime = prime;
            Generator = generator;
            Beta = beta;
        }
    }
}
