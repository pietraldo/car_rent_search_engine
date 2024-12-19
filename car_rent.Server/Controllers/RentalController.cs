using car_rent.Server.Model;
using car_rent_api2.Server.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace car_rent.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RentalsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SearchEngineDbContext _context;
        private readonly HttpClient _httpClient;
        public RentalsController(UserManager<ApplicationUser> userManager, SearchEngineDbContext context, HttpClient httpClient)
        {
            _userManager = userManager;
            _context = context;
            _httpClient = httpClient;
        }
        [HttpGet]
        public IActionResult GetHistory([FromHeader(Name = "X-User-Email")] string userEmail)
        {

            var user_id = _context.Users
                .Where(u => u.Email == userEmail);

            var rentedCars = _context.History
                .Where(h => h.User.UserName == userEmail)
                .ToList();
            Console.WriteLine("Rented Cars Data:");
            foreach (var car in rentedCars)
            {
                Console.WriteLine($"Rent ID: {car.Rent_ID}, Status: {car.Status}, Company: {car.Company?.Name}");
            }

            return Ok(rentedCars);

        }

    }

}
