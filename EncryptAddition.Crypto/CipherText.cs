using System.Numerics;
using System.Text.RegularExpressions;

namespace EncryptAddition.Crypto
{
    /// <summary>
    /// The CipherText struct is defined to store any ciphertexts
    /// produced from encryption or homomorphic addition by either Paillier
    /// or ElGamal.
    /// </summary>
    public readonly struct CipherText
    {
        // Define the proper validation format for deserialization
        private static readonly Regex _correctFormat = new(@"^\d+(\|\d+)?$");

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

        /// <summary>
        /// Returns a string representation of the ciphertext. If both a shared secret and an encrypted message
        /// are present, they will be seperated by the '|' character.
        /// </summary>
        /// <returns>String representation of the cipher.</returns>
        public override string ToString()
        {
            return SharedSecret.HasValue ? $"{EncryptedMessage}|{SharedSecret.Value}" : EncryptedMessage.ToString();
        }

        /// <summary>
        /// Deserialize a string representaion of the ciphertext.
        /// </summary>
        /// <param name="serializedCipherText">The serialized CipherText object.</param>
        /// <returns>A new CipherText instance.</returns>
        public static CipherText Deserialize(string serializedCipherText)
        {
            if (!_correctFormat.IsMatch(serializedCipherText))
                throw new ArgumentException("The provided ciphertext is in an incorrect format.");

            var cipherParts = serializedCipherText.Split("|").Select(x => BigInteger.Parse(x.Trim())).ToArray();

            if (cipherParts.Length == 1)
                return new CipherText(cipherParts[0]);

            return new CipherText(cipherParts[0], cipherParts[1]);
        }
    }
}
