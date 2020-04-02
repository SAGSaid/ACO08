using System.Linq;
using ACO08_Library.Enums;

namespace ACO08_Library.Communication.Protocol
{
    internal class OptionFactory
    {
        private readonly Option<bool>[] _boolOptions;
        private readonly Option<float>[] _floatOptions;
        private readonly Option<int>[] _intOptions;

        public static OptionFactory Instance { get; } = new OptionFactory();

        public Option<bool> GetBoolOption(OptionId id)
        {
            return _boolOptions.First(option => option.Id == id).Copy();
        }

        public Option<float> GetFloatOption(OptionId id)
        {
            return _floatOptions.First(option => option.Id == id).Copy();
        }

        public Option<int> GetIntOption(OptionId id)
        {
            return _intOptions.First(option => option.Id == id).Copy();
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
                    false),
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
                    false),
                new Option<bool>(OptionId.EnableDrift,
                    false),
                new Option<bool>(OptionId.CheckEnvelope,
                    false)

            };

            _floatOptions = new[]
            {
                new Option<float>(OptionId.CalibrationFactor,
                    1.0f,
                    val => val >= 0.01 && val <= 100),
                new Option<float>(OptionId.LowerEnvelope,
                    5.0f,
                    val => val >= 1 && val <= 50),
                new Option<float>(OptionId.UpperEnvelope,
                    5.0f,
                    val => val >= 1 && val <= 50),
                new Option<float>(OptionId.LowerTrouble,
                    30.0f,
                    val => val >= 1 && val <= 90),
                new Option<float>(OptionId.UpperTrouble,
                    30.0f,
                    val => val >= 1 && val <= 90),
                new Option<float>(OptionId.LowerArea,
                    90.0f,
                    val => val >= 1 && val <= 90),
                new Option<float>(OptionId.UpperArea,
                    90.0f,
                    val => val >= 1 && val <= 90)
            };

            _intOptions = new[]
            {
                new Option<int>(OptionId.ReferenceCrimpAmount,
                    5,
                    val => val >= 1 && val <= 100),
                new Option<int>(OptionId.ObservationStartForce,
                    70,
                    val => val >= 10 && val <= 99),
                new Option<int>(OptionId.ObservationEndForce,
                    90,
                    val => val >= 10 && val <= 99),
                new Option<int>(OptionId.Gain,
                    0,
                    val => val >= 0 && val <= 5),
                new Option<int>(OptionId.SampleRate,
                    0,
                    val => val >= 1 && val <= 4),
                new Option<int>(OptionId.TriggerOffset,
                    0,
                    val => val >= -255 && val <= 255),
                new Option<int>(OptionId.TriggerLevel,
                    10,
                    val => val >= 1 && val <= 99),
                new Option<int>(OptionId.DebugMessageTypes,
                    5,
                    val => val >= 0 && val <= 15),
                new Option<int>(OptionId.GPIO_Function,
                    0),
                new Option<int>(OptionId.ActiveChannel,
                    1,
                    val => val >= 1 && val <= 3),
                new Option<int>(OptionId.DefaultIP,
                    -1062731320), // This corresponds to IP Adress: 192.168.1.200
                new Option<int>(OptionId.SampleCount,
                    512,
                    val => val == 256 || val == 512 || val == 1024),
                new Option<int>(OptionId.CommunicationFrameLength,
                    100,
                    val => val >= 100 && val <= 500),
                new Option<int>(OptionId.MaxDrift,
                    10,
                    val => val >= 1 && val <= 20)
            };
        }
    }
}
