using EncryptAddition.Crypto.ElGamal;

namespace EncryptAddtion.Tests.Crypto.ElGamal
{
    [TestClass]
    public class ElGamalSettersTests
    {
        [TestMethod]
        public void ElGamal_SetBitLengthWithProperLength()
        {
            var elGamal = new ElGamalEncryption(6);
            Assert.AreEqual(elGamal.PrimeBitLength, 6);

            elGamal.SetPrimeBitLength(5);
            Assert.AreEqual(elGamal.PrimeBitLength, 5);
        }

        [TestMethod]
        public void ElGamal_SetBitLengthWithInvalidLength()
        {
            var elGamal = new ElGamalEncryption(4);
            Assert.AreEqual(elGamal.PrimeBitLength, 4);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => elGamal.SetPrimeBitLength(2));
        }

        // The group of SetKeyPair methods is defined as private,
        // so must call the ElGamalEncryption constructor with a keypair
        // as an argument to invoke it.
        [TestMethod]
        public void ElGamal_SetKeyPairWithValidationWithProperFormat()
        {
            // Correct format
            var keyPair = KeyPair.Deserialize("5|2|4;2");
            var elGamal = new ElGamalEncryption(keyPair);

            Assert.IsNotNull(elGamal.KeyPair);
            Assert.AreEqual(keyPair, elGamal.KeyPair);
        }

        [TestMethod]
        public void ElGamal_SetKeyPairWithValidationWithInvalidFormat_GeneratorTooLarge()
        {
            var keyPair = KeyPair.Deserialize("5|6|4;2");

            try
            {
                var elGamal = new ElGamalEncryption(keyPair);
            }
            catch (ArgumentException e)
            {
                Assert.AreEqual(e.Message, "Invalid public key. The generator is greater than the prime.");
            }
        }

        [TestMethod]
        public void ElGamal_SetKeyPairWithValidationWithInvalidFormat_BetaTooLarge()
        {
            var keyPair = KeyPair.Deserialize("5|2|6;2");

            try
            {
                var elGamal = new ElGamalEncryption(keyPair);
            }
            catch (ArgumentException e)
            {
                Assert.AreEqual(e.Message, "Invalid public key. Beta is greater than the prime.");
            }
        }

        [TestMethod]
        public void ElGamal_SetKeyPairWithValidationWithInvalidFormat_PrivateKeyOutsideRange()
        {
            var keyPair = KeyPair.Deserialize("5|2|4;1");
            try
            {
                var elGamal = new ElGamalEncryption(keyPair);
            }
            catch (ArgumentException e)
            {
                Assert.AreEqual(e.Message, "Invalid private key");
            }

            var keyPair2 = KeyPair.Deserialize("5|2|4;7");
            try
            {
                var elGamal2 = new ElGamalEncryption(keyPair);
            }
            catch (ArgumentException e2)
            {
                Assert.AreEqual(e2.Message, "Invalid private key");
            }
        }

        [TestMethod]
        public void ElGamal_SetKeyPairWithValidationWithInvalidFormat_NonPrime()
        {
            var keyPair = KeyPair.Deserialize("4|2|4;2");
            try
            {
                var elGamal = new ElGamalEncryption(keyPair);
            }
            catch (ArgumentException e)
            {
                Assert.AreEqual(e.Message, "Invalid public key. The order of the group is not prime.");
            }
        }

        [TestMethod]
        public void ElGamal_SetKeyPairWithValidationWithInvalidFormat_NonSafePrime()
        {
            var keyPair = KeyPair.Deserialize("73|20|57;55");
            try
            {
                var elGamal = new ElGamalEncryption(keyPair);
            }
            catch (ArgumentException e)
            {
                Assert.AreEqual(e.Message, "Invalid public key. The prime is not a safe prime.");
            }
        }

        [TestMethod]
        public void ElGamal_SetKeyPairWithValidationWithInvalidFormat_NonGenerator()
        {
            var keyPair = KeyPair.Deserialize("5|4|4;2");
            try
            {
                var elGamal = new ElGamalEncryption(keyPair);
            }
            catch (ArgumentException e)
            {
                Assert.AreEqual(e.Message, "Invalid public key. The provided value is not a generator for the group defined by the prime.");
            }
        }

        [TestMethod]
        public void ElGamal_SetKeyPairWithValidationWithInvalidFormat_InvalidBeta()
        {
            var keyPair = KeyPair.Deserialize("5|2|3;2");
            try
            {
                var elGamal = new ElGamalEncryption(keyPair);
            }
            catch (ArgumentException e)
            {
                Assert.AreEqual(e.Message, "Invalid public key. The provided beta is not correct.");
            }
        }
    }
}