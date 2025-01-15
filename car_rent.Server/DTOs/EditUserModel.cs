namespace car_rent.Server.DTOs;

public class EditUserModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public string HouseNumber { get; set; }
    public DateTime DrivingLicenseIssueDate { get; set; }
    public DateTime DateOfBirth { get; set; }
}