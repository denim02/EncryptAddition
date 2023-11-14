using EncryptAddition.Analysis.ResultTypes;
using EncryptAddition.WPF.Services;
using System;
using System.Numerics;
using System.Threading.Tasks;

namespace EncryptAddition.WPF.ServiceAdapters
{
    public class AsyncComparisonServiceAdapter : IAsyncAnalysisAdapter
    {
        private ComparisonService _comparisonService;
        private bool _isServiceReady = false;

        public AsyncComparisonServiceAdapter(int bitLength)
        {
            _comparisonService = new ComparisonService(bitLength);
        }

        public Task PrepareService()
        {
            _isServiceReady = true;
            return Task.Run(() => _comparisonService.PrepareService());
        }

        public async Task<Tuple<BenchmarkResult, BenchmarkResult?>> RunAnalysis(BigInteger[] inputs)
        {
            if (!_isServiceReady)
                throw new InvalidOperationException("The service was not prepared properly to handle this operation.");

            var results = await Task.Run(() => _comparisonService.RunAnalysis(inputs));
            return new Tuple<BenchmarkResult, BenchmarkResult?>(results.Item1, results.Item2);
        }
    }
}
