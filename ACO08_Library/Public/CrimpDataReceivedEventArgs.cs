using System;
using ACO08_Library.Data;

namespace ACO08_Library.Public
{
    public class CrimpDataReceivedEventArgs : EventArgs
    {
        public CrimpData Data { get; }

        public CrimpDataReceivedEventArgs(CrimpData data)
        {
            Data = data;
        }
    }
}
