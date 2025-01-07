using car_rent_api2.Server.Database;
using car_rent.Server.Model;
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
    [HttpPost("updateUserData")]
    public async Task<IActionResult> UpdateUserData([FromBody] EditUserModel editUserModel)
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            return NotFound("User not found.");
        }

        user.FirstName = editUserModel.FirstName;
        user.LastName = editUserModel.LastName;
        user.City = editUserModel.City;
        user.Country = editUserModel.Country;
        user.HouseNumber = editUserModel.HouseNumber;
        user.DrivingLicenseIssueDate = editUserModel.DrivingLicenseIssueDate;
        user.DateOfBirth = editUserModel.DateOfBirth;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            return StatusCode(500, "Error updating user data.");
        }

        return Ok();
    }
}