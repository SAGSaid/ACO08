namespace ACO08_Library.Enums
{
    public enum ErrorId : byte
    {
        Ok = 0, // No error
        Timeout = 1,
        BufferLength = 2, // Conflict with a buffer length detected
        LengthNotReceived = 3, // The amount of demanded data bytes has not been specified
        UnknownCommand = 4,
        FrameIncomplete = 5, 
        InvalidInterface = 6, // Command is not allowed for current interface
        NoReferenceData = 7, 
        SD_CardAccessFailed = 8, 
        InvalidReference = 9, // Generating reference data caused an error
        Checksum = 10, 
        InvalidMode = 11, // Sent command is not allowed in current workmode
        FileAccessFailed = 12, 
        InvalidParameter = 13, // Atleast one parameter is invalid
        Transport = 14, // Generic error while receiving a frame
        UnknownOption = 15,
        InvalidValue = 16,
        NoDataReady = 17, 
        ReferenceMaxValueTooLow = 18,
        ReferenceMinValueTooHigh = 19, 
        ReferenceAreaTooSmall = 20, // Area corresponds to physical work
        ReferenceAreaTooBig = 21,
        HighPositiveDeviationFromPreviousReferences = 22,
        HighNegativeDeviationFromPreviousReferences = 23,
        MallocFailed = 24
    }
}
