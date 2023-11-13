namespace EncryptAddition.Analysis.Benchmarking
{
    public readonly struct BenchmarkResult
    {
        public string AlgorithmName { get; }
        public int BitLength { get; }
        public double KeyGenerationTime { get; }
        public double EncryptionTime { get; }
        public double DecryptionTime { get; }
        public double? AdditionTime { get; } = null;

        public BenchmarkResult(string algorithmName, int bitLength, double keyGenerationTime, double encryptionTime, double decryptionTime)
        {
            AlgorithmName = algorithmName;
            BitLength = bitLength;
            KeyGenerationTime = keyGenerationTime;
            EncryptionTime = encryptionTime;
            DecryptionTime = decryptionTime;
        }

        public BenchmarkResult(string algorithmName, int bitLength, double keyGenerationTime, double encryptionTime, double decryptionTime, double additionTime) : this(algorithmName, bitLength, keyGenerationTime, encryptionTime, decryptionTime)
        {
            AdditionTime = additionTime;
        }

        public override string ToString()
        {
            if (AdditionTime.HasValue)
                return $"Algo: {AlgorithmName} | Bit: {BitLength} | KeyGenTime: {KeyGenerationTime} | EncTime: {EncryptionTime} | DecTime: {DecryptionTime} | AddTime: {AdditionTime}";
            else
                return $"Algo: {AlgorithmName} | Bit: {BitLength} | KeyGenTime: {KeyGenerationTime} | EncTime: {EncryptionTime} | DecTime: {DecryptionTime}";
        }
    }
}
