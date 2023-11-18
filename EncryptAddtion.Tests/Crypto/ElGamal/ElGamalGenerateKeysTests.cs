﻿using EncryptAddition.Crypto.ElGamal;

namespace EncryptAddtion.Tests.Crypto.ElGamal
{
    [TestClass]
    public class ElGamalGenerateKeysTests
    {
        [TestMethod]
        public void ElGamalGenerateKeys_WithCorrectBitLength()
        {
            int bitLength = 3;
            var elGamal = new ElGamalEncryption(bitLength);
            var serializedKeys = elGamal.PrintKeys();

            elGamal.SetPrimeBitLength(4);
            elGamal.GenerateKeys();

            Assert.AreEqual(elGamal.PrimeBitLength, 4);
            Assert.AreEqual(elGamal.MaxPlaintextSize, elGamal.KeyPair.PublicKey.Prime - 2);
            Assert.AreNotEqual(serializedKeys, elGamal.PrintKeys());
        }

    }
}
