namespace ACO08_Library.Data.Validation
{
    internal class NonValidatingValidator<T> : IValidateData<T>
    {
        public static readonly NonValidatingValidator<T> Instance = new NonValidatingValidator<T>();

        private NonValidatingValidator() { }

        public bool Validate(T data)
        {
            return true;
        }
    }
}
