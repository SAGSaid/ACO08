using System;

namespace ACO08_Library.Enums
{
    [Flags]
    public enum DebugMessageType
    {
        None = 0,
        System = 1,
        Info = 2,
        Error = 4,
        Data = 8
    }
}
