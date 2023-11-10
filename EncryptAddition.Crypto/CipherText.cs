using System.Numerics;

namespace EncryptAddition.Crypto
{
    /// <summary>
    /// The CipherText struct is defined to store any ciphertexts
    /// produced from encryption or homomorphic addition by either Paillier
    /// or ElGamal.
    /// </summary>
    public readonly struct CipherText
    {
        public BigInteger EncryptedMessage { get; }

        // SharedSecret is only used for ElGamal, appropriate checks are included
        public readonly BigInteger? SharedSecret { get; }

        /// <summary>
        /// Instantiates a new CipherText object with the provided encrypted message.
        /// The shared secret used by ElGamal is set to null.
        /// Usable by Paillier.
        /// </summary>
        /// <param name="encryptedMessage">The encrypted message.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the encryptedMessage is negative.</exception>
        public CipherText(BigInteger encryptedMessage)
        {
            SharedSecret = null;
            EncryptedMessage = (encryptedMessage >= 0) ? encryptedMessage : throw new ArgumentOutOfRangeException(nameof(encryptedMessage), "The encrypted message of the ciphertext must not be negative.");
        }

        /// <summary>
        /// Instantiates a new CipherText object.
        /// Usable by ElGamal.
        /// </summary>
        /// <param name="encryptedMessage">The encrypted message.</param>
        /// <param name="sharedSecret">The shared secret used by ElGamal.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if either parameter is negative.</exception>
        public CipherText(BigInteger encryptedMessage, BigInteger sharedSecret)
        {
            SharedSecret = (sharedSecret >= 0) ? sharedSecret : throw new ArgumentOutOfRangeException(nameof(sharedSecret), "The shared secret of the ciphertext must not be negative.");
            EncryptedMessage = (encryptedMessage >= 0) ? encryptedMessage : throw new ArgumentOutOfRangeException(nameof(encryptedMessage), "The encrypted message of the ciphertext must not be negative.");
        }
    }
}
