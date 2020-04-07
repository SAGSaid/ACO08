using System;
using ACO08_Library.Enums;

namespace ACO08_Library.Communication.Protocol
{
    /// <summary>
    /// Encapsulates an option of the device in a generic way.
    /// </summary>
    /// <typeparam name="T">The underlying data type of the option</typeparam>
    public class Option<T>
    {
        private T _value;
        private readonly Predicate<T> _validateValue;

        public OptionId Id { get; }
        public T DefaultValue { get; }
        public T Value
        {
            get { return _value; }
            set
            {
                if (_validateValue(value))
                {
                    _value = value;
                }
            }
        }

        internal Option(OptionId id, T defaultValue, Predicate<T> validateValue = null)
        {
            _validateValue = validateValue ?? (_ => true);

            // Usually throwing in constructor is finicky, but this helps during testing.
            // It isn't a concern for runtime issues.
            if (!_validateValue(defaultValue))
            {
                throw new ArgumentException(
                    "The defaultValue given is invalid according to the given validateValue predicate.");
            }

            Id = id;
            DefaultValue = defaultValue;
            _value = defaultValue;
        }

        /// <summary>
        /// Copies the instance.
        /// </summary>
        /// <returns>A copy of the instance</returns>
        public Option<T> Copy()
        {
            return new Option<T>(Id, DefaultValue, _validateValue);
        }
        

    }
}
