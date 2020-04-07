using System;

namespace ACO08_Library.Enums
{
    /// <summary>
    /// The different states of the device's state machine.
    /// </summary>
    [Flags]
    public enum Workmode
    {
        All = -1, // 0xFFFF_FFFF
        Main = 0x0000_0001,
        Measure = 0x0000_0002,
        Reference = 0x0000_0004,
        Test = 0x0000_0008,
        BadCrimp = 0x0000_0010,
        InvalidReference = 0x0000_0020,
        ValidReference = 0x0000_0040,
        SetupCrimp = 0x0000_0200
    }
}
