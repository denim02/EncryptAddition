using EncryptAddition.Crypto.Paillier;

namespace EncryptAddtion.Tests.Crypto.Paillier
{
    [TestClass]
    public class PaillierRegenerateKeysTests
    {
        [TestMethod]
        public void PaillierRegenerateKeys_WithoutPrimeBitLength()
        {
            var keyPair = KeyPair.Deserialize("6|7;2|2");
            var paillier = new PaillierEncryption(keyPair);

            Assert.IsNull(paillier.PrimeBitLength);
            Assert.ThrowsException<InvalidOperationException>(() => paillier.RegenerateKeys());
        }

        [TestMethod]
        public void PaillierRegenerateKeys_WithCorrectBitLength()
        {
            int bitLength = 3;
            var paillier = new PaillierEncryption(bitLength);
            var serializedKeys = paillier.PrintKeys();

            paillier.SetPrimeBitLength(4);
            paillier.RegenerateKeys();

            Assert.AreEqual(paillier.PrimeBitLength, 4);
            Assert.AreEqual(paillier.MaxPlaintextSize, paillier.KeyPair.PublicKey.N - 2);
            Assert.AreNotEqual(serializedKeys, paillier.PrintKeys());
        }

    }
}
