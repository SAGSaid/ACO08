namespace ACO08_Library.Enums
{
    /// <summary>
    /// The device's different events are numerically differentiated.
    /// </summary>
    internal enum EventType : byte
    {
        Undefined = 0,
        CrimpDataChanged = 1,
        WorkmodeChanged = 2,
        MultireferenceChanged = 3
    }
}
