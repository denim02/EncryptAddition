using EncryptAddition.Analysis.Benchmarking;
using EncryptAddition.Crypto;
using EncryptAddition.WPF.DataTypes;
using System;
using System.Numerics;

namespace EncryptAddition.WPF.Services
{
    public class BenchmarkService
    {
        private BenchmarkSuite _benchmarkSuite;
        private EncryptionStrategy _encryptionStrategy;
        private int _bitLength;

        public BenchmarkService(BenchmarkChoice choice, int bitLength)
        {
            _encryptionStrategy = BenchmarkChoiceToEncryptionStrategyConvert(choice);
            _bitLength = bitLength;
        }

        public void PrepareBenchmark()
        {
            _benchmarkSuite = new BenchmarkSuite(_encryptionStrategy, _bitLength);
        }

        public (BenchmarkResult Result, CipherText[] IntermediarySteps) RunBenchmarks(params BigInteger[] inputs)
        {
            return _benchmarkSuite.RunBenchmarksWithSteps(inputs);
        }

        private EncryptionStrategy BenchmarkChoiceToEncryptionStrategyConvert(BenchmarkChoice choice)
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
