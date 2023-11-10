using System.Numerics;
using System.Text.RegularExpressions;

namespace EncryptAddition.Crypto.Paillier
{
    /// <summary>
    /// The KeyPair class for the Paillier algorithm.
    /// </summary>
    public class KeyPair : IKeyPair
    {
        // Define the proper validation format for deserialization
        private readonly static Regex correctFormat = new(@"^\d+\|\d+;\d+\|\d+$");

        // Properties to hold the private and public keys
        public PublicKey PublicKey { get; }
        public PrivateKey PrivateKey { get; }

        /// <summary>
        /// Instantiates a new Paillier KeyPair object based on the serialized
        /// keys provided as a string. An exception occurs if the invalid format
        /// is used.
        /// </summary>
        /// <param name="serializedKeys">The serialized Paillier keys.</param>
        public KeyPair(string serializedKeys)
        {
            KeyPair pair = Deserialize(serializedKeys);
            PublicKey = pair.PublicKey;
            PrivateKey = pair.PrivateKey;
        }

        /// <summary>
        /// Instantiates a new Paillier KeyPair using Public and PrivateKey objects.
        /// </summary>
        /// <param name="publicKey">The Paillier PublicKey object.</param>
        /// <param name="privateKey">The Paillier PrivateKey object.</param>
        public KeyPair(PublicKey publicKey, PrivateKey privateKey)
        {
            PublicKey = publicKey;
            PrivateKey = privateKey;
        }

        public string Serialize()
        {
            return $"{PublicKey.N}|{PublicKey.G};{PrivateKey.Lambda}|{PrivateKey.Mu}";
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
            if (!correctFormat.IsMatch(serializedKeys))
                throw new ArgumentException("The provided keys are in an incorrect format.", nameof(serializedKeys));

            string[] seperatedKeys = serializedKeys.Split(';');

            BigInteger[] publicKeyProperties = seperatedKeys[0].Split("|").Select(str => BigInteger.Parse(str)).ToArray();
            BigInteger[] privateKeyProperties = seperatedKeys[1].Split("|").Select(str => BigInteger.Parse(str)).ToArray();

            return new KeyPair(
                new PublicKey(publicKeyProperties[0], publicKeyProperties[1]),
                new PrivateKey(privateKeyProperties[0], privateKeyProperties[1])
                );
        }
    }

    public readonly struct PublicKey
    {
        public BigInteger N { get; }
        public BigInteger G { get; }

        public PublicKey(BigInteger primeMultiple, BigInteger randomValue)
        {
            N = primeMultiple;
            G = randomValue;
        }
    }

    public readonly struct PrivateKey
    {
        public BigInteger Lambda { get; }
        public BigInteger Mu { get; }

        public PrivateKey(BigInteger lambda, BigInteger mu)
        {
            Lambda = lambda;
            Mu = mu;
        }
    }
}
