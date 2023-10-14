namespace EncryptAddition.Crypto
{
    internal interface IAdditivelyHomomorphic<CipherText>
    {
        public CipherText Add(params CipherText[] ciphers);
    }
}
