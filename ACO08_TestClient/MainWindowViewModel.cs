﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using ACO08_Library.Communication.Networking;
using ACO08_Library.Public;
using ACO08_TestClient.Views;
using AsyncAwaitBestPractices.MVVM;

namespace ACO08_TestClient
{
    /// <summary>
    /// ViewModel for the application
    /// </summary>
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly DockPanel _container;
        private readonly Dispatcher _dispatcher;

        private ACO08_Device _selectedDevice;
        private bool _isLocating = false;
        private bool _isConnecting = false;

        private DeviceLocator _locator;

        public ObservableCollection<ACO08_Device> Devices { get; } =
            new ObservableCollection<ACO08_Device>();

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

        public bool IsConnecting
        {
            get { return _isConnecting; }
            private set
            {
                _isConnecting = value;
                OnPropertyChanged();
            }
        }

        public ICommand StartDiscoveringCommand { get; }
        public ICommand StopDiscoveringCommand { get; }
        public ICommand StartConnectingCommand { get; }
        public ICommand StopConnectingCommand { get; }
        public ICommand ClearDevicesCommand { get; }




        public MainWindowViewModel(DockPanel container)
        {
            _container = container;
            _container.Children.Add(new DiscoveryView());

            _dispatcher = Dispatcher.CurrentDispatcher;

            #region Commands Init

            StartDiscoveringCommand = 
                new RelayCommand(StartDiscoveringExecute, _ => !IsLocating);
            StopDiscoveringCommand = 
                new RelayCommand(StopDiscoveringExecute, _ => IsLocating);
            StartConnectingCommand =
                new AsyncCommand<object>(StartConnectingExecute, StartConnectingCanExecute);
            #endregion
        }

        private void StartDiscoveringExecute(object parameter)
        {
            if (!_isLocating)
            {
                IsLocating = true;
                _locator = new DeviceLocator();
                _locator.DeviceLocated += DeviceLocatedHandler;
                _locator.StartLocating();
            }
        }

        private void StopDiscoveringExecute(object parameter)
        {
            if (_isLocating)
            {
                IsLocating = false;
                _locator.StopLocating();
                _locator.DeviceLocated -= DeviceLocatedHandler;
                _locator.Dispose();
            }
        }

        private async Task StartConnectingExecute(object parameter)
        {
            if (parameter is ACO08_Device device)
            {
                IsConnecting = true;

                bool isConnected = await device.ConnectAsync();

                IsConnecting = false;

                if (isConnected)
                {
                    _container.Children.Clear();
                    _container.Children.Add(new DeviceView());
                }
                else
                {
                    MessageBox.Show("Connection failed.");
                }
            }
        }

        private bool StartConnectingCanExecute(object parameter)
        {
            return !IsConnecting;
        }

        private void DeviceLocatedHandler(object sender, DeviceLocatedEventArgs args)
        {
            if (Devices.All(dev => dev.SerialNumber != args.Device.SerialNumber))
            {
                _dispatcher.Invoke(() => Devices.Add(args.Device));
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
