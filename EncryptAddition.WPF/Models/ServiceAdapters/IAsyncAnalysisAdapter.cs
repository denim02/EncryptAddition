using EncryptAddition.Analysis.ResultTypes;
using System;
using System.Numerics;
using System.Threading.Tasks;

namespace EncryptAddition.WPF.Models.ServiceAdapters
{
    public interface IAsyncAnalysisAdapter
    {
        public bool IsReady { get; }
        Task PrepareService();
        Task<Tuple<BenchmarkResult, BenchmarkResult?>> RunAnalysis(BigInteger[] inputs);
    }
}
