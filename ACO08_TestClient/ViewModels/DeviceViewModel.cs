using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ACO08_Library.Enums;
using ACO08_Library.Public;
using ACO08_TestClient.Tools;

namespace ACO08_TestClient.ViewModels
{
    public class DeviceViewModel : INotifyPropertyChanged
    {
        public ACO08_Device Device { get; }

        //TODO Remove
        public string TestText { get; set; }

        public ICommand SetWorkmodeMainCommand { get; }
        public ICommand SetWorkmodeMeasureCommand { get; }
        public ICommand SetWorkmodeReferenceCommand { get; }
        public ICommand ShowOptionDialogCommand { get; }
        public ICommand ReferenceOkCommand { get; }
        public ICommand ReferenceNotOkCommand { get; }

        public DeviceViewModel(ACO08_Device device)
        {
            Device = device;
            Device.CrimpDataReceived += CrimpDataChangedHandler;

            #region Command Init

            SetWorkmodeMainCommand = new RelayCommand(SetWorkmodeMainExecute,
                _ => Device.CurrentWorkmode != Workmode.Main);

            SetWorkmodeMeasureCommand = new RelayCommand(SetWorkmodeMeasureExecute,
                _ => Device.CurrentWorkmode != Workmode.Measure);

            SetWorkmodeReferenceCommand = new RelayCommand(SetWorkmodeReferenceExecute,
                _ => Device.CurrentWorkmode != Workmode.Reference);

            ShowOptionDialogCommand = new RelayCommand(ShowOptionDialogExecute,
                _ => Device.CurrentWorkmode == Workmode.Main);

            ReferenceOkCommand = new RelayCommand(ReferenceOkExecute, 
                _ => Device.CurrentWorkmode == Workmode.ValidReference || 
                     Device.CurrentWorkmode == Workmode.InvalidReference);

            ReferenceNotOkCommand = new RelayCommand(ReferenceNotOkExecute,
                _ => Device.CurrentWorkmode == Workmode.ValidReference || 
                     Device.CurrentWorkmode == Workmode.InvalidReference);

            #endregion
        }

        private void SetWorkmodeMainExecute(object parameter)
        {
            Device.SetWorkmodeMain();
        }

        private void SetWorkmodeMeasureExecute(object parameter)
        {
            Device.SetWorkmodeMeasure();
        }

        private void SetWorkmodeReferenceExecute(object parameter)
        {
            Device.SetWorkmodeReference();
        }

        private void ShowOptionDialogExecute(object parameter)
        {
            Device.Options.GetAll();
            var optionWindow = new OptionWindow(Device.Options);

            optionWindow.ShowDialog();
        }

        private void ReferenceOkExecute(object parameter)
        {
            Device.ReferenceOk();
        }

        private void ReferenceNotOkExecute(object parameter)
        {
            Device.ReferenceNotOk();
        }

        private void CrimpDataChangedHandler(object sender, CrimpDataReceivedEventArgs args)
        {
            TestText = args.Data.ToString();
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
