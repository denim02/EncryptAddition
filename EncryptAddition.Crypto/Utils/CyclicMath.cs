using System.Numerics;

namespace EncryptAddition.Crypto.Utils
{
    public static class CyclicMath
    {
        public static BigInteger FindGeneratorForSafePrime(BigInteger safePrime)
        {
            // Find the Sophie Germain prime from the safe-prime
            BigInteger sophieGermainPrime = (safePrime - 1) / 2;

            while (true)
            {
                BigInteger g = Helpers.GetBigInteger(2, safePrime - 1);
                if (!(BigInteger.ModPow(g, 2, safePrime) == 1) || BigInteger.ModPow(g, sophieGermainPrime, safePrime) == 1)
                    return g;
            }

        }

        // Uses baby-step/giant-step algorithm
        public static BigInteger SolveDiscreteLogarithm(BigInteger orderOfGroup, BigInteger generator, BigInteger argument)
        {
            BigInteger m = BigInteger.Min(new(Math.Ceiling(Math.Sqrt((double)orderOfGroup))), 100000);

            Dictionary<BigInteger, int> lookupTable = new();

            for (var j = 0; j < m; j++)
            {
                lookupTable[BigInteger.ModPow(generator, j, orderOfGroup)] = j;
            }

            BigInteger inverse = PrimeModInverse(BigInteger.ModPow(generator, m, orderOfGroup), orderOfGroup);
            BigInteger y = argument;

            BigInteger numGiantSteps = (orderOfGroup + m - 1) / m;
            for (var i = 0; i < numGiantSteps; i++)
            {
                int j = -1;
                bool result = lookupTable.TryGetValue(y, out j);
                if (result)
                    return Helpers.ModMul(i, m, orderOfGroup) + j;

                y = Helpers.ModMul(y, inverse, orderOfGroup);
            }

            return -1;
        }


        /// <summary>
        /// Determines the modular inverse of a number using Fermat's little theorem.
        /// The specified modulus must be a prime number.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="modulus"></param>
        /// <returns></returns>
        public static BigInteger PrimeModInverse(BigInteger value, BigInteger modulus)
        {
            // If the two arguments are not coprime (aka their GCD is 1), the modular inverse does not even exist.
            // If the modulus is not a prime number, then it cannot be determined with this method.
            return (BigInteger.GreatestCommonDivisor(value, modulus) == 1 && modulus.IsProbablePrime())
                ? BigInteger.ModPow(value, modulus - 2, modulus)
                : -1;
        }

        private static BigInteger FuncF(BigInteger x_i, BigInteger baseVal, BigInteger y, BigInteger p)
        {
            if (x_i % 3 == 2)
                return (y * x_i) % p;
            else if (x_i % 3 == 0)
                return BigInteger.ModPow(x_i, 2, p);
            else if (x_i % 3 == 1)
                return (baseVal * x_i) % p;
            else
                throw new Exception("[-] Something's wrong!");
        }

        private static BigInteger FuncG(BigInteger a, BigInteger n, BigInteger p, BigInteger x_i)
        {
            if (x_i % 3 == 2)
                return a;
            else if (x_i % 3 == 0)
                return (2 * a) % n;
            else if (x_i % 3 == 1)
                return (a + 1) % n;
            else
                throw new Exception("[-] Something's wrong!");
        }

        private static BigInteger FuncH(BigInteger b, BigInteger n, BigInteger p, BigInteger x_i)
        {
            if (x_i % 3 == 2)
                return (b + 1) % n;
            else if (x_i % 3 == 0)
                return (2 * b) % n;
            else if (x_i % 3 == 1)
                return b;
            else
                throw new Exception("[-] Something's wrong!");
        }


        public static BigInteger PollardRho(BigInteger baseVal, BigInteger y, BigInteger p, BigInteger n)
        {
            if (!n.IsProbablePrime())
                throw new Exception("[-] Order of group must be prime for Pollard Rho");

            BigInteger x_i = 1;
            BigInteger x_2i = 1;
            BigInteger a_i = 0;
            BigInteger b_i = 0;
            BigInteger a_2i = 0;
            BigInteger b_2i = 0;

            BigInteger i = 1;
            while (i <= n)
            {
                // Single Step calculations
                a_i = FuncG(a_i, n, p, x_i);
                b_i = FuncH(b_i, n, p, x_i);
                x_i = FuncF(x_i, baseVal, y, p);

                // Double Step calculations
                a_2i = FuncG(FuncG(a_2i, n, p, x_2i), n, p, FuncF(x_2i, baseVal, y, p));
                b_2i = FuncH(FuncH(b_2i, n, p, x_2i), n, p, FuncF(x_2i, baseVal, y, p));
                x_2i = FuncF(FuncF(x_2i, baseVal, y, p), baseVal, y, p);

                if (x_i == x_2i)
                {
                    BigInteger r = (b_i - b_2i) % n;
                    if (r == 0)
                        throw new Exception("[-] b_i = b_2i, returning -1");
                    else
                    {
                        if (BigInteger.GreatestCommonDivisor(r, n) == 1)
                        {
                            return (PrimeModInverse(r, n) * (a_2i - a_i)) % n;
                        }
                        else
                        {
                            throw new Exception("GCD(r, n) != 1, cannot solve DLP with current algorithm.");
                        }
                    }
                }
                else
                {
                    i += 1;
                }
            }

            throw new Exception("Failed to converge.");
        }
    }
}
