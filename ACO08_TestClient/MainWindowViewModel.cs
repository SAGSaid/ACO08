﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using ACO08_Library.Public;
using ACO08_TestClient.Views;

namespace ACO08_TestClient
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private ACO08_Device _selectedDevice;
        private DockPanel _container;

        public TestClientInterface Model { get; }
        

        public ACO08_Device SelectedDevice
        {
            get { return _selectedDevice; }
            private set
            {
                _selectedDevice = value;
                OnPropertyChanged();
            }
        }


        public MainWindowViewModel(TestClientInterface model, DockPanel container)
        {
            Model = model;
            _container = container;

            _container.Children.Add(new DiscoveryView());
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
