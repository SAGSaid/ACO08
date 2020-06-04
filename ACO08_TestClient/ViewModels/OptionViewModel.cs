using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACO08_Library.Public;

namespace ACO08_TestClient.ViewModels
{
    public class OptionViewModel
    {
        public ACO08_Options Options { get; }

        public OptionViewModel(ACO08_Options options)
        {
            Options = options;
        }
    }
}
