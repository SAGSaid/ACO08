using System.Windows;
using System.Windows.Input;
using ACO08_Library.Public;
using ACO08_TestClient.ViewModels;

namespace ACO08_TestClient
{
    /// <summary>
    /// Interaction logic for OptionWindow.xaml
    /// </summary>
    public partial class OptionWindow : Window
    {
        public OptionWindow(ACO08_Options options)
        {
            DataContext = new OptionViewModel(options);

            InitializeComponent();
        }

        private void CommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }
    }
}
