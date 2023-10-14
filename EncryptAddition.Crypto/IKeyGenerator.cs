namespace EncryptAddition.Crypto
{
    internal interface IKeyGenerator<KeyPair>
    {
        KeyPair GenerateKeyPair();
    }
}
