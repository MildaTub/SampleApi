namespace SampleApi.Models.Exceptions;

[Serializable]
public sealed class DomainException : Exception
{
    public DomainException(string message) : base(message)
    {
    }
}