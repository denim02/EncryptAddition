using EncryptAddition.Analysis.ResultTypes;
using EncryptAddition.WPF.DataTypes;
using EncryptAddition.WPF.Services;
using System;
using System.Numerics;
using System.Threading.Tasks;

namespace EncryptAddition.WPF.ServiceAdapters
{
    public class AsyncSingleBenchmarkServiceAdapter : IAsyncAnalysisAdapter
    {
        private SingleBenchmarkService _singleBenchmarkService;
        private bool _isServiceReady = false;

        public AsyncSingleBenchmarkServiceAdapter(BenchmarkChoice choice, int bitLength)
        {
            _singleBenchmarkService = new SingleBenchmarkService(choice, bitLength);
        }

        public Task PrepareService()
        {
            _isServiceReady = true;
            return Task.Run(() => _singleBenchmarkService.PrepareService());
        }

        public async Task<Tuple<BenchmarkResult, BenchmarkResult?>> RunAnalysis(BigInteger[] inputs)
        {
            if (!_isServiceReady)
                throw new InvalidOperationException("The service was not prepared properly to handle this operation.");

            var result = await Task.Run(() => _singleBenchmarkService.RunAnalysis(inputs));
            return new Tuple<BenchmarkResult, BenchmarkResult?>(result, null);
        }
    }
}
