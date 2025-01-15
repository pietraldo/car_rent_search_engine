using car_rent.Server.DataProvider;
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
        private readonly List<ICarRentalDataProvider> _carRentalProviders;

        public RentalsController(HttpClient httpClient, 
            UserManager<ApplicationUser> userManager, SearchEngineDbContext context,
            IEnumerable<ICarRentalDataProvider> carRentalProviders)
        {
            _httpClient = httpClient;
            _userManager = userManager;
            _context = context;
            _carRentalProviders = carRentalProviders.ToList();
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
                .Include(c=> c.Company)
                .Include(r => r.Offer)
                .Include(r=>r.Offer.Car)
                .ToListAsync();

            return Ok(rents);
        }

        [HttpGet("return/{rentId}")]
        public async Task<IActionResult> ReturnCar(string rentId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            

            var rent = await _context.History
                .Where(r => r.RentId_in_company == rentId)
                .Include(r => r.Offer)
                .Include(r => r.Offer.Car)
                .Include(r => r.Company)
                .FirstOrDefaultAsync();

            bool success = false;
            foreach(var provider in _carRentalProviders)
            {
                if(provider.GetProviderName()== rent.Company.Name)
                {
                    success=provider.ReturnCar(rentId).Result;
                    break;
                }
            }
            if(success == false)
            {
                return BadRequest("Error while returning the car.");
            }

            rent.Status = RentStatus.Returned;

            await _context.SaveChangesAsync();

            return Ok();
        }
    }

}
