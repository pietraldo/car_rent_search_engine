using car_rent_api2.Server.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using car_rent.Server.DTOs;

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
    public async Task<IActionResult> UpdateData([FromBody] EditUserModel editUserModel)
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