using System.Numerics;

namespace EncryptAddition.Crypto
{
    public struct ElGamalPublicKey
    {
        public BigInteger Prime { get; set; }
        public BigInteger Generator { get; set; }
        public BigInteger Beta { get; set; }


        public ElGamalPublicKey(BigInteger prime, BigInteger generator, BigInteger beta)
        {
            Prime = prime;
            Generator = generator;
            Beta = beta;
        }
    }

    public class ElGamal
    {
        private readonly BigInteger _prime;
        private readonly BigInteger _generator;
        public KeyPair KeyPair { get; private set; }

        public ElGamal(BigInteger prime, BigInteger generator)
        {
            _prime = prime;
            _generator = generator;
            KeyPair = GenerateKeyPair();
        }

        public KeyPair GenerateKeyPair()
        {
            Random random = new();
            var privateKey = Helpers.NextBigInteger(2, _prime - 1);

            var beta = BigInteger.ModPow(_generator, privateKey, _prime);

            ElGamalPublicKey publicKey = new ElGamalPublicKey(_prime, _generator, beta);
            return new KeyPair(privateKey, publicKey);
        }

        public (BigInteger r, BigInteger t) Encrypt(BigInteger input)
        {
            int k = 100;
            BigInteger r = BigInteger.ModPow(KeyPair.PublicKey.Generator, k, KeyPair.PublicKey.Prime);
            BigInteger t = BigInteger.Multiply(
                BigInteger.ModPow(
                    KeyPair.PublicKey.Beta,
                    k,
                    KeyPair.PublicKey.Prime),
                BigInteger.ModPow(KeyPair.PublicKey.Generator, input, KeyPair.PublicKey.Prime)
                ) % KeyPair.PublicKey.Prime;

            return (r, t);
        }

        public BigInteger Decrypt((BigInteger r, BigInteger t) input)
        {
            BigInteger r = input.r;
            BigInteger t = input.t;


            BigInteger val = BigInteger.Multiply(
                t % KeyPair.PublicKey.Prime,
                Helpers.PrimeModInverse(
                BigInteger.ModPow(
                    r,
                    KeyPair.PrivateKey,
                    KeyPair.PublicKey.Prime
                    ),
                KeyPair.PublicKey.Prime)
                ) % KeyPair.PublicKey.Prime;

            return Helpers.SolveDiscreteLogarithm(KeyPair.PublicKey.Prime, KeyPair.PublicKey.Generator, val);
        }
    }
}
