using Microsoft.AspNetCore.Identity;

namespace car_rent_api2.Server.Database
{
    public class ApplicationUser: IdentityUser<Guid>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? HouseNumber { get; set; }
        public DateTime? DrivingLicenseIssueDate { get; set; }
        public DateTime? DateOfBirth { get; set; }
        // Navigation property
        public ICollection<Rent> Rents { get; set; }
    }
}
