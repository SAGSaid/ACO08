﻿namespace ACO08_Library.Enums
{
    /// <summary>
    /// Every command has a numeric ID. This enumeration contains all needed ID.
    /// </summary>
    public enum CommandId : byte
    {
        GetVersion = 0,
        GetWorkmode = 1,
        SetWorkmodeMain = 2,
        SetWorkmodeMeasure = 3,
        SetWorkmodeReference = 4,
        SetWorkmodeTest = 5,
        GetCrimpData = 7,
        NextBlock = 8, // Notifies that the next block of data can be sent
        GetReferenceData = 9,
        GetLowerEnvelope = 10,
        GetUpperEnvelope = 11,
        ReadCrimpRecord = 12,
        ReferenceOk = 13,
        ReferenceNotOk = 14,
        GetCrimpCounter = 15,
        GetTime = 16,
        SetTime = 17,
        GetOption = 18,
        SetOption = 19,
        SaveSetup = 20,
        GetOptionList = 21,
        Reset = 22,
        GetAreaDeviation = 26,
        GetMaxima = 27,
        GetForce = 33,
        GetSerialNumber = 35,
        GetDebugMessageTypes = 37,
        SetDebugMessageTypes = 38,
        GetLCD_Activation = 39,
        SetLCD_Activation = 40,
        GetGPIO_Function = 41,
        SetGPIO_Function = 42,
        ResetFactorySetup = 43,
        GetGPIO_Status = 44,
        GetTrendList = 46,
        SetMultireference = 47,
        GetMultireference = 48,
    }
}
