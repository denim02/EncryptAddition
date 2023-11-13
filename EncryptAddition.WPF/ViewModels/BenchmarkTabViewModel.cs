using EncryptAddition.Analysis.Benchmarking;
using EncryptAddition.Crypto;
using EncryptAddition.WPF.Commands;
using EncryptAddition.WPF.DataTypes;
using System.Numerics;
using System.Windows.Input;

namespace EncryptAddition.WPF.ViewModels
{
    public class BenchmarkTabViewModel : BaseViewModel
    {
        public string[] StrategyList { get; } = new string[] { "ElGamal", "Paillier", "Both" };

        private int _bitLength;
        public int BitLength
        {
            get => _bitLength;
            set
            {
                _bitLength = value;
                OnPropertyChanged(nameof(BitLength));
            }
        }

        private BenchmarkChoice _benchmarkChoice = BenchmarkChoice.PAILLIER;
        public BenchmarkChoice StrategyChoice
        {
            get => _benchmarkChoice;
            set
            {
                _benchmarkChoice = value;
                OnPropertyChanged(nameof(StrategyChoice));
            }
        }

        private BigInteger[] _inputValues;
        public BigInteger[] InputValues
        {
            get => _inputValues;
            set
            {
                _inputValues = value;
                OnPropertyChanged(nameof(InputValues));
            }
        }

        private (BenchmarkResult Result, CipherText[] IntermediarySteps)? _benchmarkResults = null;
        public (BenchmarkResult Result, CipherText[] IntermediarySteps)? BenchmarkResults
        {
            get => _benchmarkResults;
            set
            {
                _benchmarkResults = value;
                OnPropertyChanged(nameof(BenchmarkResults));
            }
        }

        private bool _isPreparingBenchmark = false;
        public bool IsPreparingBenchmark
        {
            get => _isPreparingBenchmark;
            set
            {
                _isPreparingBenchmark = value;
                OnPropertyChanged(nameof(IsPreparingBenchmark));
            }
        }

        private bool _isBenchmarking = false;
        public bool IsBenchmarking
        {
            get => _isBenchmarking;
            set
            {
                _isBenchmarking = value;
                OnPropertyChanged(nameof(IsBenchmarking));
            }
        }

        public ICommand ExecuteBenchmark { get; }

        public BenchmarkTabViewModel()
        {
            ExecuteBenchmark = new ExecuteBenchmarkCommand(this);
        }
    }
}
