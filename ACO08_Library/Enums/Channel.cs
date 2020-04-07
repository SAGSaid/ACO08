namespace ACO08_Library.Enums
{
    /// <summary>
    /// Second byte of a header specifies the channel it targets.
    /// This enumeration limits the possible options.
    /// </summary>
    internal enum Channel : byte
    {
        None = 0,
        Channel1 = 1,
        Channel2 = 2,
        BothChannels = 3
    }
}
