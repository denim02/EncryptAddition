using EncryptAddition.Crypto.Exceptions;
using EncryptAddition.WPF.Models.ServiceAdapters;
using EncryptAddition.WPF.Models.Stores;
using EncryptAddition.WPF.ViewModels;
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
            var inputs = Utils.ParseStringToBigIntegerArray(_benchmarkTabViewModel.InputValues);

            try
            {
                var analysisServiceStore = AnalysisServiceStore.GetInstance(_benchmarkTabViewModel.BenchmarkChoice, int.Parse(_benchmarkTabViewModel.BenchmarkBitLength));
                IAsyncAnalysisAdapter analysisAdapter = analysisServiceStore.AsyncAnalysisAdapter;

                if (!analysisAdapter.IsReady)
                    await analysisAdapter.PrepareService();

                _benchmarkTabViewModel.IsPreparingBenchmark = false;
                _benchmarkTabViewModel.IsBenchmarking = true;

                var results = await analysisAdapter.RunAnalysis(inputs);

                _benchmarkTabViewModel.BenchmarkResults = results;
            }
            catch (EncryptionOverflowException ex)
            {
                Utils.HandleBenchmarkException(ex);

                var errorToolTip = (inputs.Length > 1) ? $"Invalid input values. Each input and their total sum must be less than {ex.MaxPlaintextSize}." : $"Invalid input value. Input must be less than {ex.MaxPlaintextSize}.";

                _benchmarkTabViewModel.InvalidateProperty(errorToolTip, nameof(_benchmarkTabViewModel.InputValues));
            }
            catch (InvalidDecryptionException ex)
            {
                AnalysisServiceStore.ClearInstance();
                Utils.HandleBenchmarkException(ex);
            }
            finally
            {
                _benchmarkTabViewModel.IsPreparingBenchmark = false;
                _benchmarkTabViewModel.IsBenchmarking = false;
            }
        }
    }
}
