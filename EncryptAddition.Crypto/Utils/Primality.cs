using System.Numerics;

namespace EncryptAddition.Crypto.Utils
{
    public static class Primality
    {
        public static BigInteger GenerateSafePrime(int bitLength)
        {
            while (true)
            {
                BigInteger p = GeneratePrime(bitLength);
                // Determine if the argument is a Sophie Germain prime
                BigInteger q = 2 * p + 1;
                if (q.IsProbablePrime())
                    return q;
            }

        }

        // Naive prime generation algorithm
        public static BigInteger GeneratePrime(int bitLength)
        {
            while (true)
            {
                var candidate = Helpers.GetBigInteger(BigInteger.One << (bitLength - 1), BigInteger.One << bitLength);
                if (candidate.IsProbablePrime()) return candidate;
            }
        }

        /// <summary>
        /// Returns a boolean value indiciating whether the provided BigInteger value is a prime or not.
        /// The test checks for primality probabilistically using the Miller-Rabin primality test.
        /// The probability for a false positive is 4^(-k) where k is the number of iterations.
        /// If only one iteration is used, the probability of a correct results would be 75%.
        /// </summary>
        /// <param name="certainty">Number of iterations of the test</param>
        /// <returns>true if the value is prime, false if not</returns>
        public static bool IsProbablePrime(this BigInteger candidate, int certainty = 5)
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
