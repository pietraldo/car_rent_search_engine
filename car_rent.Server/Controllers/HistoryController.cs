using car_rent.Server.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace car_rent.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HistoryController : ControllerBase
    {
        private readonly SearchEngineDbContext _context;

        public HistoryController(SearchEngineDbContext context)
        {
            _context = context;
        }

        [HttpGet("get-history")]
        public async Task<IActionResult> GetRents([FromQuery] int user_id)
        {
            if (user_id <= 0)
            {
                return BadRequest("Invalid User ID.");
            }
            user_id = 3;
            // Query rents for the given user_id and include the corresponding offers and cars asynchronously
            var rents = await _context.History
                .Where(rent => rent.User_ID == user_id)
                .Include(rent => rent.Offer)  // Include the related Offer entity
                    .ThenInclude(offer => offer.Car)  // Include the related Car entity within Offer
                .ToListAsync();  // Use ToListAsync for async execution

            // Project the result to RentWithOfferDTO to include Rent, Offer, and Car data
            var result = rents.Select(rent => new RentWithOfferDTO
            {
                Rent_ID = rent.Rent_ID,
                Rent_date = rent.Rent_date,
                Return_date = rent.Return_date,
                Status = rent.Status,
                User_ID = rent.User_ID,
                Offer_ID = rent.Offer.Offer_ID,  // Include Offer_ID
                Model = rent.Offer.Car.Model,  // Include Car Model
                Brand = rent.Offer.Car.Brand,  // Include Car Brand
                Year = rent.Offer.Car.Year,  // Include Car Year
            }).ToList();

            return Ok(result);
        }
        public class RentWithOfferDTO
        {
            public int Rent_ID { get; set; }
            public DateTime Rent_date { get; set; }
            public DateTime Return_date { get; set; }
            public string Status { get; set; }
            public int User_ID { get; set; }
            public int Offer_ID { get; set; }
            public int Company_ID { get; set; }
            public double Price { get; set; }

            // Car-related details
            public string Model { get; set; }
            public string Brand { get; set; }
            public int Year { get; set; }
            public string Color { get; set; }
            public string Picture { get; set; }
        }

    }
}
