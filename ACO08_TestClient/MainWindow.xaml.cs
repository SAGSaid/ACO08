using System.Windows;

namespace ACO08_TestClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            var model = new MainWindowModel();
            var viewModel = new MainWindowViewModel(model);
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
