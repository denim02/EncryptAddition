using EncryptAddition.Analysis.Benchmarking;
using EncryptAddition.Analysis.ResultTypes;
using System;
using System.Numerics;

namespace EncryptAddition.WPF.Models.Services
{
    public class ComparisonService : IAnalysisService<Tuple<BenchmarkResult, BenchmarkResult>>
    {
        private ComparisonSuite? _comparisonSuite;

        private readonly int _bitLength;

        public ComparisonService(int bitLength)
        {
            _bitLength = bitLength;
        }

        public void PrepareService()
        {
            _comparisonSuite = new ComparisonSuite(_bitLength);
        }

        public Tuple<BenchmarkResult, BenchmarkResult> RunAnalysis(BigInteger[] inputs)
        {
            if (_comparisonSuite == null)
                throw new InvalidOperationException("ComparisonSuite is not initialized.");

            return _comparisonSuite.RunBenchmarks(inputs).ToTuple();
        }
    }
}
