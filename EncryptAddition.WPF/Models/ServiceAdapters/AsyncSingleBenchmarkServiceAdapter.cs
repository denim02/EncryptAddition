using EncryptAddition.Analysis.ResultTypes;
using EncryptAddition.WPF.DataTypes;
using EncryptAddition.WPF.Models.Services;
using System;
using System.Numerics;
using System.Threading.Tasks;

namespace EncryptAddition.WPF.Models.ServiceAdapters
{
    public class AsyncSingleBenchmarkServiceAdapter : IAsyncAnalysisAdapter
    {
        private SingleBenchmarkService _singleBenchmarkService;

        public bool IsReady { get; private set; } = false;

        public AsyncSingleBenchmarkServiceAdapter(BenchmarkChoice choice, int bitLength)
        {
            _singleBenchmarkService = new SingleBenchmarkService(choice, bitLength);
        }

        public Task PrepareService()
        {
            IsReady = true;
            return Task.Run(() => _singleBenchmarkService.PrepareService());
        }

        public async Task<Tuple<BenchmarkResult, BenchmarkResult?>> RunAnalysis(BigInteger[] inputs)
        {
            if (!IsReady)
                throw new InvalidOperationException("The service was not prepared properly to handle this operation.");

            var result = await Task.Run(() => _singleBenchmarkService.RunAnalysis(inputs));
            return new Tuple<BenchmarkResult, BenchmarkResult?>(result, null);
        }
    }
}
