using EncryptAddition.Crypto.Utils;
using System.Numerics;

namespace EncryptAddition.Crypto.Paillier
{
    /// <summary>
    /// Defines the Paillier encryption algorithm.
    /// </summary>
    public class PaillierEncryption : IEncryptionStrategy
    {
        /// <summary>
        /// Members of the PaillierEncryption class
        /// </summary>

        // The bit length used to generate the primes for Paillier,
        // may be missing if instantiated with a key pair
        public int? PrimeBitLength { get; private set; }

        // The created or provided key pair for the algorithm
        public KeyPair KeyPair { get; private set; }

        // The maximum value for BigInteger values that can be encrypted
        public BigInteger MaxPlaintextSize => KeyPair.PublicKey.N - 2;

        private BigInteger NSquared => KeyPair.PublicKey.N * KeyPair.PublicKey.N;

        /// <summary>
        /// Instantiates a new instance of the PaillierEncryption class
        /// with primes created using the passed bit length. An exception
        /// will occur if the bit length is less than 2.
        /// </summary>
        /// <param name="primeBitLength">The bit length for prime generation.</param>
        public PaillierEncryption(int primeBitLength)
        {
            SetPrimeBitLength(primeBitLength);
            RegenerateKeys();
        }

        /// <summary>
        /// Instantiates an object of the PaillierEncryption class
        /// with the passed in key pair. An exception will occur
        /// if the key pair is not valid.
        /// </summary>
        /// <param name="keyPair">A Paillier key pair.</param>
        public PaillierEncryption(KeyPair keyPair)
        {
            ValidateAndSetKeyPair(keyPair);
        }

        /// <summary>
        /// Changes the set bit length for the prime modulus.
        /// Must call RegenerateKeys() to refresh the keys with 
        /// the new bit length.
        /// </summary>
        /// <param name="primeBitLength">The new desired bit length.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the bit length is less than 2.</exception>
        public void SetPrimeBitLength(int primeBitLength)
        {
            if (primeBitLength < 2)
                throw new ArgumentOutOfRangeException(nameof(primeBitLength), "The specified bit length for prime generation must be greater than or equal to 2.");

            PrimeBitLength = primeBitLength;
        }

        /// <summary>
        /// Validates and sets a passed in key pair according to the rules
        /// for Paillier keys.
        /// </summary>
        /// <param name="keyPair">A Paillier key pair.</param>
        /// <exception cref="ArgumentException">Thrown if the key pair values are invalid.</exception>
        private void ValidateAndSetKeyPair(KeyPair keyPair)
        {
            // Ensure that the values within the key pair are valid and can be used for
            // encryption/ decryption
            if (keyPair.PublicKey.G != (keyPair.PublicKey.N + 1))
                throw new ArgumentException("Invalid public key. G is not equal to n + 1.");
            if (keyPair.PrivateKey.Mu != BigInteger.ModPow(keyPair.PrivateKey.Lambda, keyPair.PrivateKey.Lambda - 1, keyPair.PublicKey.N))
                throw new ArgumentException("Invalid private key.");

            KeyPair = keyPair;
        }

        private void SetKeyPair(KeyPair keyPair)
        {
            KeyPair = keyPair;
        }

        /// <summary>
        /// Refreshes the keys used by the algorithm by re-creating them using
        /// the set bit length.
        /// </summary>
        public void RegenerateKeys()
        {
            if (!PrimeBitLength.HasValue)
                throw new InvalidOperationException("Cannot re-generate the keys without first setting the desired prime bit length.");

            BigInteger p = Primality.GeneratePrime(PrimeBitLength.Value);
            BigInteger q = Primality.GeneratePrime(PrimeBitLength.Value);
            while (p == q)
                q = Primality.GeneratePrime(PrimeBitLength.Value);

            BigInteger n = p * q;
            BigInteger g = n + 1;
            BigInteger lambda = (p - 1) * (q - 1);
            BigInteger mu = BigInteger.ModPow(lambda, lambda - 1, n);

            PublicKey publicKey = new(n, g);
            PrivateKey privateKey = new(lambda, mu);
            SetKeyPair(new KeyPair(publicKey, privateKey));
        }

        /// <summary>
        /// Encrypts a BigInteger value and returns its cipher.
        /// </summary>
        /// <param name="input">A BigInteger plaintext.</param>
        /// <returns>The corresponding ciphertext.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the plaintext is too large to be encrypted.</exception>
        public CipherText Encrypt(BigInteger input)
        {
            // Check if the input is within the range of the public key
            if (input < 0 || input > MaxPlaintextSize)
                throw new ArgumentOutOfRangeException(nameof(input), $"The input must be within the range [0, {MaxPlaintextSize}].");

            BigInteger randomVal;

            do
            {
                randomVal = Helpers.GetBigInteger(1, KeyPair.PublicKey.N - 1);
            } while (BigInteger.GreatestCommonDivisor(randomVal, KeyPair.PublicKey.N) != BigInteger.One);

            return new CipherText(BigInteger.Multiply(BigInteger.ModPow(KeyPair.PublicKey.G, input, NSquared), BigInteger.ModPow(randomVal, KeyPair.PublicKey.N, NSquared)) % (NSquared));
        }

        /// <summary>
        /// Decrypts a given ciphertext and returns the plaintext.
        /// </summary>
        /// <param name="cipher">A Paillier cipher.</param>
        /// <returns>The corresponding plaintext.</returns>
        public BigInteger Decrypt(CipherText cipher)
        {

            BigInteger power = BigInteger.ModPow(cipher.EncryptedMessage, KeyPair.PrivateKey.Lambda, NSquared);
            BigInteger ratio = (power - 1) / KeyPair.PublicKey.N;
            BigInteger result = BigInteger.Multiply(ratio, KeyPair.PrivateKey.Mu) % KeyPair.PublicKey.N;

            return result;
        }

        /// <summary>
        /// Add an arbitrary number of ciphers using homomorphic addition.
        /// No validation is done to ensure sum does not exceed max plaintext size,
        /// so caution must be exercised.
        /// </summary>
        /// <param name="ciphers">An arbitrary number of Paillier cipher values.</param>
        /// <returns>The sum of the ciphertexts as a cipher.</returns>
        /// <exception cref="InvalidOperationException">Thrown if called with no arguments.</exception>
        public CipherText Add(params CipherText[] ciphers)
        {
            if (ciphers.Length == 0)
                throw new InvalidOperationException("At least one value must be passed to the function.");
            if (ciphers.Length == 1)
                return ciphers[0];

            return ciphers.Aggregate((cipher1, cipher2) => new CipherText(BigInteger.Multiply(cipher1.EncryptedMessage, cipher2.EncryptedMessage) % NSquared));
        }

        /// <summary>
        /// Return the Paillier keys in serialized form.
        /// </summary>
        /// <returns>A string with the serialized keys.</returns>
        public string PrintKeys() => KeyPair.Serialize();
    }
}
