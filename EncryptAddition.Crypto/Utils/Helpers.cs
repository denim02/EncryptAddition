using System.Numerics;
using System.Security.Cryptography;

namespace EncryptAddition.Crypto.Utils
{
    public static class Helpers
    {
        /// <summary>
        /// Multiplies two BigInteger values and returns the result modulo the specified modulus.
        /// </summary>
        /// <returns>The modular multiplication of the two values.</returns>
        /// <param name="firstOperand">The left operand of the multiplication.</param>
        /// <param name="secondOperand">The right operand of the multiplication.</param>
        /// <param name="modulus">The modulus. Must be positive.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the value of the modulus is negative.</exception>
        public static BigInteger ModMul(BigInteger firstOperand, BigInteger secondOperand, BigInteger modulus)
        {
            // Check if provided modulus is negative
            if (modulus <= 0)
                throw new ArgumentOutOfRangeException(nameof(modulus), "The modulus must be positive.");

            return ((firstOperand % modulus) * (secondOperand % modulus)) % modulus;
        }

        /// <summary>
        /// Generates a random BigInteger value within the specified range (inclusive).
        /// Only positive bounds are permitted.
        /// </summary>
        /// <returns>The random BigInteger generated between the bounds.</returns>
        /// <param name="minBound">The lower bound for the number (inclusive).</param>
        /// <param name="maxBound">The upper bound for the number (inclusive).</param>
        /// <exception cref="ArgumentException">Thrown when the minimum bound is greater than the maximum bound.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when either the minimum or maximum bound is negative.</exception>
        public static BigInteger GetBigInteger(BigInteger minBound, BigInteger maxBound)
        {
            // Trivial cases
            if (minBound < 0)
                throw new ArgumentOutOfRangeException(nameof(minBound), "The minimum bound must be positive.");
            if (maxBound < 0)
                throw new ArgumentOutOfRangeException(nameof(maxBound), "The maximum bound must be positive.");
            if (minBound > maxBound)
                throw new ArgumentException("The minimum bound cannot be larger than the maximum bound.", nameof(minBound));
            if (minBound == maxBound)
                return minBound;

            // Determine the zero-based upper bound (to reduce number of bits needed to work with).
            BigInteger zeroBasedUpperBound = maxBound - minBound;
            byte[] bytes = zeroBasedUpperBound.ToByteArray();
            byte mostSignificantByte = bytes[^1];

            // Search for the most significant non-zero bit
            // Will be used with bitwise & to determine leftmost non-zero bit
            byte mostSignificantBitMask = 0b11111111;

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
                bytes = RandomNumberGenerator.GetBytes(bytes.Length);
                // Clear bits above the most significant bit (to ensure that the value is not automatically 
                // larger than the upper bound)
                bytes[^1] &= mostSignificantBitMask;

                BigInteger result = new(bytes);
                if (result <= zeroBasedUpperBound)
                    return result + minBound;
            }
        }
    }
}
