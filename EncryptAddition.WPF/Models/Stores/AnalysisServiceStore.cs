using EncryptAddition.WPF.DataTypes;
using EncryptAddition.WPF.Models.ServiceAdapters;

namespace EncryptAddition.WPF.Models.Stores
{
    public sealed class AnalysisServiceStore
    {
        private static AnalysisServiceStore _instance;

        private static int _bitLength;
        private static BenchmarkChoice _choice;
        public IAsyncAnalysisAdapter AsyncAnalysisAdapter { get; private set; }

        private AnalysisServiceStore(BenchmarkChoice choice, int primeBitLength)
        {
            _bitLength = primeBitLength;
            _choice = choice;
            AsyncAnalysisAdapter = (choice == BenchmarkChoice.Comparison) ? new AsyncComparisonServiceAdapter(primeBitLength) : new AsyncSingleBenchmarkServiceAdapter(choice, primeBitLength);
        }

        public static AnalysisServiceStore GetInstance(BenchmarkChoice choice, int primeBitLength)
        {
            if (_instance == null || choice != _choice || _bitLength != primeBitLength)
            {
                _instance = new AnalysisServiceStore(choice, primeBitLength);
                return _instance;
            }

            return _instance;
        }

        public static void ClearInstance()
        {
            _instance = null;
        }
    }
}
