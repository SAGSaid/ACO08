using System;
using System.Windows.Input;

namespace ACO08_TestClient.Tools
{
    /// <summary>
    /// Implements ICommand and allows binding of external methods.
    /// Source: https://www.c-sharpcorner.com/UploadFile/20c06b/icommand-and-relaycommand-in-wpf/
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute ?? (_ => true);
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}
