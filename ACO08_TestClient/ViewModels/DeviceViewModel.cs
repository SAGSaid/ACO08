using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using ACO08_Library.Enums;
using ACO08_Library.Public;
using ACO08_TestClient.Tools;

namespace ACO08_TestClient.ViewModels
{
    public class DeviceViewModel : INotifyPropertyChanged
    {
        public ACO08_Device Device { get; }

        public ICommand SetWorkmodeMainCommand { get; }
        public ICommand SetWorkmodeMeasureCommand { get; }
        public ICommand SetWorkmodeReferenceCommand { get; }
        public ICommand TempCommand { get; } //TODO Remove after done testing
        public ICommand ShowOptionDialogCommand { get; }

        public DeviceViewModel(ACO08_Device device)
        {
            Device = device;

            #region Command Init

            SetWorkmodeMainCommand = new RelayCommand(SetWorkmodeMainExecute, 
                _ => Device.CurrentWorkmode != Workmode.Main);

            SetWorkmodeMeasureCommand = new RelayCommand(SetWorkmodeMeasureExecute, 
                _ => Device.CurrentWorkmode != Workmode.Measure);

            SetWorkmodeReferenceCommand = new RelayCommand(SetWorkmodeReferenceExecute, 
                _ => Device.CurrentWorkmode != Workmode.Reference);

            TempCommand = new RelayCommand(_ =>
                {
                },
                _ => Device.CurrentWorkmode == Workmode.Main);

            ShowOptionDialogCommand = new RelayCommand(ShowOptionDialogExecute, 
                _ => Device.CurrentWorkmode == Workmode.Main);
            #endregion
        }


        private void SetWorkmodeMainExecute(object parameter)
        {
            bool result = Device.SetWorkmodeMain();

            if (!result)
            {
                MessageBox.Show("Error");
            }
        }
        private void SetWorkmodeMeasureExecute(object parameter)
        {
            bool result = Device.SetWorkmodeMeasure();

            if (!result)
            {
                MessageBox.Show("Error");
            }
        }
        private void SetWorkmodeReferenceExecute(object parameter)
        {
            bool result = Device.SetWorkmodeReference();

            if (!result)
            {
                MessageBox.Show("Error");
            }
        }

        private void ShowOptionDialogExecute(object obj)
        {
            Device.Options.UpdateOptions();
            var optionWindow = new OptionWindow(Device.Options);

            optionWindow.ShowDialog();
        }


        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        } 
        #endregion
    }
}
