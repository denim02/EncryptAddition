using EncryptAddition.Analysis.ResultTypes;
using System;
using System.Numerics;
using System.Threading.Tasks;

namespace EncryptAddition.WPF.ServiceAdapters
{
    public interface IAsyncAnalysisAdapter
    {
        Task PrepareService();
        Task<Tuple<BenchmarkResult, BenchmarkResult?>> RunAnalysis(BigInteger[] inputs);
    }
}
