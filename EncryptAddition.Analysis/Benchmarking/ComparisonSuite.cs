using EncryptAddition.Analysis.ResultTypes;
using System.Numerics;

namespace EncryptAddition.Analysis.Benchmarking
{
    public class ComparisonSuite
    {
        private int _bitLength;
        public int BitLength
        {
            get => _bitLength;
            set
            {
                // Since the minimum for ElGamal is 3.
                if (value < 4)
                    throw new ArgumentOutOfRangeException("primeBitLength", "The bit length for the comparison suite must be at least 4.");

                _bitLength = value;
            }
        }

        private readonly BenchmarkSuite _elGamalSuite;
        private readonly BenchmarkSuite _paillierSuite;

        public ComparisonSuite(int primeBitLength)
        {
            BitLength = primeBitLength;

            _elGamalSuite = new BenchmarkSuite(EncryptionStrategy.ELGAMAL, BitLength);
            _paillierSuite = new BenchmarkSuite(EncryptionStrategy.PAILLIER, BitLength);
        }

        public (BenchmarkResult PaillierResult, BenchmarkResult ElGamalResult) RunBenchmarks(params BigInteger[] values)
        {
            // Run benchmarks for Paillier
            var paillierResults = _paillierSuite.RunBenchmarks(values);

            // Run benchmarks for ElGamal
            var elGamalResults = _elGamalSuite.RunBenchmarks(values);

            return (paillierResults, elGamalResults);
        }
    }
}
