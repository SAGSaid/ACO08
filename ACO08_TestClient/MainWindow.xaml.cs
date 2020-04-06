using System.Windows;
using ACO08_Library.Public;

namespace ACO08_TestClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            var model = new TestClientInterface();
            var viewModel = new MainWindowViewModel(model, MainContainer);

            DataContext = viewModel;
            InitializeComponent();
        }
    }
}
