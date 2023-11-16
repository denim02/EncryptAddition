using EncryptAddition.Analysis.ResultTypes;
using EncryptAddition.WPF.Commands;
using EncryptAddition.WPF.DataTypes;
using System;
using System.Numerics;
using System.Windows.Input;

namespace EncryptAddition.WPF.ViewModels
{
    public class BenchmarkTabViewModel : BaseViewModel
    {
        public string[] StrategyList { get; } = { "ElGamal", "Paillier", "Comparison" };

        private BenchmarkChoice _benchmarkChoice = BenchmarkChoice.PAILLIER;
        public BenchmarkChoice BenchmarkChoice
        {
            get => _benchmarkChoice;
            set
            {
                _benchmarkChoice = value;
                OnPropertyChanged(nameof(BenchmarkChoice));
            }
        }

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

        // Can hold two in case of comparison
        private Tuple<BenchmarkResult, BenchmarkResult?>? _benchmarkResults = null;
        public Tuple<BenchmarkResult, BenchmarkResult?>? BenchmarkResults
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
                OnPropertyChanged(nameof(IsBusy));
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
                OnPropertyChanged(nameof(IsBusy));
            }
        }

        public bool IsBusy => IsBenchmarking || IsPreparingBenchmark;

        private bool _hasValidationErrors;
        public bool HasValidationErrors
        {
            get => _hasValidationErrors;
            set
            {
                if (_hasValidationErrors != value)
                {
                    _hasValidationErrors = value;
                    OnPropertyChanged(nameof(HasValidationErrors));
                }
            }
        }

        public ICommand ExecuteBenchmark { get; }

        public BenchmarkTabViewModel()
        {
            ExecuteBenchmark = new ExecuteBenchmarkCommand(this);
        }
    }
}
