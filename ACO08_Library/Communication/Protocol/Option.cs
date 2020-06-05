using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ACO08_Library.Data.Validation;
using ACO08_Library.Enums;

namespace ACO08_Library.Communication.Protocol
{
    /// <summary>
    /// Encapsulates an option of the device in a generic way.
    /// </summary>
    /// <typeparam name="T">The underlying data type of the option</typeparam>
    public class Option<T> : INotifyPropertyChanged
    {
        private T _value;
        private readonly IValidateData<T> _validator;

        public OptionId Id { get; }
        public T DefaultValue { get; }
        public T Value
        {
            get { return _value; }
            set
            {
                if (_validator.Validate(value))
                {
                    _value = value;
                    OnPropertyChanged();
                }
            }
        }

        internal Option(OptionId id, T defaultValue, IValidateData<T> validateValue = null)
        {
            _validator = validateValue ?? new NonValidatingValidator<T>();

            // Usually throwing in constructor is finicky, but this helps during debugging.
            // It doesn't create runtime issues.
            _validator.Validate(defaultValue);

            Id = id;
            DefaultValue = defaultValue;
            Value = defaultValue;
        }

        /// <summary>
        /// Copies the instance.
        /// </summary>
        /// <returns>A copy of the instance</returns>
        public Option<T> Copy()
        {
            return new Option<T>(Id, DefaultValue, _validator);
        }


        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        } 
        #endregion
    }
}
