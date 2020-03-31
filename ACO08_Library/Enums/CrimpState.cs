using System;

namespace ACO08_Library.Enums
{
    [Flags]
    public enum CrimpState : ushort
    {
        Ok = 0x0000,
        ForceTooHigh = 0x0001,
        ForceTooLow = 0x0002,
        ForceMuchTooHigh = 0x0004, 
        ForceMuchTooLow = 0x0008, 
        EnvelopeTooHigh = 0x0010, 
        EnvelopeTooLow = 0x0020, 
        AreaTooHigh = 0x0040, // Area corresponds to physical work
        AreaTooLow = 0x0080 
    }
}
