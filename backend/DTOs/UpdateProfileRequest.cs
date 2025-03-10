namespace backend.DTOs
{
    public class UpdateProfileRequest
    {
        public required string NewEmail { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public required string Address { get; set; }
        public required string Gender { get; set; }
        public required string JobTitle { get; set; }
    }
}
