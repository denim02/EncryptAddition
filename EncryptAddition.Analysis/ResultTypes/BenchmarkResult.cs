using EncryptAddition.Crypto;
using System.Numerics;

namespace EncryptAddition.Analysis.ResultTypes
{
    public readonly struct BenchmarkResult
    {
        public string AlgorithmName { get; }
        public int BitLength { get; }
        public BigInteger MaxPlaintextSize { get; }
        public double KeyGenerationTime { get; }
        public double EncryptionTime { get; }
        public double DecryptionTime { get; }
        public BigInteger DecryptionResult { get; }
        public CipherText[] IntermediarySteps { get; }
        public double? AdditionTime { get; } = null;

        public BenchmarkResult(string algorithmName, int bitLength, BigInteger maxPlaintextSize, double keyGenerationTime, double encryptionTime, double decryptionTime, BigInteger decryptionResult, CipherText[] intermediarySteps)
        {
            AlgorithmName = algorithmName;
            BitLength = bitLength;
            MaxPlaintextSize = maxPlaintextSize;
            KeyGenerationTime = keyGenerationTime;
            EncryptionTime = encryptionTime;
            DecryptionTime = decryptionTime;
            DecryptionResult = decryptionResult;
            IntermediarySteps = intermediarySteps;
        }

        public BenchmarkResult(string algorithmName, int bitLength, BigInteger maxPlaintextSize, double keyGenerationTime, double encryptionTime, double decryptionTime, BigInteger decryptionResult, CipherText[] intermediarySteps, double additionTime) : this(algorithmName, bitLength, maxPlaintextSize, keyGenerationTime, encryptionTime, decryptionTime, decryptionResult, intermediarySteps)
        {
            AdditionTime = additionTime;
        }

        public override string ToString()
        {
            if (AdditionTime.HasValue)
                return $"Algo: {AlgorithmName} | Bit: {BitLength} | KeyGenTime: {KeyGenerationTime} | EncTime: {EncryptionTime} | DecTime: {DecryptionTime} | AddTime: {AdditionTime} | DecResult: {DecryptionResult}";
            else
                return $"Algo: {AlgorithmName} | Bit: {BitLength} | KeyGenTime: {KeyGenerationTime} | EncTime: {EncryptionTime} | DecTime: {DecryptionTime} | DecResult: {DecryptionResult}";
        }
    }
}
