using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ACO08_Library.Public;

namespace ACO08_TestClient.ViewModels
{
    public class DeviceViewModel : INotifyPropertyChanged
    {
        

        public ACO08_Device Device { get; }

        public DeviceViewModel(ACO08_Device device)
        {
            Device = device;
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
