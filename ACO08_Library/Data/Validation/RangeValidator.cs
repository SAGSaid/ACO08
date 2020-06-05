using System;

namespace ACO08_Library.Data.Validation
{
    internal class RangeValidator<T> : IValidateData<T> where T : IComparable<T>
    {
        public T Minimum { get; }
        public T Maximum { get; }

        public RangeValidator(T minimum, T maximum)
        {
            Minimum = minimum;
            Maximum = maximum;
        }

        public bool Validate(T data)
        {
            if (data.CompareTo(Minimum) < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(data), 
                    "The given value is smaller than the allowed minimum. " +
                    $"Minimum for this value is {Minimum}");
            }

            if (data.CompareTo(Maximum) > 0)
            {
                throw new ArgumentOutOfRangeException(nameof(data),
                    "The given value is bigger than the allowed maximum. " +
                    $"Minimum for this value is {Maximum}");
            }

            return true;
        }
    }
}
