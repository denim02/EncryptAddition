using EncryptAddition.Analysis.Benchmarking;
using LiveCharts;
using LiveCharts.Wpf;
using System.Windows;
using System.Windows.Controls;

namespace EncryptAddition.WPF.Controls
{
    public partial class BenchmarkChart : UserControl
    {
        private SeriesCollection ChartSeries { get; set; }

        public BenchmarkChart()
        {
            InitializeComponent();
            DataContext = this;
        }

        public static readonly DependencyProperty ChartDataProperty =
        DependencyProperty.Register("ChartData", typeof(BenchmarkResult), typeof(BenchmarkChart),
            new PropertyMetadata(default(BenchmarkResult), OnChartDataChanged));

        private static void OnChartDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (BenchmarkChart)d;
            control.UpdateChart();
        }

        public BenchmarkResult BenchmarkData
        {
            get { return (BenchmarkResult)GetValue(ChartDataProperty); }
            set { SetValue(ChartDataProperty, value); }
        }

        private void UpdateChart()
        {
            // Clear existing series
            ChartSeries.Clear();

            ChartSeries.Add(new ColumnSeries
            {
                Title = "Key Generation Time",
                Values = new ChartValues<double> { BenchmarkData.KeyGenerationTime }
            });

            ChartSeries.Add(new ColumnSeries
            {
                Title = "Encryption Time",
                Values = new ChartValues<double> { BenchmarkData.EncryptionTime }
            });

            ChartSeries.Add(new ColumnSeries
            {
                Title = "Decryption Time",
                Values = new ChartValues<double> { BenchmarkData.DecryptionTime }
            });

            if (BenchmarkData.AdditionTime.HasValue)
                ChartSeries.Add(new ColumnSeries
                {
                    Title = "Addition Time",
                    Values = new ChartValues<double> { BenchmarkData.AdditionTime.Value }
                });
        }
    }
}