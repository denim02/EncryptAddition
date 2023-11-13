using EncryptAddition.Crypto.Utils;
using System.Numerics;

namespace EncryptAddition.Crypto.ElGamal
{
    /// <summary>
    /// Defines the ElGamal encryption algorithm.
    /// </summary>
    public class ElGamalEncryption : IEncryptionStrategy
    {
        /// <summary>
        /// Members of the ElGamalEncryption class
        /// </summary>

        // The bit length used to generate the prime for ElGamal
        public int PrimeBitLength { get; private set; }

        // The created or provided key pair for the algorithm
        public KeyPair KeyPair { get; private set; }

        // The maximum value for BigInteger values that can be encrypted
        public BigInteger MaxPlaintextSize => KeyPair.PublicKey.Prime - 2;


        /// <summary>
        /// Instantiates a new instance of the ElGamalEncryption class
        /// with a prime created using the passed bit length. An exception
        /// will occur if the bit length is less than 3.
        /// </summary>
        /// <param name="primeBitLength">The bit length for prime generation.</param>
        public ElGamalEncryption(int primeBitLength)
        {
            SetPrimeBitLength(primeBitLength);
            RegenerateKeys();
        }

        /// <summary>
        /// Instantiates an object of the ElGamalEncryption class
        /// with the passed in key pair. An exception will occur
        /// if the key pair is not valid.
        /// </summary>
        /// <param name="keyPair">An ElGamal key pair.</param>
        public ElGamalEncryption(KeyPair keyPair)
        {
            ValidateAndSetKeyPair(keyPair);
            SetPrimeBitLength((int)KeyPair!.PublicKey.Prime.GetBitLength());
        }

        /// <summary>
        /// Changes the set bit length for the prime modulus.
        /// Must call RegenerateKeys() to refresh the keys with 
        /// the new bit length.
        /// </summary>
        /// <param name="primeBitLength">The new desired bit length.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the bit length is less than 3.</exception>
        public void SetPrimeBitLength(int primeBitLength)
        {
            if (primeBitLength < 3)
                throw new ArgumentOutOfRangeException(nameof(primeBitLength), "The specified bit length for prime generation must be greater than or equal to 3, as the smallest usable prime for ElGamal is 5.");

            PrimeBitLength = primeBitLength;
        }

        /// <summary>
        /// Validates and sets a passed in key pair according to the rules
        /// for ElGamal keys.
        /// </summary>
        /// <param name="keyPair">An ElGamal key pair.</param>
        /// <exception cref="ArgumentException">Thrown if the key pair values are invalid.</exception>
        private void ValidateAndSetKeyPair(KeyPair keyPair)
        {
            // Ensure that the values within the key pair are valid and can be used for
            // encryption/ decryption

            // Easiest checks - everything must be less than the prime
            if (keyPair.PublicKey.Generator > keyPair.PublicKey.Prime)
                throw new ArgumentException("Invalid public key. The generator is greater than the prime.");
            if (keyPair.PublicKey.Beta > keyPair.PublicKey.Prime)
                throw new ArgumentException("Invalid public key. Beta is greater than the prime.");
            if (keyPair.PrivateKey > keyPair.PublicKey.Prime - 2 || keyPair.PrivateKey < 2)
                throw new ArgumentException("Invalid private key");

            // Check if the prime is actually a safe prime
            if (!keyPair.PublicKey.Prime.IsProbablePrime())
                throw new ArgumentException("Invalid public key. The order of the group is not prime.");

            BigInteger sophieGermainPrime = (keyPair.PublicKey.Prime - 1) / 2;
            if (!sophieGermainPrime.IsProbablePrime())
                throw new ArgumentException("Invalid public key. The prime is not a safe prime.");

            // Check if the generator is valid
            if (BigInteger.ModPow(keyPair.PublicKey.Generator, 2, keyPair.PublicKey.Prime) == 1 || BigInteger.ModPow(keyPair.PublicKey.Generator, sophieGermainPrime, keyPair.PublicKey.Prime) == 1)
                throw new ArgumentException("Invalid public key. The provided value is not a generator for the group defined by the prime.");

            // Check if beta is valid
            if (keyPair.PublicKey.Beta != BigInteger.ModPow(keyPair.PublicKey.Generator, keyPair.PrivateKey, keyPair.PublicKey.Prime))
                throw new ArgumentException("Invalid public key. The provided beta is not correct.");

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
            BigInteger prime = Primality.GenerateSafePrime(PrimeBitLength);
            BigInteger generator = CyclicMath.FindGeneratorForSafePrime(prime);

            var privateKey = Helpers.GetBigInteger(2, prime - 2);
            var beta = BigInteger.ModPow(generator, privateKey, prime);

            var publicKey = new PublicKey(prime, generator, beta);

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
            if (input < 0 || input > MaxPlaintextSize)
                throw new ArgumentOutOfRangeException(nameof(input), $"The input must be within the range [0, {MaxPlaintextSize}].");

            BigInteger k = Helpers.GetBigInteger(2, KeyPair.PublicKey.Prime - 2);
            BigInteger sharedSecret = BigInteger.ModPow(KeyPair.PublicKey.Generator, k, KeyPair.PublicKey.Prime);
            BigInteger encryptedMessage = Helpers.ModMul(
                BigInteger.ModPow(
                    KeyPair.PublicKey.Beta,
                    k,
                    KeyPair.PublicKey.Prime),
                BigInteger.ModPow(KeyPair.PublicKey.Generator, input, KeyPair.PublicKey.Prime)
                , KeyPair.PublicKey.Prime);

            return new CipherText(encryptedMessage, sharedSecret);
        }

