﻿using System.Security.Claims;
using car_rent_api2.Server.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace car_rent.Server.Controllers;

[Route("api/[controller]")]
public class IdentityController : ControllerBase
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public IdentityController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpPost("google-login")]
    [AllowAnonymous]
    public IActionResult GoogleLogin()
    {
        var redirectUrl = "/api/Identity/google-login-callback";
        var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);

        return Challenge(properties, "Google");
    }

    [HttpPost("google-logout")]
    [AllowAnonymous]
    public async Task<IActionResult> GoogleLogout()
    {
        Response.Cookies.Delete("UserEmail");
        await _signInManager.SignOutAsync();
        return Redirect("/"); 
    }


    [HttpGet("google-login-callback")]
    [AllowAnonymous]
    public async Task<IActionResult> GoogleCallback()
    {
        // Get external login info
        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null) return Unauthorized("Error: External login info could not be retrieved.");

        // Attempt to sign in the user with external login info
        var signInResult = await _signInManager.ExternalLoginSignInAsync(
            info.LoginProvider,
            info.ProviderKey,
            false);

        var email = info.Principal.FindFirstValue(ClaimTypes.Email);

        if (signInResult.Succeeded)
        {
            // Store email in a cookie
            Response.Cookies.Append("UserEmail", email, new CookieOptions
            {
                HttpOnly = false, // Allow JavaScript to access
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            });

            return Redirect("/");
        }
        
        if (string.IsNullOrEmpty(email)) return BadRequest("Error: Email is not provided by the external provider.");

        var user = new ApplicationUser { UserName = email, Email = email };
        var result = await _userManager.CreateAsync(user);

        if (!result.Succeeded) return BadRequest("Error: User creation failed.");

        // Link external login to the newly created user
        await _userManager.AddLoginAsync(user, info);

        // Sign in the user
        await _signInManager.SignInAsync(user, false);

        Response.Cookies.Append("UserEmail", email, new CookieOptions
        {
            HttpOnly = false, // Allow JavaScript to access
            Expires = DateTimeOffset.UtcNow.AddHours(1)
        });
        
        return Redirect("/FillUserDataAfterLogin");
    }
}