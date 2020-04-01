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
                    "ReadySignalInverse",
                    false),
                new Option<bool>(OptionId.CheckArea,
                    "AreaCheck",
                    false),
                new Option<bool>(OptionId.CheckTrouble,
                    "TroubleCheck",
                    false),
                new Option<bool>(OptionId.EnableInternalTrigger,
                    "Trigger",
                    false),
                new Option<bool>(OptionId.AlwaysAcknowledgeError,
                    "AllwaysAcknowledgeError", // That is an intentional typo, the specification demands it
                    false),
                new Option<bool>(OptionId.InvertMeasureData,
                    "InverseMeasureData",
                    false),
                new Option<bool>(OptionId.InvertErrorSignal,
                    "ErrorSignalInverse", 
                    false),
                new Option<bool>(OptionId.ShiftCurvePeak,
                    "ShiftCurvePeak", 
                    false),
                new Option<bool>(OptionId.EnableLCD,
                    "LCD_Enable", 
                    false),
                new Option<bool>(OptionId.EnableAutoReference,
                    "AutoRefCheck", 
                    false),
                new Option<bool>(OptionId.EnableDHCP,
                    "DHCP_enable", 
                    false),
                new Option<bool>(OptionId.EnableFilter,
                    "FilterActive", 
                    false),
                new Option<bool>(OptionId.EnableDrift,
                    "FilterActive", 
                    false),
                new Option<bool>(OptionId.CheckEnvelope,
                    "EnvelopeCheck", 
                    false)

            };

            _floatOptions = new[]
            {
                new Option<float>(OptionId.CalibrationFactor,
                    "CalFactor",
                    1.0f,
                    val => val >= 0.01 && val <= 100),
                new Option<float>(OptionId.LowerEnvelope,
                    "LowerEnvelope",
                    5.0f,
                    val => val >= 1 && val <= 50),
                new Option<float>(OptionId.UpperEnvelope,
                    "UpperEnvelope",
                    5.0f,
                    val => val >= 1 && val <= 50),
                new Option<float>(OptionId.LowerTrouble,
                    "LowerTrouble",
                    30.0f,
                    val => val >= 1 && val <= 90),
                new Option<float>(OptionId.UpperTrouble,
                    "UpperTrouble",
                    30.0f,
                    val => val >= 1 && val <= 90),
                new Option<float>(OptionId.LowerArea,
                    "LowerArea",
                    90.0f,
                    val => val >= 1 && val <= 90),
                new Option<float>(OptionId.UpperArea,
                    "UpperArea",
                    90.0f,
                    val => val >= 1 && val <= 90)
            };

            _intOptions = new[]
            {
                new Option<int>(OptionId.ReferenceCrimpAmount,
                    "RefCrimpAmount",
                    5,
                    val => val >= 1 && val <= 100),
                new Option<int>(OptionId.ObservationStartForce,
                    "ObservationStartForce",
                    70,
                    val => val >= 10 && val <= 99),
                new Option<int>(OptionId.ObservationEndForce,
                    "ObservationEndForce",
                    90,
                    val => val >= 10 && val <= 99),
                new Option<int>(OptionId.Gain,
                    "Gain",
                    0,
                    val => val >= 0 && val <= 5),
                new Option<int>(OptionId.SampleRate,
                    "SampleRate",
                    0,
                    val => val >= 1 && val <= 4),
                new Option<int>(OptionId.TriggerOffset,
                    "SampleRate",
                    0,
                    val => val >= -255 && val <= 255),
                new Option<int>(OptionId.TriggerLevel,
                    "TriggerLevel",
                    10,
                    val => val >= 1 && val <= 99),
                new Option<int>(OptionId.DebugMessageTypes,
                    "DbgMsgTypes",
                    5,
                    val => val >= 0 && val <= 15),
                new Option<int>(OptionId.GPIO_Function,
                    "GPIO_Function",
                    0),
                new Option<int>(OptionId.ActiveChannel,
                    "Active_Channel",
                    1,
                    val => val >= 1 && val <= 3),
                new Option<int>(OptionId.DefaultIP,
                    "Default_IP",
                    -1062731320), // This corresponds to IP Adress: 192.168.1.200
                new Option<int>(OptionId.SampleCount,
                    "SampleCount",
                    512,
                    val => val == 256 || val == 512 || val == 1024),
                new Option<int>(OptionId.CommunicationFrameLength,
                    "ComFrameLength",
                    100,
                    val => val >= 100 && val <= 500),
                new Option<int>(OptionId.MaxDrift,
                    "MaxDrift",
                    10,
                    val => val >= 1 && val <= 20)
            };
        }
    }
}
