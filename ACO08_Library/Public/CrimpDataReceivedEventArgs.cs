using System;
using ACO08_Library.Data;

namespace ACO08_Library.Public
{
    /// <summary>
    /// Carries crimp data to event subscribers
    /// </summary>
    public class CrimpDataReceivedEventArgs : EventArgs
    {
        public CrimpData Data { get; }

        public CrimpDataReceivedEventArgs(CrimpData data)
        {
            Data = data;
        }
    }
}
