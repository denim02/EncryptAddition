using System.Numerics;

namespace EncryptAddition.WPF.Services
{
    public interface IAnalysisService<T>
    {
        void PrepareService();
        T RunAnalysis(BigInteger[] inputs);
    }
}
