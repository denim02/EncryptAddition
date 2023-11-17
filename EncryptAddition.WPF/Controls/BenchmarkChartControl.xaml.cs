using EncryptAddition.Analysis.ResultTypes;
using LiveCharts;
using LiveCharts.Wpf;
using System;
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
        DependencyProperty.Register("ChartData", typeof(Tuple<BenchmarkResult, BenchmarkResult?>), typeof(BenchmarkChartControl),
            new PropertyMetadata(null, OnChartDataChanged));

        private static void OnChartDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (BenchmarkChartControl)d;
            control.UpdateChart();
        }

        public Tuple<BenchmarkResult, BenchmarkResult?> ChartData
        {
            get { return (Tuple<BenchmarkResult, BenchmarkResult?>)GetValue(ChartDataProperty); }
            set { SetValue(ChartDataProperty, value); }
        }

        private void UpdateChart()
        {
            var chartSeries = new SeriesCollection();
            (BenchmarkResult firstResult, BenchmarkResult? secondResult) = ChartData;

            var firstBenchmarks = (firstResult.AdditionTime.HasValue) ?
                new ChartValues<double> {
                    firstResult.KeyGenerationTime,
                    firstResult.EncryptionTime,
                    firstResult.DecryptionTime,
                    firstResult.AdditionTime.Value
                } :
                new ChartValues<double> {
                    firstResult.KeyGenerationTime,
                    firstResult.EncryptionTime,
                    firstResult.DecryptionTime
                };

            var title = firstResult.AlgorithmName == "Paillier" ? "Paillier Benchmarks" : "ElGamal Benchmarks";

            chartSeries.Add(new ColumnSeries
            {
                Title = title,
                Values = firstBenchmarks
            });

            if (secondResult.HasValue)
            {
                var secondBenchmarks = (secondResult.Value.AdditionTime.HasValue) ?
                    new ChartValues<double> {
                        secondResult.Value.KeyGenerationTime,
                        secondResult.Value.EncryptionTime,
                        secondResult.Value.DecryptionTime,
                        secondResult.Value.AdditionTime.Value
                    } :
                    new ChartValues<double> {
                        secondResult.Value.KeyGenerationTime,
                        secondResult.Value.EncryptionTime,
                        secondResult.Value.DecryptionTime
                    };

                // If there's two, the second is always ElGamal as per function definitions in service classes
                chartSeries.Add(new ColumnSeries
                {
                    Title = "ElGamal Benchmarks",
                    Values = secondBenchmarks
                });
            }

            var labels = (firstResult.AdditionTime.HasValue) ?
                new string[] { "Key Generation Time", "Encryption Time", "Decryption Time", "Addition Time" } :
                new string[] { "Key Generation Time", "Encryption Time", "Decryption Time" };

            Func<double, string> labelFormatter = value => value.ToString("G5");

            this.CartesianChart.AxisX[0].LabelFormatter = labelFormatter;
            this.CartesianChart.Series = chartSeries;
            this.XAxis.Labels = labels;
        }
    }
}