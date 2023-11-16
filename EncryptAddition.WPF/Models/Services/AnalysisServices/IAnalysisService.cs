using System.Numerics;

namespace EncryptAddition.WPF.Models.Services
{
    public interface IAnalysisService<T>
    {
        void PrepareService();
        T RunAnalysis(BigInteger[] inputs);
    }
}
