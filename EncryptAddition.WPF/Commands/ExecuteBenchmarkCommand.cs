using EncryptAddition.WPF.Models.ServiceAdapters;
using EncryptAddition.WPF.Models.Stores;
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
                var analysisServiceStore = AnalysisServiceStore.GetInstance(_benchmarkTabViewModel.BenchmarkChoice, _benchmarkTabViewModel.BitLength);
                IAsyncAnalysisAdapter analysisAdapter = AnalysisServiceStore.AsyncAnalysisAdapter;

                if (!analysisAdapter.IsReady)
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
