using System;
using System.Linq;

namespace ACO08_Library.Data.Validation
{
    internal class DistinctIntegersValidator : IValidateData<int>
    {
        public int[] ValidValues { get; }

        public DistinctIntegersValidator(params int[] validValues)
        {
            ValidValues = validValues;
        }

        public bool Validate(int data)
        {
            if (!ValidValues.Contains(data))
            {
                throw new ArgumentException($"The given integer is not valid. " +
                                            $"Valid integers include {ValidValues}.");
            }

            return true;
        }
    }
}
