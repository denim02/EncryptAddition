using System.Numerics;
using System.Security.Cryptography;

namespace EncryptAddition.Crypto.Utils
{
    public static class Helpers
    {
        /// <summary>
        /// Calculates the ceiling of the integer square root of the provided BigInteger.
        /// To increase performance with this type, bitshifts are used instead of regular division operations.
        /// <example>
        /// For example, if square rooting 3, the result will be 2, since 2 is the smallest integer greater than or equal to the square root of 3.
        /// </example>
        /// </summary>
        /// <returns>The integer square root.</returns>
        /// <param name="number">A positive BigInteger value.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if a negative value argument is provided.</exception>
        public static BigInteger SqrtCeil(BigInteger number)
        {
            // Validate arguments
            if (number < 0)
                throw new ArgumentOutOfRangeException(nameof(number), "The number cannot be negative when determing its square root.");

            // Trivial cases
            if (number == 0)
                return 0;
            if (number == 1)
                return 1;
            if (number <= 4)
                return 2;
            if (number <= 9)
                return 3;

            // Establish bounds for binary search.
            BigInteger middle = 0, squaredMiddle = 0, lowerBound = 0;

            // Take half of the number as the upper bound (equivalent to one right shift),
            // since the square root of a number cannot be greater than half of the number.
            BigInteger upperBound = number >> 1;

            // Perform binary search to find the square root of the number.
            while (lowerBound <= upperBound)
            {
                // The middle is the sum of the bounds divided by two (equivalent to one right shift).
                middle = (upperBound + lowerBound) >> 1;

                // Calculate the square of the middle.
                squaredMiddle = BigInteger.Pow(middle, 2);

                // Re-evaluate the bounds based on the square of the middle.
                if (number < squaredMiddle)
                    upperBound = middle - 1;
                else if (number > squaredMiddle)
                    lowerBound = middle + 1;
                else
                    break; // The square root was found.
            }

            return squaredMiddle == number ? middle : lowerBound;
        }

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
