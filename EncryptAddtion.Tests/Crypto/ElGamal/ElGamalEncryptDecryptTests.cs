using EncryptAddition.Crypto.ElGamal;
using EncryptAddition.Crypto.Exceptions;
using EncryptAddition.Crypto.Utils;

namespace EncryptAddtion.Tests.Crypto.ElGamal
{
    [TestClass]
    public class ElGamalEncryptDecryptTests
    {
        #region Trivial Cases
        // Values will simply be encrypted and then decrypted to see if the same value is found
        [TestMethod]
        public void ElGamalEncryptDecrypt_Zero()
        {
            var elGamal = new ElGamalEncryption(4);
            BigInteger input = 0;
            var cipher = elGamal.Encrypt(input);
            var output = elGamal.Decrypt(cipher);
            Assert.AreEqual(input, output);
        }


        [TestMethod]
        public void ElGamalEncryptDecrypt_One()
        {
            var elGamal = new ElGamalEncryption(4);
            BigInteger input = 1;
            var cipher = elGamal.Encrypt(input);
            var output = elGamal.Decrypt(cipher);
            Assert.AreEqual(input, output);
        }

        [TestMethod]
        public void ElGamalEncryptDecrypt_Three()
        {
            var elGamal = new ElGamalEncryption(4);
            BigInteger input = 3;
            var cipher = elGamal.Encrypt(input);
            var output = elGamal.Decrypt(cipher);
            Assert.AreEqual(input, output);
        }

        [TestMethod]
        public void ElGamalEncryptDecrypt_Encrypt_ArgumentOutOfRangeLarger()
        {
            var elGamal = new ElGamalEncryption(3);
            BigInteger input = 8;
            Assert.ThrowsException<EncryptionOverflowException>(() => elGamal.Encrypt(input));
        }

        [TestMethod]
        public void ElGamalEncryptDecrypt_Encrypt_ArgumentOutOfRangeNegative()
        {
            var elGamal = new ElGamalEncryption(3);
            BigInteger input = -1;
            Assert.ThrowsException<EncryptionOverflowException>(() => elGamal.Encrypt(input));
        }

        [TestMethod]
        public void ElGamalEncryptDecrypt_Encrypt_ArgumentEqualToPrime()
        {
            var elGamal = new ElGamalEncryption(5);
            BigInteger input = elGamal.KeyPair.PublicKey.Prime;
            Assert.ThrowsException<EncryptionOverflowException>(() => elGamal.Encrypt(input));
        }

        [TestMethod]
        public void ElGamalEncryptDecrypt_Encrypt_ArgumentTwoLessThanPrime()
        {
            var elGamal = new ElGamalEncryption(5);
            BigInteger input = elGamal.KeyPair.PublicKey.Prime - 2;
            Assert.AreEqual(input, elGamal.MaxPlaintextSize);
            Assert.AreEqual(input, elGamal.Decrypt(elGamal.Encrypt(input)));
        }

        [TestMethod]
        public void ElGamalEncryptDecrypt_Decrypt_InvalidCipherUsed()
        {
            var elGamal = new ElGamalEncryption(5);
            // Define an invalid cipher for ElGamal (one with no shared secret)
            var cipher = new CipherText(100);

            Assert.ThrowsException<InvalidDecryptionException>(() => elGamal.Decrypt(cipher));
        }

        [TestMethod]
        public void ElGamalEncryptDecrypt_Decrypt_InternalMathException()
        {
            KeyPair keys = new("23|5|8;6");
            var elGamal = new ElGamalEncryption(keys);
            // Define an invalid cipher for ElGamal (one with no shared secret)
            var cipher = new CipherText(3, 46);

            Assert.ThrowsException<InvalidDecryptionException>(() => elGamal.Decrypt(cipher));
        }
        #endregion

        #region Complex Cases
        [TestMethod]
        public void ElGamalEncryptDecrypt_RandomValues()
        {
            var elGamal = new ElGamalEncryption(5);

            for (int i = 0; i < 5; i++)
            {
                var input = Helpers.GetBigInteger(0, elGamal.MaxPlaintextSize);
                Assert.AreEqual(input, elGamal.Decrypt(elGamal.Encrypt(input)));
            }
        }

        #endregion
    }
}
