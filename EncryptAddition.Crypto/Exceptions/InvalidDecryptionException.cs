namespace EncryptAddition.Crypto.Exceptions
{
    public class InvalidDecryptionException : Exception
    {
        public CipherText CipherText { get; set; }

        public InvalidDecryptionException(CipherText cipherText)
        {
            CipherText = cipherText;
        }

        public InvalidDecryptionException(CipherText cipherText, string message) : base(message)
        {
            CipherText = cipherText;
        }

        public InvalidDecryptionException(CipherText cipherText, string message, Exception innerException) : base(message, innerException)
        {
            CipherText = cipherText;
        }
    }
}
