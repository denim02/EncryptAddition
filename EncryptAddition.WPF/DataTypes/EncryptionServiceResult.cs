using EncryptAddition.Crypto;
using System;
using System.Numerics;

namespace EncryptAddition.WPF.DataTypes
{
    public struct EncryptionServiceResult
    {
        public string AlgorithmName { get; set; }
        public string Operation { get; set; }
        public OperationChoice OperationChoice { get; set; }
        public string SerializedKeyPair { get; set; }
        public string MaxPlaintextSize { get; set; }

        // <input, output>
        public Tuple<object, object>[] Results { get; set; }

        public EncryptionServiceResult(EncryptionChoice choice, OperationChoice operation, string serializedKeyPair, BigInteger maxPlaintextSize, Tuple<object, object>[] results)
        {
            AlgorithmName = choice == EncryptionChoice.ElGamal ? "ElGamal" : "Paillier";
            Operation = operation == OperationChoice.ENCRYPTION ? "Encryption" : "Decryption";
            OperationChoice = operation;
            SerializedKeyPair = serializedKeyPair;
            MaxPlaintextSize = maxPlaintextSize.ToString();
            Results = results;
        }
    }
}
