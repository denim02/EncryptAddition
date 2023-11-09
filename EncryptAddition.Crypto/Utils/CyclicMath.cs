using System.Numerics;

namespace EncryptAddition.Crypto.Utils
{
    public static class CyclicMath
    {
        /// <summary>
        /// Finds a generator within the cyclic group of the provided safe-prime.
        /// </summary>
        /// <returns>A generator for the defined cyclic group.</returns>
        /// <param name="safePrime">A BigInteger safe prime.</param>
        public static BigInteger FindGeneratorForSafePrime(BigInteger safePrime)
        {
            // No validation is done to ensure that the safe prime is truly prime or an actual 
            // safe prime to preserve performance. Use with caution.

            // Minimal check to ensure that no bogus values are entered (5 is the smallest safe prime)
            if (safePrime < 5)
                throw new ArgumentOutOfRangeException(nameof(safePrime), "A safe prime cannot be less than 5.");

            // Find the Sophie Germain prime from the safe-prime
            BigInteger sophieGermainPrime = (safePrime - 1) / 2;

            while (true)
            {
                // Since the group order is 2p (2p + 1 - 1), the order of the elements is necessarily 2, p, or 2p.
                // The generator will have order 2p.
                BigInteger g = Helpers.GetBigInteger(2, safePrime - 1);
                if (!(BigInteger.ModPow(g, 2, safePrime) == 1) || BigInteger.ModPow(g, sophieGermainPrime, safePrime) == 1)
                    return g;
            }

        }

        public static BigInteger DiscreteLog(BigInteger orderOfGroup, BigInteger generator, BigInteger argument)
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
        /// </summary>
        /// <returns>The modular inverse of the provided number.</returns>
        /// <param name="value">The BigInteger value whose modular inverse should be calculated.</param>
        /// <param name="modulus">The modulus. Must be prime and positive.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the modulus is negative or 0.</exception>
        /// <exception cref="ArgumentException">Thrown if the modulus is not a prime number.</exception>
        public static BigInteger PrimeModInverse(BigInteger value, BigInteger modulus)
        {
            // The modulus must be a positive number.
            if (modulus <= 0)
                throw new ArgumentOutOfRangeException(nameof(modulus), "The modulus must be positive.");
            // Also, the modulus must be prime to use this method.
            if (!modulus.IsProbablePrime())
                throw new ArgumentException("The modulus must be a prime number.", nameof(modulus));

            // If the two arguments are not coprime (aka their GCD is 1), the modular inverse does not exist.
            return (BigInteger.GreatestCommonDivisor(value, modulus) == 1)
                ? BigInteger.ModPow(value, modulus - 2, modulus)
                : -1;
        }
    }
}
