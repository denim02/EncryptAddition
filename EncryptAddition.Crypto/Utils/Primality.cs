using System.Numerics;

namespace EncryptAddition.Crypto.Utils
{
    public static class Primality
    {
        /// <summary>
        /// Generates a safe prime, that is a prime of the form 2p + 1 where p is also prime.
        /// </summary>
        /// <returns>The generated safe prime.</returns>
        /// <param name="bitLength">The bit length for the prime to be generated. Must be greater than 2.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the bit length is not a positive integer.</exception>
        public static BigInteger GenerateSafePrime(int bitLength)
        {
            // Check if the provided bit length is valid
            if (bitLength < 3)
                throw new ArgumentOutOfRangeException(nameof(bitLength), "The bit length must be greater than 2.");

            // Generate random primes until a safe prime is found
            while (true)
            {
                // To generate the prime, use bitLength - 1 since the prime will be of the form 2p + 1
                BigInteger p = GeneratePrime(bitLength - 1);

                // Determine if the argument is a Sophie Germain prime
                BigInteger q = 2 * p + 1;

                // There's still a chance the desired bit length was not produced so check again
                if (q.IsProbablePrime() && q.GetBitLength() == bitLength)
                    return q;
            }

        }

        /// <summary>
        /// Generates a prime with the provided bit length and returns it.
        /// </summary>
        /// <returns>A random prime number with the specified bit length.</returns>
        /// <param name="bitLength">The bit length for the prime to be generated. Must be greater than 1.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the provided bit length is 1 or less.</exception>
        public static BigInteger GeneratePrime(int bitLength)
        {
            // Check if the provided bit length is valid
            if (bitLength < 2)
                throw new ArgumentOutOfRangeException(nameof(bitLength), "The bit length must be greater than 1.");

            // Define the upper and lower bounds for the prime so it is within the specified bit length
            BigInteger lowerBound = BigInteger.One << (bitLength - 1);
            // Must decrease upper bound by 1 since GetBigInteger is inclusive and could otherwise generated
            // a number with a bit length one greater than the specified bit length.
            BigInteger upperBound = (BigInteger.One << bitLength) - 1;

            // Generate random numbers until a prime is found
            while (true)
            {
                var candidate = Helpers.GetBigInteger(lowerBound, upperBound);
                if (candidate.IsProbablePrime()) return candidate;
            }
        }

        /// <summary>
        /// Tests a BigInteger value using the Miller-Rabin primality test and returns true if it is a prime
        /// and false if not. The probability for a false positive is 4^(-k) where k is the number of iterations.
        /// </summary>
        /// <returns>True if the value is prime, False if not.</returns>
        /// <param name="candidate">The value to test for primality.</param>
        /// <param name="certainty">Number of iterations of the test. Must be positive.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the provided certainty is less than 1.</exception>
        public static bool IsProbablePrime(this BigInteger candidate, int certainty = 5)
        {
            // Validate certainty
            if (certainty < 1)
                throw new ArgumentOutOfRangeException(nameof(certainty), "The number of iterations must be a positive integer.");

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
            BigInteger a;

            // Perform the Miller-Robin primality test *certainty* times.
            for (int i = 0; i < certainty; i++)
            {
                a = Helpers.GetBigInteger(2, candidate - 2);

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
    }
}
