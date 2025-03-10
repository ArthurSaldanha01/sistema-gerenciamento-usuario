using Microsoft.AspNetCore.Identity;
using System;

namespace backend.Models
{
    public class ApplicationUser : IdentityUser
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public required string Address { get; set; }
        public required string Gender { get; set; }
        public override string? PhoneNumber { get; set; }
        public required string JobTitle { get; set; }
    }
}
