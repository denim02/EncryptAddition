namespace EncryptAddition.Crypto
{
    /// <summary>
    /// IKeyPair defines a common method to be implemented
    /// by the encryption algorithm key pairs, all of which need to 
    /// be serializable as strings to be exported and re-imported.
    /// </summary>
    public interface IKeyPair
    {
        /// <summary>
        /// Serializes the keypair.
        /// </summary>
        /// <returns>
        /// Returns a string with semicolons seperating the two keys, 
        /// and the '|' symbol seperating key members.
        /// </returns>
        public string Serialize();
    }
}
