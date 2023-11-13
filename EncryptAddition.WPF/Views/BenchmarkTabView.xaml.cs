using EncryptAddition.WPF.ViewModels;
using System.Windows.Controls;

namespace EncryptAddition.WPF.Views
{
    /// <summary>
    /// Interaction logic for BenchmarkTabView.xaml
    /// </summary>
    public partial class BenchmarkTabView : UserControl
    {
        public BenchmarkTabView()
        {
            InitializeComponent();
            DataContext = new BenchmarkTabViewModel();
        }
    }
}
