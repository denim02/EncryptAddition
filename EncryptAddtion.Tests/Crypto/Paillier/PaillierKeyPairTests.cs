using EncryptAddition.Crypto.Paillier;

namespace EncryptAddtion.Tests.Crypto.Paillier
{
    [TestClass]
    public class PaillierKeyPairTests
    {
        [TestMethod]
        public void KeyPair_ConstructorWithSerializedKeys()
        {
            var n = new BigInteger(10);
            var g = new BigInteger(20);
            var lambda = new BigInteger(30);
            var mu = new BigInteger(40);

            string serializedKeys = $"{n}|{g};{lambda}|{mu}";

            var keyPair = new KeyPair(serializedKeys);

            Assert.AreEqual(n, keyPair.PublicKey.N);
            Assert.AreEqual(g, keyPair.PublicKey.G);
            Assert.AreEqual(lambda, keyPair.PrivateKey.Lambda);
            Assert.AreEqual(mu, keyPair.PrivateKey.Mu);
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
            var n = new BigInteger(10);
            var g = new BigInteger(20);
            var lambda = new BigInteger(30);
            var mu = new BigInteger(40);

            var publicKey = new PublicKey(n, g);
            var privateKey = new PrivateKey(lambda, mu);

            var keyPair = new KeyPair(publicKey, privateKey);

            Assert.AreEqual(publicKey, keyPair.PublicKey);
            Assert.AreEqual(privateKey, keyPair.PrivateKey);
        }

        [TestMethod]
        public void KeyPair_Serialize()
        {
            var n = new BigInteger(10);
            var g = new BigInteger(20);
            var lambda = new BigInteger(30);
            var mu = new BigInteger(40);

            var publicKey = new PublicKey(n, g);
            var privateKey = new PrivateKey(lambda, mu);

            var keyPair = new KeyPair(publicKey, privateKey);

            string serializedKeyPair = keyPair.Serialize();

            string expected = $"{n}|{g};{lambda}|{mu}";
            Assert.AreEqual(expected, serializedKeyPair);
        }

        [TestMethod]
        public void KeyPair_DeserializeWithProperFormat()
        {
            var n = new BigInteger(10);
            var g = new BigInteger(20);
            var lambda = new BigInteger(30);
            var mu = new BigInteger(40);

            string serializedKeys = $"{n}|{g};{lambda}|{mu}";

            var keyPair = KeyPair.Deserialize(serializedKeys);

            Assert.AreEqual(n, keyPair.PublicKey.N);
            Assert.AreEqual(g, keyPair.PublicKey.G);
            Assert.AreEqual(lambda, keyPair.PrivateKey.Lambda);
            Assert.AreEqual(mu, keyPair.PrivateKey.Mu);
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
            var n = BigInteger.Parse("123123123123123123123");
            var g = BigInteger.Parse("123123123123123445656");

            var lambda = BigInteger.Parse("123123123123123789789");
            var mu = BigInteger.Parse("123123123123123012012");

            string serializedKeys = $"{n}|{g};{lambda}|{mu}";

            var keyPair = new KeyPair(serializedKeys);

            Assert.AreEqual(keyPair.Serialize(), serializedKeys);
        }

        [TestMethod]
        public void KeyPair_ValidateSerializedKeys()
        {
            string serializedKeyCorrect = "123|123;123|123";
            string serializedKeyInvalid = "123|123|123;123";

            Assert.IsFalse(KeyPair.ValidateSerializedKeys(serializedKeyInvalid));
            Assert.IsTrue(KeyPair.ValidateSerializedKeys(serializedKeyCorrect));
        }
    }
}
