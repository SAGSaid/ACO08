﻿namespace ACO08_Library.Enums
{
    /// <summary>
    /// The option's numeric representation
    /// </summary>
    public enum OptionId : byte
    {
        ReferenceCrimpAmount = 2,
        CalibrationFactor = 3,
        ObservationStartForce = 4,
        ObservationEndForce = 5,
        LowerEnvelope = 6,
        UpperEnvelope = 7,
        LowerTrouble = 8,
        UpperTrouble = 9,
        InvertReadySignal = 10,
        LowerArea = 11,
        UpperArea = 12,
        CheckArea = 13,
        CheckTrouble = 14,
        EnableInternalTrigger = 15,
        Gain = 16,
        AlwaysAcknowledgeError = 17,
        SampleRate = 18,
        TriggerOffset = 19,
        InvertMeasureData = 20,
        InvertErrorSignal = 21,
        TriggerLevel = 22,
        ShiftCurvePeak = 23,
        EnableLCD = 24,
        DebugMessageTypes = 25,
        EnableAutoReference = 27,
        ActiveChannel = 28,
        DefaultIP = 29,
        SampleCount = 30,
        CommunicationFrameLength = 31,
        EnableDHCP = 32,
        EnableFilter = 33,
        EnableDrift = 34,
        MaxDrift = 35,
        CheckEnvelope = 36,
    }
}
