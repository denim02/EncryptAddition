using EncryptAddition.Analysis.ResultTypes;
using EncryptAddition.WPF.Models.Services;
using System;
using System.Numerics;
using System.Threading.Tasks;

namespace EncryptAddition.WPF.Models.ServiceAdapters
{
    public class AsyncComparisonServiceAdapter : IAsyncAnalysisAdapter
    {
        private ComparisonService _comparisonService;
        public bool IsReady { get; private set; } = false;

        public AsyncComparisonServiceAdapter(int bitLength)
        {
            _comparisonService = new ComparisonService(bitLength);
        }

        public Task PrepareService()
        {
            IsReady = true;
            return Task.Run(() => _comparisonService.PrepareService());
        }

        public async Task<Tuple<BenchmarkResult, BenchmarkResult?>> RunAnalysis(BigInteger[] inputs)
        {
            if (!IsReady)
                throw new InvalidOperationException("The service was not prepared properly to handle this operation.");

            var results = await Task.Run(() => _comparisonService.RunAnalysis(inputs));
            return new Tuple<BenchmarkResult, BenchmarkResult?>(results.Item1, results.Item2);
        }
    }
}
