using System.Numerics;

namespace EncryptAddition.Crypto
{
    public struct KeyPair
    {
        public BigInteger PrivateKey { get; set; }
        public ElGamalPublicKey PublicKey { get; set; }

        public KeyPair(BigInteger privateKey, ElGamalPublicKey publicKey)
        {
            PrivateKey = privateKey;
            PublicKey = publicKey;
        }
    }
}
