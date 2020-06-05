namespace ACO08_Library.Data.Validation
{
    internal class NonValidatingValidator<T> : IValidateData<T>
    {
        public bool Validate(T data)
        {
            return true;
        }
    }
}
