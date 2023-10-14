using System.Numerics;
using System.Security.Cryptography;

namespace EncryptAddition.Crypto
{
    public static class Helpers
    {
        private static readonly RandomNumberGenerator rng = RandomNumberGenerator.Create();

        public static BigInteger FindGeneratorForSafePrime(BigInteger safePrime)
        {
            // Find the Sophie Germain prime from the safe-prime
            BigInteger sophieGermainPrime = (safePrime - 1) / 2;

            while (true)
            {
                BigInteger g = NextBigInteger(2, safePrime - 1);
                if (!(BigInteger.ModPow(g, 2, safePrime).Equals(BigInteger.One) || BigInteger.ModPow(g, sophieGermainPrime, safePrime).Equals(BigInteger.One)))
                    return g;
            }

        }

        // Uses baby-step/giant-step algorithm
        public static BigInteger SolveDiscreteLogarithm(BigInteger orderOfGroup, BigInteger generator, BigInteger argument)
        {
            BigInteger m = new(Math.Ceiling(Math.Sqrt((double)argument)));

            Dictionary<BigInteger, int> lookupTable = new();

            for (var j = 0; j < m; j++)
            {
                lookupTable[BigInteger.ModPow(generator, j, orderOfGroup)] = j;
            }

            BigInteger inverse = PrimeModInverse(BigInteger.ModPow(generator, m, orderOfGroup), orderOfGroup);

            BigInteger y = argument;

            for (var i = 0; i < m; i++)
            {
                int j = -1;
                bool result = lookupTable.TryGetValue(y, out j);
                if (result)
                    return (BigInteger.Multiply(i, m) % orderOfGroup) + j;

                y = BigInteger.Multiply(y, inverse) % orderOfGroup;
            }

            return -1;
        }

        public static BigInteger GenerateSafePrime(int bitLength)
        {
            while (true)
            {
                BigInteger p = GeneratePrime(bitLength);
                // Determine if the argument is a Sophie Germain prime
                BigInteger q = 2 * p + 1;
                if (IsProbablePrime(q))
                    return q;
            }

        }

        // Naive prime generation algorithm
        public static BigInteger GeneratePrime(int bitLength)
        {
            while (true)
            {
                var candidate = NextBigInteger(BigInteger.One << (bitLength - 1), BigInteger.One << bitLength);
                if (IsProbablePrime(candidate)) return candidate;
            }
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
            return (BigInteger.GreatestCommonDivisor(value, modulus) == BigInteger.One && IsProbablePrime(modulus))
                ? BigInteger.ModPow(value, modulus - 2, modulus)
                : BigInteger.MinusOne;
        }

        /// <summary>
        /// Returns a boolean value indiciating whether the provided BigInteger value is a prime or not.
        /// The test checks for primality probabilistically using the Miller-Rabin primality test.
        /// The probability for a false positive is 4^(-k) where k is the number of iterations.
        /// If only one iteration is used, the probability of a correct results would be 75%.
        /// </summary>
        /// <param name="candidate">Prime candidate to be tested</param>
        /// <param name="certainty">Number of iterations of the test</param>
        /// <returns>true if the value is prime, false if not</returns>
        public static bool IsProbablePrime(BigInteger candidate, int certainty = 10)
        {
            // Trivial cases
            // Candidate is prime if it three or two.
            if (candidate == 2 || candidate == 3)
                return true;
            // Candidate cannot be prime if it is negative, 0, 1, or even (by definition).
            if (candidate < 2 || candidate % 2 == 0)
                return false;

            // Based on Fermat's little theorem, if candidate is an odd prime
            // and we take an integer a coprime to candidate,
            // then a^(candidate-1) ≡ 1 (mod candidate).
            // (candidate - 1) may be expressed as 2^s * d, where d is odd.
            // Below, we determine s and d.
            BigInteger d = candidate - 1;
            int s = 0;

            while (d % 2 == 0)
            {
                d /= 2;
                s += 1;
            }

            // Declaring a variable to hold the randomly-generated value a
            // that will be used to repeatedly test candidate for primality.
            Random random = new();
            BigInteger a;

            // Perform the Miller-Robin primality test *certainty* times.
            for (int i = 0; i < certainty; i++)
            {
                a = NextBigInteger(2, candidate - 2);

                // Start off with the odd component of the exponent (d).
                BigInteger x = BigInteger.ModPow(a, d, candidate);
                // If a^d % candidate is congruent to 1 or candidate - 1, then candidate is prime to base a.
                if (x == 1 || x == candidate - 1)
                    continue;

                // Otherwise, we square x repeatedly until we reach 1 or candidate - 1.
                for (int r = 1; r < s; r++)
                {
                    // If x^2 % candidate is congruent to candidate - 1, then candidate is prime to base a.
                    x = BigInteger.ModPow(x, 2, candidate);
                    if (x == 1)
                        return false;
                    if (x == candidate - 1)
                        break;
                }

                // Candidate is not prime.
                if (x != candidate - 1)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Returns a random BigInteger value within the specified range (inclusive).
        /// Only positive bounds are permitted.
        /// </summary>
        /// <param name="minBound"></param>
        /// <param name="maxBound"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static BigInteger NextBigInteger(BigInteger minBound, BigInteger maxBound)
        {
            // Trivial cases
            if (minBound < 0 || maxBound < 0)
                throw new ArgumentOutOfRangeException("The minimum and maximum bounds must be positive.");
            if (minBound > maxBound)
                throw new ArgumentException("The minimum bound cannot be larger than the maximum bound.");
            if (minBound == maxBound)
                return minBound;

            // Determine the zero-based upper bound (to reduce number of bits needed to work with).
            BigInteger zeroBasedUpperBound = maxBound - minBound;
            byte[] bytes = zeroBasedUpperBound.ToByteArray();
            byte mostSignificantByte = bytes[bytes.Length - 1];

            // Search for the most significant non-zero bit
            byte mostSignificantBitMask = 0b11111111;   // Will be used with bitwise & to determine leftmost non-zero bit
                                                        // Define another mask to keep track of only one leftmost bit which is continually right-shifted until
                                                        // the most significant bit is found.
            for (byte mask = 0b10000000; mask > 0; mask >>= 1, mostSignificantBitMask >>= 1)
            {
                if ((mostSignificantByte & mask) == mask)   // Most significant bit found
                    break;
            }

            while (true)
            {
                // Generate random array of bits
                rng.GetBytes(bytes);
                // Clear bits above the most significant bit (to ensure that the value is not automatically 
                // larger than the upper bound)
                bytes[bytes.Length - 1] &= mostSignificantBitMask;

                BigInteger result = new(bytes);
                if (result <= zeroBasedUpperBound)
                    return result + minBound;
            }
        }
    }
}
