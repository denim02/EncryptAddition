using EncryptAddition.Crypto.Paillier;

namespace EncryptAddtion.Tests.Crypto.Paillier
{
    [TestClass]
    public class PaillierSettersTests
    {
        [TestMethod]
        public void Paillier_SetBitLengthWithProperLength()
        {
            var paillier = new PaillierEncryption(6);
            Assert.AreEqual(paillier.PrimeBitLength, 6);

            paillier.SetPrimeBitLength(5);
            Assert.AreEqual(paillier.PrimeBitLength, 5);
        }

        [TestMethod]
        public void Paillier_SetBitLengthWithInvalidLength()
        {
            var paillier = new PaillierEncryption(4);
            Assert.AreEqual(paillier.PrimeBitLength, 4);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => paillier.SetPrimeBitLength(1));
        }

        // The group of SetKeyPair methods is defined as private,
        // so must call the PaillierEncryption constructor with a keypair
        // as an argument to invoke it.
        [TestMethod]
        public void Paillier_SetKeyPairWithValidationWithProperFormat()
        {
            // Correct format
            var keyPair = KeyPair.Deserialize("6|7;2|2");
            var paillier = new PaillierEncryption(keyPair);

            Assert.IsNotNull(paillier.KeyPair);
            Assert.AreEqual(keyPair, paillier.KeyPair);
        }

        [TestMethod]
        public void Paillier_SetKeyPairWithValidationWithInvalidFormat_InvalidG()
        {
            var keyPair = KeyPair.Deserialize("6|8;2|2");

            try
            {
                var paillier = new PaillierEncryption(keyPair);
            }
            catch (ArgumentException e)
            {
                Assert.AreEqual(e.Message, "Invalid public key. G is not equal to n + 1.");
            }
        }

        [TestMethod]
        public void Paillier_SetKeyPairWithValidationWithInvalidFormat_InvalidMuPrivateKey()
        {
            var keyPair = KeyPair.Deserialize("6|7;2|3");

            try
            {
                var paillier = new PaillierEncryption(keyPair);
            }
            catch (ArgumentException e)
            {
                Assert.AreEqual(e.Message, "Invalid private key.");
            }
        }
    }
}