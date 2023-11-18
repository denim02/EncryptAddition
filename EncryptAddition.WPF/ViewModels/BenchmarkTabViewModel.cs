using EncryptAddition.Analysis.ResultTypes;
using EncryptAddition.WPF.Commands;
using EncryptAddition.WPF.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace EncryptAddition.WPF.ViewModels
{
    public class BenchmarkTabViewModel : BaseViewModel
    {
        #region Data Members
        // Property used to automatically populate combobox
        public BenchmarkChoice[] StrategyList { get; } = { BenchmarkChoice.Paillier, BenchmarkChoice.ElGamal, BenchmarkChoice.Comparison };

        #region Input Fields
        // BenchmarkChoice combobox field
        private BenchmarkChoice _benchmarkChoice = BenchmarkChoice.Paillier;
        public BenchmarkChoice BenchmarkChoice
        {
            get => _benchmarkChoice;
            set
            {
                _benchmarkChoice = value;
                OnPropertyChanged(nameof(BenchmarkChoice));
            }
        }

        // BitLength textbox field
        private string _benchmarkBitLength;
        private bool _isBenchmarkBitLengthValid = false;
        public string BenchmarkBitLength
        {
            get => _benchmarkBitLength;
            set
            {
                _isBenchmarkBitLengthValid = IsPropertyValid(value, IsBitLengthValid);

                _benchmarkBitLength = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsDataValid));
            }
        }
        // BitLength validation logic
        private (bool IsValid, IEnumerable<string> ErrorMessages) IsBitLengthValid(string value)
        {
            if (String.IsNullOrWhiteSpace(value))
                return (false, new[] { "The bit length field is required." });

            // Try to parse the bit length to an int
            if (int.TryParse(value, out int bitLength))
            {
                if (bitLength < 3)
                    return (false, new[] { "The bit length value must be 3 or greater." });
                else
                    return (true, Enumerable.Empty<string>());
            }
            return (false, new[] { "The bit length value must be a positive integer." });
        }

        // InputValues textbox field
        private string _inputValues;
        private bool _isInputValuesValid = false;
        public string InputValues
        {
            get => _inputValues;
            set
            {
                _isInputValuesValid = IsPropertyValid(value, IsInputValuesValid);

                _inputValues = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsDataValid));
            }
        }
        // InputValues validation logic
        private Regex _inputValueFormat = new Regex("^\\s*\\d+(\\s*,\\s*\\d+\\s*)*$");
        private (bool IsValid, IEnumerable<string> ErrorMessages) IsInputValuesValid(string value)
        {
            // Check if the field is empty
            if (String.IsNullOrWhiteSpace(value))
                return (false, new[] { "The input value field is required." });

            if (_inputValueFormat.IsMatch(value))
                return (true, Enumerable.Empty<string>());
            else
                return (false, new[] { "Invalid format. The input values must be entered as a comma-separated list of positive integers." });
        }
        #endregion


        #region Output Fields
        public bool IsDataValid => _isInputValuesValid && _isBenchmarkBitLengthValid;

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
        #endregion

        #region Command Execution fields
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

        public ICommand ExecuteBenchmark { get; }
        #endregion
        #endregion

        // Constructor
        public BenchmarkTabViewModel()
        {
            ExecuteBenchmark = new ExecuteBenchmarkCommand(this);
        }
    }
}
