using EncryptAddition.WPF.Services;
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
            var _benchmarkService = new BenchmarkService(_benchmarkTabViewModel.StrategyChoice, _benchmarkTabViewModel.BitLength);

            try
            {
                await Task.Run(() => _benchmarkService.PrepareBenchmark());
                _benchmarkTabViewModel.IsPreparingBenchmark = false;
                _benchmarkTabViewModel.IsBenchmarking = true;
                var results = await Task.Run(() => _benchmarkService.RunBenchmarks(_benchmarkTabViewModel.InputValues));
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
