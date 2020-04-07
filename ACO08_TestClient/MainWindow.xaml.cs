using System.Windows;

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

            var viewModel = new MainWindowViewModel(MainContainer);
            DataContext = viewModel;
        }
    }
}
