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
            InitializeComponent();

            var viewModel = new MainWindowViewModel(MainContainer);
            DataContext = viewModel;
        }
    }
}
