using EncryptAddition.Analysis.Benchmarking;
using EncryptAddition.Analysis.ResultTypes;
using EncryptAddition.WPF.DataTypes;
using System;
using System.Numerics;

namespace EncryptAddition.WPF.Services
{
    public class SingleBenchmarkService : IAnalysisService<BenchmarkResult>
    {
        private BenchmarkSuite? _benchmarkSuite;

        private readonly EncryptionStrategy _encryptionStrategy;
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

        private static EncryptionStrategy BenchmarkChoiceToEncryptionStrategyConvert(BenchmarkChoice choice)
        {
            switch (choice)
            {
                case BenchmarkChoice.PAILLIER:
                    return EncryptionStrategy.PAILLIER;
                case BenchmarkChoice.ELGAMAL:
                    return EncryptionStrategy.ELGAMAL;
                default:
                    throw new ArgumentException("Invalid choice.");
            }
        }
    }
}
