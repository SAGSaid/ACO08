using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACO08_TestClient
{
    public class MainWindowViewModel
    {
        public MainWindowModel Model { get; }

        public MainWindowViewModel(MainWindowModel model)
        {
            Model = model;
        }
    }
}
