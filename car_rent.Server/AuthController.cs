using Microsoft.AspNetCore.Mvc;

namespace car_rent.Server
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // Your logic to check username and password
            if (request.Login == "admin" && request.Password == "password") // Example logic
            {
                return Ok(new { message = "Login successful" });
            }
            else
            {
                return Unauthorized(new { message = "Invalid login or password" });
            }
        }
    }

    public class LoginRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }

}
