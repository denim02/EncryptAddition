using System.Numerics;
using System.Security.Cryptography;

namespace EncryptAddition.Crypto.Utils
{
    public static class Helpers
    {
        public static BigInteger ModMul(BigInteger firstOperand, BigInteger secondOperand, BigInteger modulus)
        {
            return ((firstOperand % modulus) * (secondOperand % modulus)) % modulus;
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
        public static BigInteger GetBigInteger(BigInteger minBound, BigInteger maxBound)
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
                bytes = RandomNumberGenerator.GetBytes(bytes.Length);
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
