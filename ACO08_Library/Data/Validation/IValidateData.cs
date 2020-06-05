namespace ACO08_Library.Data.Validation
{
    internal interface IValidateData<in T>
    {
        bool Validate(T data);
    }
}
