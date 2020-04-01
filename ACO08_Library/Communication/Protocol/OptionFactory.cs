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

                // TODO Other boolean options
            };

            _floatOptions = new[]
            {
                new Option<float>(OptionId.CalibrationFactor,
                    "CalFactor",
                    1.0f,
                    val => val >= 0.01 && val <= 100.0),

                // TODO Other float options
            };

            _intOptions = new[]
            {
                new Option<int>(OptionId.ReferenceCrimpAmount,
                    "RefCrimpAmount",
                    5,
                    val => val >= 1 && val <= 100),

                // TODO Other int options
            };
        }
    }
}
