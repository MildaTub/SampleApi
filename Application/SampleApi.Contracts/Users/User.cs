using System;

namespace SampleApi.Contracts.Users
{
    public sealed class User : CreateUserRequest
    {
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
