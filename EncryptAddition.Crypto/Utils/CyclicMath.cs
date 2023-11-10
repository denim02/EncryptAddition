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

            // Find the Sophie Germain prime from the safe-prime ((safePrime - 1) / 2).
            BigInteger sophieGermainPrime = (safePrime - 1) >> 1;

            while (true)
            {
                // Since the group order is 2p (2p + 1 - 1), the order of the elements is necessarily 2, p, or 2p.
                // The generator will have order 2p.
                BigInteger g = Helpers.GetBigInteger(2, safePrime - 1);
                if (!((BigInteger.ModPow(g, 2, safePrime) == 1) || BigInteger.ModPow(g, sophieGermainPrime, safePrime) == 1))
                    return g;
            }
        }

        /// <summary>
        /// Calculates and returns the discrete logarithm of an element of a finite abelian group (in this case
        /// the multiplicative group of integers modulo n) with a generator of the group as the base.
        /// When looking at the equation g^x = y mod n, y would be the element, g would be the generator,
        /// n would be the prime modulus (thus, the order is n - 1), and x is the value to be calculated.
        /// Uses the baby-step giant-step algorithm. 
        /// </summary>
        /// <returns>The calculated discrete logarithm or -1 if one could not be found.</returns>
        /// <param name="generator">The generator to serve as the base of the logarithm.</param>
        /// <param name="element">The power of the logarithm.</param>
        /// <param name="orderOfGroup">The order of the group. n - 1 if it is a multiplicative group modulo n.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if any parameters are not positive or if the generator and element do not fall within the group.</exception>
        public static BigInteger DiscreteLog(BigInteger generator, BigInteger element, BigInteger orderOfGroup)
        {
            // Validate arguments
            if (orderOfGroup <= 0)
                throw new ArgumentOutOfRangeException(nameof(orderOfGroup), "The order of the group must be a positive integer.");
            if (generator <= 0 || generator > orderOfGroup)
                throw new ArgumentOutOfRangeException(nameof(generator), "The generator must be a positive integer and fall within the defined group.");
            if (element <= 0 || element > orderOfGroup)
                throw new ArgumentOutOfRangeException(nameof(element), "The element must be a positive integer and fall within the defined group.");

            // No validation will be performed to check if the argument is actually a generator to preserve performance.
            // Use with caution.

            BigInteger mod = orderOfGroup + 1;

            // Trivial cases
            if (element == generator)
                return 1;
            if (element == 1)
                return 0;

            // Determine maximum bound for baby steps.
            // Arbitrary cap of 100000 steps was placed to prevent application from getting deadlocked by
            // insufficient memory.
            BigInteger cap = Helpers.SqrtCeil(orderOfGroup);
            cap = BigInteger.Min(cap, 100000);

            // Define lookup table to store babystep values.
            Dictionary<BigInteger, int> lookupTable = new();

            // Generate and store babysteps.
            for (var i = 0; i < cap; i++)
            {
                lookupTable[BigInteger.ModPow(generator, i, mod)] = i;
            }

            // Determine g^(-cap) mod p.
            BigInteger inverse = PrimeModInverse(BigInteger.ModPow(generator, cap, mod), mod);

            // From g^x = y, store a temporary copy of y.
            BigInteger y = element;

            // Since the algorithm uses a space-time tradeoff, if baby-steps are capped, giant-steps
            // must be increased. In total, they should cover the entire group so numOfBabySteps * numOfGiantSteps
            // should equal the order of the group.
            BigInteger numGiantSteps = (orderOfGroup + cap - 1) / cap; // Cap is added to perform ceiling division.

            // Check if a g^i = y can be found in the lookup table. If not, multiply y by g^-m to move backwards within
            // the group, so that eventually a value g^i = y * g^(-jm) is found. But, this is equivalent to 
            // g^i = g^x * g^(-jm). So, the exponents may be equated to produce i = x - jm or x = i + jm.
            for (var j = 0; j < numGiantSteps; j++)
            {
                bool result = lookupTable.TryGetValue(y, out int i);
                if (result)
                    return Helpers.ModMul(j, cap, mod) + i;

                y = Helpers.ModMul(y, inverse, mod);
            }

            // If the discrete log could not be determined, return -1.
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
