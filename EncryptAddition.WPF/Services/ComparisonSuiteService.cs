using EncryptAddition.Analysis.Benchmarking;

namespace EncryptAddition.WPF.Services
{
    public class ComparisonSuiteService
    {
        private ComparisonSuite _comparisonSuite;

        public ComparisonSuiteService(int bitLength)
        {
            _comparisonSuite = new ComparisonSuite(bitLength);
        }
    }
}
