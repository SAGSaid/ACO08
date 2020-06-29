using System;
using System.Collections.Generic;
using ACO08_Library.Enums;

namespace ACO08_Library.Data
{
    /// <summary>
    /// Encapsulates a crimp data entity.
    /// </summary>
    public class CrimpData
    {
        public uint ReferenceId { get; }
        public CrimpState CrimpState { get; }
        public short CrimpForce { get; }
        public uint CrimpWork { get; }
        public uint CrimpCounter { get; }
        public uint BadCrimpCounter { get; }
        public ushort Gain { get; }
        public ushort Reserve { get; }
        public ACO_Time DateTime { get; }
        public float CalibrationFactor { get; }
        public int Drift { get; }
        public int ObservationStartForce { get; }
        public int ObservationEndForce { get; }
        public uint ObservationStartIndex { get; }
        public uint ObservationEndIndex { get; }
        public float LowerEnvelope { get; }
        public float UpperEnvelope { get; }
        public float LowerArea { get; }
        public float UpperArea { get; }
        public bool InvertReadySignal { get; }
        public uint UpperEnvelopeErrorCount { get; }
        public uint LowerEnvelopeErrorCount { get; }
        public uint UpperTroubleError { get; }
        public uint LowerTroubleError { get; }
        public List<short> MeasureData { get; }

        public CrimpData(byte[] rawData)
        {
            int index = 0;
            ReferenceId = BitConverter.ToUInt32(rawData, index);
            index = sizeof(uint);

            CrimpState = (CrimpState)BitConverter.ToUInt16(rawData, index);
            index += sizeof(ushort);

            CrimpForce = BitConverter.ToInt16(rawData, index);
            index += sizeof(short);

            CrimpWork = BitConverter.ToUInt32(rawData, index);
            index += sizeof(uint);

            CrimpCounter = BitConverter.ToUInt32(rawData, index);
            index += sizeof(uint);

            BadCrimpCounter = BitConverter.ToUInt32(rawData, index);
            index += sizeof(uint);

            Gain = BitConverter.ToUInt16(rawData, index);
            index += sizeof(ushort);

            Reserve = BitConverter.ToUInt16(rawData, index);
            index += sizeof(ushort);

            var dateBytes = new byte[8];
            Array.Copy(rawData, index, dateBytes, 0, 8);
            DateTime = new ACO_Time(dateBytes);
            index += 8;

            CalibrationFactor = BitConverter.ToSingle(rawData, index);
            index += sizeof(float);

            Drift = BitConverter.ToInt32(rawData, index);
            index += sizeof(int);

            ObservationStartForce = BitConverter.ToInt32(rawData, index);
            index += sizeof(int);

            ObservationEndForce = BitConverter.ToInt32(rawData, index);
            index += sizeof(int);

            ObservationStartIndex = BitConverter.ToUInt32(rawData, index);
            index += sizeof(uint);

            ObservationEndIndex = BitConverter.ToUInt32(rawData, index);
            index += sizeof(uint);

            LowerEnvelope = BitConverter.ToSingle(rawData, index);
            index += sizeof(float);

            UpperEnvelope = BitConverter.ToSingle(rawData, index);
            index += sizeof(float);

            LowerArea = BitConverter.ToSingle(rawData, index);
            index += sizeof(float);

            UpperArea = BitConverter.ToSingle(rawData, index);
            index += sizeof(float);

            InvertReadySignal = BitConverter.ToUInt32(rawData, index) > 0;
            index += sizeof(uint);

            UpperEnvelopeErrorCount = BitConverter.ToUInt32(rawData, index);
            index += sizeof(uint);

            LowerEnvelopeErrorCount = BitConverter.ToUInt32(rawData, index);
            index += sizeof(uint);

            UpperTroubleError = BitConverter.ToUInt32(rawData, index);
            index += sizeof(uint);

            LowerTroubleError = BitConverter.ToUInt32(rawData, index);
            index += sizeof(uint);

            // Give the list an initial size, so there is only one memory allocation
            MeasureData = new List<short>(rawData.Length - 92);

            for (; index < rawData.Length; index += sizeof(short))
            {
                MeasureData.Add(BitConverter.ToInt16(rawData, index));
            }

        }

    }
}
