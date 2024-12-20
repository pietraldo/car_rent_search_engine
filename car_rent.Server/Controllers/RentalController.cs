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
            var rentedCars = _context.History
                .Where(h => h.User.UserName == userEmail) // Filter by user email
                .Include(h => h.Company)  // Eagerly load the related Company
                .Include(h => h.Offer)    // Eagerly load the related Offer
                .Select(h => new
                {
                    h.Rent_date,
                    h.Return_date,
                    h.Rent_ID,
                    h.Status,
                    CompanyName = h.Company != null ? h.Company.Name : "No Company", // Safe check for null Company
                    OfferPrice = h.Offer != null ? h.Offer.Price : 0, // Safe check for null Offer
                    OfferBrand = h.Offer != null ? h.Offer.Brand : "No Brand" // Safe check for null Offer
                });
                
            Console.WriteLine("Rented Cars Data:");
            foreach (var car in rentedCars)
            {
                Console.WriteLine($"Rent ID: {car.Rent_ID}, Status: {car.Status}, Company: {car.CompanyName}, Offer Price: {car.OfferPrice}, Offer Brand: {car.OfferBrand}");
            }

            return Ok(rentedCars);
        }


    }

}
