using System.Security.Claims;
using car_rent_api2.Server.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace car_rent.Server.Controllers;

[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [Authorize]
    [HttpPost("updatedata")]
    public IActionResult UpdateData(string firstName, string lastName, string city, string country, string houseNumber, DateTime drivingLicenseIssueDate, DateTime dateOfBirth)
    {
        var user = _userManager.GetUserAsync(User);
        
        user.Result.FirstName = firstName;
        user.Result.LastName = lastName;
        user.Result.City = city;
        user.Result.Country = country;
        user.Result.HouseNumber = houseNumber;
        user.Result.DrivingLicenseIssueDate = drivingLicenseIssueDate;
        user.Result.DateOfBirth = dateOfBirth;

        return Ok();
    }
}