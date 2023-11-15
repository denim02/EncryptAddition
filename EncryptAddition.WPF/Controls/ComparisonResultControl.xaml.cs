using EncryptAddition.Analysis.ResultTypes;
using System;
using System.Windows;
using System.Windows.Controls;

namespace EncryptAddition.WPF.Controls
{
    /// <summary>
    /// Interaction logic for ComparisonResultControl.xaml
    /// </summary>
    public partial class ComparisonResultControl : UserControl
    {
        public ComparisonResultControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty BenchmarkResultsProperty =
      DependencyProperty.Register("BenchmarkResults", typeof(Tuple<BenchmarkResult, BenchmarkResult?>), typeof(ComparisonResultControl),
          new PropertyMetadata(null, OnBenchmarkResultsChanged));

        private static void OnBenchmarkResultsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (ComparisonResultControl)d;
        }

        public Tuple<BenchmarkResult, BenchmarkResult?> BenchmarkResults
        {
            get { return (Tuple<BenchmarkResult, BenchmarkResult?>)GetValue(BenchmarkResultsProperty); }
            set { SetValue(BenchmarkResultsProperty, value); }
        }
    }
}
