using System.Numerics;

namespace EncryptAddition.Crypto
{
    public class Paillier
    {
        private readonly BigInteger p;
        private readonly BigInteger q;
        private readonly BigInteger n;
        private readonly BigInteger g;
        private readonly BigInteger lambda;
        private readonly BigInteger miu;

        public Paillier(BigInteger p, BigInteger q)
        {
            this.p = p;
            this.q = q;
            n = BigInteger.Multiply(p, q);
            g = BigInteger.Add(n, BigInteger.One);
            lambda = BigInteger.Multiply(p - 1, q - 1);
            miu = BigInteger.ModPow(lambda, lambda - 1, n);
        }

        public BigInteger Encrypt(BigInteger input)
        {
            BigInteger r;

            do
            {
                r = Helpers.NextBigInteger(1, n - 1);
            } while (BigInteger.GreatestCommonDivisor(r, n) != BigInteger.One);

            return BigInteger.Multiply(BigInteger.ModPow(g, input, n * n), BigInteger.ModPow(r, n, n * n)) % (n * n);
        }

        public BigInteger Decrypt(BigInteger cipher)
        {
            BigInteger temperr = BigInteger.ModPow(cipher, lambda, BigInteger.Multiply(n, n));
            BigInteger temp = L(temperr);
            BigInteger result = BigInteger.Multiply(temp, miu) % n;

            return result;
        }

        public BigInteger Add(BigInteger cipher1, BigInteger cipher2)
        {
            return BigInteger.Multiply(cipher1, cipher2) % (n * n);
        }

        private BigInteger L(BigInteger input)
        {
            return (input - 1) / n;
        }
    }
}
