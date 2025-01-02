using car_rent.Server.Migrations;
using car_rent.Server.Model;
using car_rent_api2.Server.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace car_rent.Server.Controllers
{
    [ApiController]
    [Route("api2/[controller]")]
    public class RentalsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SearchEngineDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;

        public RentalsController(HttpClient httpClient, string car_rent_company_api1, UserManager<ApplicationUser> userManager, SearchEngineDbContext context)
        {
            _httpClient = httpClient;
            _userManager = userManager;
            _apiUrl = car_rent_company_api1;
            _context = context;
        }
        [HttpGet("rents")]
        public async Task<ActionResult<IEnumerable<Rent>>> Get()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var rents = await _context.History
                .Where(r => r.User == user)
                .Include(r => r.Offer)
                .Include(r=>r.Offer.Car)
                .ToListAsync();

            return Ok(rents);
        }

        [HttpGet("return/{rentId}")]
        public async Task<IActionResult> ReturnCar(int rentId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var rentCarResponse = await _httpClient.GetAsync($"{_apiUrl}/api/Rent/readyToReturn/{rentId}");
            if (!rentCarResponse.IsSuccessStatusCode)
            {
                return StatusCode((int)rentCarResponse.StatusCode, "Error renting car in external API");
            }

            var rent = await _context.History
                .Where(r => r.RentId_in_company == rentId)
                .Include(r => r.Offer)
                .Include(r => r.Offer.Car)
                .FirstOrDefaultAsync();

            rent.Status = RentStatus.Returned;

            await _context.SaveChangesAsync();

            return Ok();
        }
    }

}
