using System.Windows;
using System.Windows.Input;
using ACO08_Library.Public;
using ACO08_TestClient.Tools;

namespace ACO08_TestClient.ViewModels
{
    public class OptionViewModel
    {
        public ACO08_Options Options { get; }

        public ICommand CommitChangesCommand { get; }

        public OptionViewModel(ACO08_Options options)
        {
            Options = options;

            #region CommandInit

            CommitChangesCommand = new RelayCommand(ExecuteCommitChanges);

            #endregion
        }

        private void ExecuteCommitChanges(object parameter)
        {
            Options.SetChangedOptions();

            var frame = parameter as Window;
            frame?.Close();
        }
    }
}
