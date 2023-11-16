using EncryptAddition.Analysis.Benchmarking;
using EncryptAddition.Analysis.ResultTypes;
using EncryptAddition.Crypto;
using EncryptAddition.WPF.DataTypes;
using System;
using System.Numerics;

namespace EncryptAddition.WPF.Models.Services
{
    public class SingleBenchmarkService : IAnalysisService<BenchmarkResult>
    {
        private BenchmarkSuite? _benchmarkSuite;

        private readonly EncryptionChoice _encryptionStrategy;
        private readonly int _bitLength;

        public SingleBenchmarkService(BenchmarkChoice choice, int bitLength)
        {
            _encryptionStrategy = BenchmarkChoiceToEncryptionStrategyConvert(choice);
            _bitLength = bitLength;
        }

        public void PrepareService()
        {
            _benchmarkSuite = new BenchmarkSuite(_encryptionStrategy, _bitLength);
        }

        public BenchmarkResult RunAnalysis(BigInteger[] inputs)
        {
            if (_benchmarkSuite == null)
                throw new InvalidOperationException("BenchmarkSuite not initialized.");

            return _benchmarkSuite.RunBenchmarks(inputs);
        }

        private static EncryptionChoice BenchmarkChoiceToEncryptionStrategyConvert(BenchmarkChoice choice)
        {
            switch (choice)
            {
                case BenchmarkChoice.PAILLIER:
                    return EncryptionChoice.PAILLIER;
                case BenchmarkChoice.ELGAMAL:
                    return EncryptionChoice.ELGAMAL;
                default:
                    throw new ArgumentException("Invalid choice.");
            }
        }
    }
}
