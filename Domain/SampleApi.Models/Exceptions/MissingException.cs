namespace SampleApi.Models.Exceptions;

[Serializable]
public class MissingEntityException : Exception
{
    public string EntityName { get; }

    public MissingEntityException(string type) : base($"Entity {type} not found.")
    {
        EntityName = type;
    }
}