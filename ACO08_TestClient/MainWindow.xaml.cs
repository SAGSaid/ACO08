using System.Windows;
using ACO08_TestClient.ViewModels;

namespace ACO08_TestClient
{
    /// <summary>
    /// Window for the TestClient
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var viewModel = new DiscoveryViewModel(MainContainer);
            DataContext = viewModel;
        }
    }
}
