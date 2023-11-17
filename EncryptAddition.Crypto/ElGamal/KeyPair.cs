using System.Numerics;
using System.Text.RegularExpressions;

namespace EncryptAddition.Crypto.ElGamal
{
    /// <summary>
    /// The KeyPair class for the ElGamal algorithm.
    /// </summary>
    public class KeyPair : IKeyPair
    {
        // Define the proper validation format for deserialization
        private readonly static Regex _correctFormat = new(@"^\d+\|\d+\|\d+;\d+$");

        // Properties to hold the private and public keys
        public BigInteger PrivateKey { get; }
        public PublicKey PublicKey { get; }

        /// <summary>
        /// Instantiates a new ElGamal KeyPair object based on the serialized
        /// keys provided as a string. An exception occurs if the invalid format
        /// is used.
        /// </summary>
        /// <param name="serializedKeys">The serialized ElGamal keys.</param>
        public KeyPair(string serializedKeys)
        {
            KeyPair pair = Deserialize(serializedKeys);
            PublicKey = pair.PublicKey;
            PrivateKey = pair.PrivateKey;
        }

        /// <summary>
        /// Instantiates a new ElGamal KeyPair using the PublicKey object and a BigInteger
        /// private key.
        /// </summary>
        /// <param name="publicKey">The ElGamal PublicKey object.</param>
        /// <param name="privateKey">The ElGamal private key as a BigInteger.</param>
        public KeyPair(PublicKey publicKey, BigInteger privateKey)
        {
            PublicKey = publicKey;
            PrivateKey = privateKey;
        }

        public string Serialize()
        {
            return $"{PublicKey.Prime}|{PublicKey.Generator}|{PublicKey.Beta};{PrivateKey}";
        }

        /// <summary>
        /// Instantiates and returns an instance of the KeyPair class based on
        /// the serialized key string.
        /// </summary>
        /// <param name="serializedKeys">The serialized KeyPair keys.</param>
        /// <returns>A new KeyPair instance.</returns>
        /// <exception cref="ArgumentException">Thrown if the serialized string does not match the correct format.</exception>
        public static KeyPair Deserialize(string serializedKeys)
        {
            // Check for proper format with RegEx
            if (!_correctFormat.IsMatch(serializedKeys))
                throw new ArgumentException("The provided keys are in an incorrect format.", nameof(serializedKeys));

            string[] seperatedKeys = serializedKeys.Split(';');

            BigInteger[] publicKeyProperties = seperatedKeys[0].Split("|").Select(str => BigInteger.Parse(str)).ToArray();
            BigInteger privateKey = BigInteger.Parse(seperatedKeys[1]);

            return new KeyPair(
                new PublicKey(publicKeyProperties[0], publicKeyProperties[1], publicKeyProperties[2]),
                privateKey
            );
        }

        /// <summary>
        /// Checks if the provided string matches the expected format for serialized ElGamal keys.
        /// </summary>
        /// <param name="serializedKeys">The serialized KeyPair keys.</param>
        /// <returns>A boolean result.</returns>
        public static bool ValidateSerializedKeys(string serializedKeys)
        {
            return _correctFormat.IsMatch(serializedKeys);
        }
    }

    public readonly struct PublicKey
    {
        public BigInteger Prime { get; }
        public BigInteger Generator { get; }
        public BigInteger Beta { get; }

        public PublicKey(BigInteger prime, BigInteger generator, BigInteger beta)
        {
            Prime = prime;
            Generator = generator;
            Beta = beta;
        }
    }
}
