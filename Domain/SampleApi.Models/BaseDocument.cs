namespace SampleApi.Models
{
    //'documents' are called as the records under collection in mongo db
    public abstract class BaseDocument
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
