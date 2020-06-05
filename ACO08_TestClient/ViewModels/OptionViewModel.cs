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
