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
        // Years are stored in bytes on the device.
        // The year 2000 is equivalent to the stored year 0.
        private const int YearOffset = 2000;

        public byte Second { get; }
        public byte Minute { get; }
        public byte Hour { get; }
        public byte Day { get; }
        public byte Weekday { get; }
        public byte Month { get; }
        public int Year { get; }

        public ACO_Time(byte[] rawBytes)
        {
            Second = rawBytes[0];
            Minute = rawBytes[1];
            Hour = rawBytes[2];
            Day = rawBytes[3];
            Weekday = rawBytes[4];
            Month = rawBytes[5];
            Year = rawBytes[6] + YearOffset;
        }

        public ACO_Time(DateTime time)
        {
            Second = (byte)time.Second;
            Minute = (byte)time.Minute;
            Hour = (byte)time.Hour;
            Day = (byte)time.Day;
            Weekday = (byte)time.DayOfWeek;
            Month = (byte)time.Month;
            Year = time.Year;
        }

        /// <summary>
        /// Provides the instance's information in the .NET DateTime type
        /// </summary>
        /// <returns>DateTime of equal value</returns>
        public DateTime ToDateTime()
        {
            return new DateTime(Year, Month, Day, Hour, Minute, Second);
        }

        /// <summary>
        /// Provides the byte representation of the data structure.
        /// </summary>
        /// <returns>The instance's data in byte form</returns>
        public List<byte> ToBytes()
        {
            // The data structure on the device is 8 bytes big,
            // but only 7 of the bytes are used to store information.
            // Last byte is just padding.
            return new List<byte>
            {
                Second,
                Minute,
                Hour,
                Day,
                Weekday,
                Month,
                (byte)(Year - YearOffset),
                Padding
            };
        }
    }
}
