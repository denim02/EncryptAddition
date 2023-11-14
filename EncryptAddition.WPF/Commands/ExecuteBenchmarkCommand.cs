using EncryptAddition.WPF.DataTypes;
using EncryptAddition.WPF.ServiceAdapters;
using EncryptAddition.WPF.ViewModels;
using System;
using System.Threading.Tasks;

namespace EncryptAddition.WPF.Commands
{
    public class ExecuteBenchmarkCommand : BaseAsyncCommand
    {
        private readonly BenchmarkTabViewModel _benchmarkTabViewModel;

        public ExecuteBenchmarkCommand(BenchmarkTabViewModel benchmarkTabViewModel)
        {
            _benchmarkTabViewModel = benchmarkTabViewModel;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            _benchmarkTabViewModel.IsPreparingBenchmark = true;

            try
            {
                IAsyncAnalysisAdapter analysisAdapter = (_benchmarkTabViewModel.BenchmarkChoice == BenchmarkChoice.COMPARISON) ? new AsyncComparisonServiceAdapter(_benchmarkTabViewModel.BitLength) : new AsyncSingleBenchmarkServiceAdapter(_benchmarkTabViewModel.BenchmarkChoice, _benchmarkTabViewModel.BitLength);
                await analysisAdapter.PrepareService();

                _benchmarkTabViewModel.IsPreparingBenchmark = false;
                _benchmarkTabViewModel.IsBenchmarking = true;

                var results = await analysisAdapter.RunAnalysis(_benchmarkTabViewModel.InputValues);

                _benchmarkTabViewModel.IsBenchmarking = false;
                _benchmarkTabViewModel.BenchmarkResults = results;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
