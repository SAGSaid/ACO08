using System;
using System.Linq;
using ACO08_Library.Data.Validation;
using ACO08_Library.Enums;

namespace ACO08_Library.Communication.Protocol
{
    /// <summary>
    /// Encapsulates the implementation details of the options
    /// according to specification V4.1.5. This class is a singleton. 
    /// </summary>
    internal class OptionFactory
    {
        private readonly Option<bool>[] _boolOptions;
        private readonly Option<float>[] _floatOptions;
        private readonly Option<int>[] _intOptions;

        public OptionId[] BoolOptionIds { get; }
        public OptionId[] FloatOptionIds { get; }
        public OptionId[] IntOptionIds { get; }

        /// <summary>
        /// Singleton instance
        /// </summary>
        public static OptionFactory Instance { get; } = new OptionFactory();

        /// <summary>
        /// Copies the specified boolean option
        /// </summary>
        /// <param name="id">Which option to copy</param>
        /// <returns>A copy of the requested option</returns>
        /// <exception cref="InvalidOperationException">If the requested option isn't a boolean option.</exception>
        public Option<bool> CopyBoolOption(OptionId id)
        {
            if (BoolOptionIds.Contains(id))
            {
                return _boolOptions.First(option => option.Id == id).Copy();
            }

            throw new InvalidOperationException("The requested option isn't of bool type.");
        }

        /// <summary>
        /// Copies the specified float option
        /// </summary>
        /// <param name="id">Which option to copy</param>
        /// <returns>A copy of the requested option</returns>
        /// <exception cref="InvalidOperationException">If the requested option isn't a float option.</exception>
        public Option<float> CopyFloatOption(OptionId id)
        {

            if (FloatOptionIds.Contains(id))
            {
                return _floatOptions.First(option => option.Id == id).Copy(); 
            }

            throw new InvalidOperationException("The requested option isn't of float type.");
        }

        /// <summary>
        /// Copies the specified integer option
        /// </summary>
        /// <param name="id">Which option to copy</param>
        /// <returns>A copy of the requested option</returns>
        /// <exception cref="InvalidOperationException">If the requested option isn't an integer option.</exception>
        public Option<int> CopyIntOption(OptionId id)
        {
            if (IntOptionIds.Contains(id))
            {
                return _intOptions.First(option => option.Id == id).Copy();
            }

            throw new InvalidOperationException("The requested option isn't of int type.");
        }

        public Option<bool>[] CopyAllBoolOptions()
        {
            return _boolOptions.Select(option => option.Copy()).ToArray();
        }

        public Option<float>[] CopyAllFloatOptions()
        {
            return _floatOptions.Select(option => option.Copy()).ToArray();
        }

        public Option<int>[] CopyAllIntOptions()
        {
            return _intOptions.Select(option => option.Copy()).ToArray();
        }

        private OptionFactory()
        {
            _boolOptions = new[]
            {
                new Option<bool>(OptionId.InvertReadySignal,
                    false),

                new Option<bool>(OptionId.CheckArea,
                    false),

                new Option<bool>(OptionId.CheckTrouble,
                    false),

                new Option<bool>(OptionId.EnableInternalTrigger,
                    false),

                new Option<bool>(OptionId.AlwaysAcknowledgeError,
                    false),

                new Option<bool>(OptionId.InvertMeasureData,
                    true),

                new Option<bool>(OptionId.InvertErrorSignal,
                    false),

                new Option<bool>(OptionId.ShiftCurvePeak,
                    false),

                new Option<bool>(OptionId.EnableLCD,
                    false),

                new Option<bool>(OptionId.EnableAutoReference,
                    false),

                new Option<bool>(OptionId.EnableDHCP,
                    false),

                new Option<bool>(OptionId.EnableFilter,
                    true),

                new Option<bool>(OptionId.EnableDrift,
                    false),

                new Option<bool>(OptionId.CheckEnvelope,
                    true)
            };

            BoolOptionIds = _boolOptions.Select(option => option.Id).ToArray();
            
            _floatOptions = new[]
            {
                new Option<float>(OptionId.CalibrationFactor,
                    1, 
                    new RangeValidator<float>(0.01f, 100)),

                new Option<float>(OptionId.LowerEnvelope,
                    5,
                    new RangeValidator<float>(1, 50)),

                new Option<float>(OptionId.UpperEnvelope,
                    5,
                    new RangeValidator<float>(1, 50)),

                new Option<float>(OptionId.LowerTrouble,
                    30, 
                    new RangeValidator<float>(1, 90)),

                new Option<float>(OptionId.UpperTrouble,
                    30,
                    new RangeValidator<float>(1, 90)),

                new Option<float>(OptionId.LowerArea,
                    90,
                    new RangeValidator<float>(1, 90)),

                new Option<float>(OptionId.UpperArea,
                    90,
                    new RangeValidator<float>(1, 90))
            };

            FloatOptionIds = _floatOptions.Select(option => option.Id).ToArray();

            _intOptions = new[]
            {
                new Option<int>(OptionId.ReferenceCrimpAmount,
                    5,
                    new RangeValidator<int>(1, 100)),

                new Option<int>(OptionId.ObservationStartForce,
                    70,
                    new RangeValidator<int>(10, 99)),

                new Option<int>(OptionId.ObservationEndForce,
                    90,
                    new RangeValidator<int>(10, 99)),

                new Option<int>(OptionId.Gain,
                    0,
                    new RangeValidator<int>(0, 5)),

                new Option<int>(OptionId.SampleRate,
                    1,
                    new RangeValidator<int>(1, 4)),

                new Option<int>(OptionId.TriggerOffset,
                    0,
                    new RangeValidator<int>(-255, 255)),

                new Option<int>(OptionId.TriggerLevel,
                    10,
                    new RangeValidator<int>(1, 99)),

                new Option<int>(OptionId.DebugMessageTypes,
                    5,
                    new RangeValidator<int>(0, 15)),

                new Option<int>(OptionId.ActiveChannel,
                    1,
                    new DistinctIntegersValidator(1, 2, 3)),

                new Option<int>(OptionId.DefaultIP,
                    -1062731320), // This corresponds to IP Adress: 192.168.1.200

                new Option<int>(OptionId.SampleCount,
                    512,
                    new DistinctIntegersValidator(256, 512, 1024)),

                new Option<int>(OptionId.CommunicationFrameLength,
                    100,
                    new RangeValidator<int>(100, 500)),

                new Option<int>(OptionId.MaxDrift,
                    10,
                    new RangeValidator<int>(1, 20))
            };

            IntOptionIds = _intOptions.Select(option => option.Id).ToArray();
        }
    }
}
