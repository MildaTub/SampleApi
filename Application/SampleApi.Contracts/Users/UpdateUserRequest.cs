using System.ComponentModel.DataAnnotations;

namespace SampleApi.Contracts.Users
{
    public class UpdateUserRequest
    {
        [Required, StringLength(100, MinimumLength = 1)]
        public string FirstName { get; set; }

        [Required, StringLength(100, MinimumLength = 1)]
        public string LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, Phone]
        public string PhoneNumber { get; set; }
    }
}