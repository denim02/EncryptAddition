using EncryptAddition.Crypto.ElGamal;

namespace EncryptAddtion.Tests.Crypto.ElGamal
{
    [TestClass]
    public class ElGamalKeyPairTests
    {
        [TestMethod]
        public void KeyPair_ConstructorWithSerializedKeys()
        {
            var prime = new BigInteger(23);
            var generator = new BigInteger(5);
            var beta = new BigInteger(18);

            var privateKey = new BigInteger(15);

            string serializedKeys = $"{prime}|{generator}|{beta};{privateKey}";

            var keyPair = new KeyPair(serializedKeys);

            Assert.AreEqual(prime, keyPair.PublicKey.Prime);
            Assert.AreEqual(generator, keyPair.PublicKey.Generator);
            Assert.AreEqual(beta, keyPair.PublicKey.Beta);
            Assert.AreEqual(privateKey, keyPair.PrivateKey);
        }

        [TestMethod]
        public void KeyPair_ConstructorWithInvalidSerializedKeys()
        {
            string invalidSerializedKeys = "Not a valid key string";

            Assert.ThrowsException<ArgumentException>(() => new KeyPair(invalidSerializedKeys));
        }

        [TestMethod]
        public void KeyPair_ConstructorWithPublicKeyAndPrivateKey()
        {
            var prime = new BigInteger(23);
            var generator = new BigInteger(5);
            var beta = new BigInteger(18);

            var privateKey = new BigInteger(15);
            var publicKey = new PublicKey(prime, generator, beta);

            var keyPair = new KeyPair(publicKey, privateKey);

            Assert.AreEqual(publicKey, keyPair.PublicKey);
            Assert.AreEqual(privateKey, keyPair.PrivateKey);
        }

        [TestMethod]
        public void KeyPair_Serialize()
        {
            var prime = new BigInteger(23);
            var generator = new BigInteger(5);
            var beta = new BigInteger(18);

            var privateKey = new BigInteger(15);
            var publicKey = new PublicKey(prime, generator, beta);

            var keyPair = new KeyPair(publicKey, privateKey);

            string serializedKeyPair = keyPair.Serialize();

            string expected = $"{prime}|{generator}|{beta};{privateKey}";
            Assert.AreEqual(expected, serializedKeyPair);
        }

        [TestMethod]
        public void KeyPair_DeserializeWithProperFormat()
        {
            var prime = new BigInteger(23);
            var generator = new BigInteger(5);
            var beta = new BigInteger(18);
            var privateKey = new BigInteger(15);

            string serializedKeys = $"{prime}|{generator}|{beta};{privateKey}";

            var keyPair = KeyPair.Deserialize(serializedKeys);

            Assert.AreEqual(prime, keyPair.PublicKey.Prime);
            Assert.AreEqual(generator, keyPair.PublicKey.Generator);
            Assert.AreEqual(beta, keyPair.PublicKey.Beta);
            Assert.AreEqual(privateKey, keyPair.PrivateKey);
        }

        [TestMethod]
        public void KeyPair_DeserializeWithInvalidFormat()
        {
            string invalidSerializedKeys = "InvalidFormat";

            Assert.ThrowsException<ArgumentException>(() => KeyPair.Deserialize(invalidSerializedKeys));
        }

        [TestMethod]
        public void KeyPair_WithLargeValues()
        {
            var prime = BigInteger.Parse("123123123123123123123");
            var generator = BigInteger.Parse("123123123123123445656");
            var beta = BigInteger.Parse("123123123123123789789");

            var privateKey = BigInteger.Parse("123123123123123012012");

            string serializedFormat = $"{prime}|{generator}|{beta};{privateKey}";

            var keyPair = new KeyPair(serializedFormat);

            Assert.AreEqual(keyPair.Serialize(), serializedFormat);
        }
    }
}
