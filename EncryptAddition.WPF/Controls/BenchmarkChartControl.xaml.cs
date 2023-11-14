using EncryptAddition.Analysis.Benchmarking;
using LiveCharts;
using LiveCharts.Wpf;
using System.Windows;
using System.Windows.Controls;

namespace EncryptAddition.WPF.Controls
{
    public partial class BenchmarkChartControl : UserControl
    {
        public BenchmarkChartControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ChartDataProperty =
        DependencyProperty.Register("ChartData", typeof(BenchmarkResult), typeof(BenchmarkChartControl),
            new PropertyMetadata(default(BenchmarkResult), OnChartDataChanged));

        private static void OnChartDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (BenchmarkChartControl)d;
            control.UpdateChart();
        }

        public BenchmarkResult ChartData
        {
            get { return (BenchmarkResult)GetValue(ChartDataProperty); }
            set { SetValue(ChartDataProperty, value); }
        }

        private void UpdateChart()
        {
            var chartSeries = new SeriesCollection();

            var values = (ChartData.AdditionTime.HasValue) ? new ChartValues<double> { ChartData.KeyGenerationTime, ChartData.EncryptionTime, ChartData.DecryptionTime, ChartData.AdditionTime.Value } : new ChartValues<double> { ChartData.KeyGenerationTime, ChartData.EncryptionTime, ChartData.DecryptionTime };

            var labels = (ChartData.AdditionTime.HasValue) ? new string[] { "Key Generation Time", "Encryption Time", "Decryption Time", "Addition Time" } :
                new string[] { "Key Generation Time", "Encryption Time", "Decryption Time" };

            chartSeries.Add(new ColumnSeries
            {
                Values = values
            });

            this.CartesianChart.Series = chartSeries;
            this.XAxis.Labels = labels;
        }
    }
}