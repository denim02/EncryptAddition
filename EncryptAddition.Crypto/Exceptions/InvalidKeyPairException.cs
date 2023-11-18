namespace EncryptAddition.Crypto.Exceptions
{
    public class InvalidKeyPairException : Exception
    {
        public string SerializedKeyPair { get; set; }

        public InvalidKeyPairException(string seiralizedKeyPair)
        {
            SerializedKeyPair = seiralizedKeyPair;
        }

        public InvalidKeyPairException(string seiralizedKeyPair, string message) : base(message)
        {
            SerializedKeyPair = seiralizedKeyPair;
        }

        public InvalidKeyPairException(string seiralizedKeyPair, string message, Exception innerException) : base(message, innerException)
        {
            SerializedKeyPair = seiralizedKeyPair;
        }
    }
}
