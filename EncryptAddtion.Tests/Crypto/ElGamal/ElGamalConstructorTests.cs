using EncryptAddition.Crypto.ElGamal;

namespace EncryptAddtion.Tests.Crypto.ElGamal
{
    [TestClass]
    public class ElGamalConstructorTests
    {
        [TestMethod]
        public void ElGamalConstructor_WithCorrectBitLength()
        {
            int bitLength = 3;
            var elGamal = new ElGamalEncryption(bitLength);

            Assert.AreEqual(elGamal.PrimeBitLength, bitLength);
            Assert.IsNotNull(elGamal.KeyPair);
            Assert.AreEqual(elGamal.KeyPair.PublicKey.Prime.GetBitLength(), bitLength);
            Assert.AreEqual(elGamal.KeyPair.PublicKey.Prime - 2, elGamal.MaxPlaintextSize);
        }

        [TestMethod]
        public void ElGamalConstructor_WithIncorrectBitLength()
        {
            int bitLength = 2;
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new ElGamalEncryption(bitLength));
        }

        [TestMethod]
        public void ElGamalConstructor_WithInvalidKey()
        {
            // Create dummy key
            string serializedKeys = "100|200|300;400";
            var keys = new KeyPair(serializedKeys);

            Assert.ThrowsException<ArgumentException>(() => new ElGamalEncryption(keys));
        }

        [TestMethod]
        public void ElGamalConstructor_WithKeyWithSmallPrime()
        {
            // Create dummy key
            string serializedKeys = "5|2|4;2";
            var keys = new KeyPair(serializedKeys);

            var elGamal = new ElGamalEncryption(keys);

            Assert.IsNotNull(elGamal.KeyPair);
            Assert.AreEqual(elGamal.KeyPair, keys);
            Assert.AreEqual(elGamal.PrimeBitLength, new BigInteger(5).GetBitLength());
            Assert.AreEqual(elGamal.MaxPlaintextSize, 3);
        }
    }
}
