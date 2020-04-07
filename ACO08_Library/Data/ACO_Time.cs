using System;
using System.Collections.Generic;

namespace ACO08_Library.Data
{
    /// <summary>
    /// Allows translation from the device's own time data type
    /// to the more comfortable .NET DateTime and vice versa.
    /// </summary>
    public class ACO_Time
    {
        private const byte Padding = 0;

        public byte YearMinus2000 { get; }
        public byte Month { get; }
        public DayOfWeek Weekday { get; }
        public byte Day { get; }
        public byte Hour { get; }
        public byte Minute { get; }
        public byte Second { get; }

        public ACO_Time(byte[] rawBytes)
        {
            // First byte is just padding
            YearMinus2000 = rawBytes[1];
            Month = rawBytes[2];
            Weekday = (DayOfWeek)rawBytes[3];
            Day = rawBytes[4];
            Hour = rawBytes[5];
            Minute = rawBytes[6];
            Second = rawBytes[7];
        }

        public ACO_Time(DateTime time)
        {
            YearMinus2000 = (byte)(time.Year - 2000);
            Month = (byte)time.Month;
            Weekday = time.DayOfWeek;
            Day = (byte)time.Day;
            Hour = (byte)time.Hour;
            Minute = (byte)time.Minute;
            Second = (byte)time.Second;
        }

        /// <summary>
        /// Provides the instance's information in the .NET DateTime type
        /// </summary>
        /// <returns>DateTime of equal value</returns>
        public DateTime ToDateTime()
        {
            return new DateTime(YearMinus2000 + 2000, Month, Day, Hour, Minute, Second);
        }

        /// <summary>
        /// Provides the byte representation of the data structure.
        /// </summary>
        /// <returns>The instance's data in byte form</returns>
        public List<byte> ToBytes()
        {
            return new List<byte>
            {
                Padding,
                YearMinus2000,
                Month,
                (byte)Weekday,
                Day,
                Hour,
                Minute,
                Second
            };
        }
    }
}