        /// <summary>
        /// Decrypts a given ciphertext and returns the plaintext.
        /// </summary>
        /// <param name="cipher">An ElGamal cipher.</param>
        /// <returns>The corresponding plaintext.</returns>
        /// <exception cref="ArgumentException">Thrown if a Paillier ciphertext is passed in.</exception>
        public BigInteger Decrypt(CipherText cipher)
        {
            if (!cipher.SharedSecret.HasValue)
                throw new ArgumentException("Invalid cipher text entered. ElGamal requires the cipher to contain a shared secret.");

            BigInteger val = Helpers.ModMul(
                cipher.EncryptedMessage,
                CyclicMath.PrimeModInverse(
                    BigInteger.ModPow(
                        cipher.SharedSecret.Value,
                        KeyPair.PrivateKey,
                        KeyPair.PublicKey.Prime
                    ),
                    KeyPair.PublicKey.Prime
                ),
                KeyPair.PublicKey.Prime
            );

            return CyclicMath.DiscreteLog(KeyPair.PublicKey.Generator, val, KeyPair.PublicKey.Prime - 1);
        }

        /// <summary>
        /// Add an arbitrary number of ciphers using homomorphic addition.
        /// No validation is done to ensure sum does not exceed max plaintext size,
        /// so caution must be exercised.
        /// </summary>
        /// <param name="ciphers">An arbitrary number of ElGamal cipher values.</param>
        /// <returns>The sum of the ciphertexts as a cipher.</returns>
        /// <exception cref="InvalidOperationException">Thrown if called with no arguments.</exception>
        /// <exception cref="ArgumentException">Thrown if a Paillier cipher is passed in.</exception>
        public CipherText Add(params CipherText[] ciphers)
        {
            if (ciphers.Length == 0)
                throw new InvalidOperationException("At least one value must be passed to the function.");

            if (ciphers.Any((cipher) => !cipher.SharedSecret.HasValue))
                throw new ArgumentException("Invalid cipher text entered. ElGamal requires the cipher to contain a shared secret.");

            if (ciphers.Length == 1)
                return ciphers[0];

            BigInteger sharedSecret = ciphers.Select(cipher => cipher.SharedSecret!.Value).Aggregate((secret1, secret2) => Helpers.ModMul(secret1, secret2, KeyPair.PublicKey.Prime));

            BigInteger encryptedMessage = ciphers.Select(cipher => cipher.EncryptedMessage).Aggregate((encryptedMsg1, encryptedMsg2) => Helpers.ModMul(encryptedMsg1, encryptedMsg2, KeyPair.PublicKey.Prime));

            return new CipherText(encryptedMessage, sharedSecret);
        }

        /// <summary>
        /// Return the ElGamal keys in serialized form.
        /// </summary>
        /// <returns>A string with the serialized keys.</returns>
        public string PrintKeys() => KeyPair.Serialize();
    }
}
