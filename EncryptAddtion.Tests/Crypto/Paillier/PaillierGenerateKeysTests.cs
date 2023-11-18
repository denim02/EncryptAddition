using EncryptAddition.Crypto.Paillier;

namespace EncryptAddtion.Tests.Crypto.Paillier
{
    [TestClass]
    public class PaillierGenerateKeysTests
    {
        [TestMethod]
        public void PaillierGenerateKeys_WithoutPrimeBitLength()
        {
            var keyPair = KeyPair.Deserialize("6|7;2|2");
            var paillier = new PaillierEncryption(keyPair);

            Assert.IsNull(paillier.PrimeBitLength);
            Assert.ThrowsException<InvalidOperationException>(() => paillier.GenerateKeys());
        }

        [TestMethod]
        public void PaillierGenerateKeys_WithCorrectBitLength()
        {
            int bitLength = 3;
            var paillier = new PaillierEncryption(bitLength);
            var serializedKeys = paillier.PrintKeys();

            paillier.SetPrimeBitLength(4);
            paillier.GenerateKeys();

            Assert.AreEqual(paillier.PrimeBitLength, 4);
            Assert.AreEqual(paillier.MaxPlaintextSize, paillier.KeyPair.PublicKey.N - 2);
            Assert.AreNotEqual(serializedKeys, paillier.PrintKeys());
        }

    }
}
