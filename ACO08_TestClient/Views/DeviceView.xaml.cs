using System.Windows.Controls;
using ACO08_Library.Public;
using ACO08_TestClient.ViewModels;

namespace ACO08_TestClient.Views
{
    /// <summary>
    /// View for a single device
    /// </summary>
    public partial class DeviceView : UserControl
    {
        public DeviceView(ACO08_Device device)
        {
            DataContext = new DeviceViewModel(device);

            InitializeComponent();
        }
    }
}
