using EncryptAddition.Crypto.Paillier;

namespace EncryptAddtion.Tests.Crypto.Paillier
{
    [TestClass]
    public class PaillierConstructorTests
    {
        [TestMethod]
        public void PaillierConstructor_WithCorrectBitLength()
        {
            int bitLength = 3;
            var paillier = new PaillierEncryption(bitLength);

            Assert.AreEqual(paillier.PrimeBitLength, bitLength);
            Assert.IsNotNull(paillier.KeyPair);
            Assert.AreEqual(paillier.KeyPair.PublicKey.N - 2, paillier.MaxPlaintextSize);
        }

        [TestMethod]
        public void PaillierConstructor_WithIncorrectBitLength()
        {
            int bitLength = 1;
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new PaillierEncryption(bitLength));
        }

        [TestMethod]
        public void PaillierConstructor_WithInvalidKey()
        {
            // Create dummy key
            string serializedKeys = "100|200;300|400";
            var keys = new KeyPair(serializedKeys);

            Assert.ThrowsException<ArgumentException>(() => new PaillierEncryption(keys));
        }

        [TestMethod]
        public void PaillierConstructor_WithKeyWithSmallPrime()
        {
            // Create dummy key
            string serializedKeys = "6|7;2|2";
            var keys = new KeyPair(serializedKeys);

            var paillier = new PaillierEncryption(keys);

            Assert.IsNotNull(paillier.KeyPair);
            Assert.AreEqual(paillier.KeyPair, keys);
            Assert.IsNull(paillier.PrimeBitLength);
            Assert.AreEqual(paillier.MaxPlaintextSize, 4);
        }
    }
}
