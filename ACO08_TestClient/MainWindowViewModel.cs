﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using ACO08_Library.Public;
using ACO08_TestClient.Views;
using AsyncAwaitBestPractices.MVVM;

namespace ACO08_TestClient
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private DockPanel _container;

        private ACO08_Device _selectedDevice;
        private bool _isLocating = false;

        public TestClientInterface Model { get; } = new TestClientInterface();


        public ACO08_Device SelectedDevice
        {
            get { return _selectedDevice; }
            private set
            {
                _selectedDevice = value;
                OnPropertyChanged();
            }
        }

        public bool IsLocating
        {
            get { return _isLocating; }
            private set
            {
                _isLocating = value;
                OnPropertyChanged();
            }
        }

        public ICommand StartDiscoveringCommand { get; }
        public ICommand StopDiscoveringCommand { get; }



        public MainWindowViewModel(DockPanel container)
        {
            _container = container;
            _container.Children.Add(new DiscoveryView());

            #region Commands

            StartDiscoveringCommand = new RelayCommand(StartDiscoveringExecute, _ => !IsLocating);
            StopDiscoveringCommand = new RelayCommand(StopDiscoveringExecute, _ => IsLocating);

            #endregion
        }

        private void StartDiscoveringExecute(object parameter)
        {
            if (!_isLocating)
            {
                IsLocating = true;
                Model.StartLocatingDevices();
            }
        }

        private void StopDiscoveringExecute(object parameter)
        {
            if (_isLocating)
            {
                IsLocating = false;
                Model.StopLocatingDevices();
            }
        }
        



        #region INotifyPropertyChanged

        // Autogenerated by ReSharper
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        } 
        #endregion
    }
}
