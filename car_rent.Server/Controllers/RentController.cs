using car_rent.Server.Model;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace car_rent.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RentController : ControllerBase
    {
        private readonly SearchEngineDbContext _context;

        public RentController(SearchEngineDbContext context)
        {
            _context = context;
        }

        [HttpPost("reserve")]
        public async Task<IActionResult> ReserveCar([FromBody] RentRequestDto rentRequest)
        {
            if (rentRequest == null || rentRequest.User_ID < 0)
                return BadRequest("Invalid request.");

            if (rentRequest.Rent_date == DateTime.MinValue || rentRequest.Return_date == DateTime.MinValue)
                return BadRequest("Invalid dates provided.");

            var rent = new Rent
            {
                User_ID = new GUIDConverter(rentRequest.User_ID).ConvertToGuid(),
                Rent_date = rentRequest.Rent_date,
                Return_date = rentRequest.Return_date,
                Status = "reserved",  
                Company_ID = rentRequest.Company_ID,
                Offer_ID = rentRequest.Offer_ID,
            };

       
            _context.History.Add(rent);
            await _context.SaveChangesAsync();

            return Ok("Car reserved successfully!");
        }
        public class RentRequestDto
        {
            public int Rent_ID { get; set; }
            public int Car_ID { get; set; }
            public DateTime Rent_date { get; set; }
            public DateTime Return_date { get; set; }
            public int User_ID { get; set; }
            public int Company_ID { get; set; }
            public int Offer_ID { get; set; }
        }
        public class GUIDConverter
        {
            public int myInt {  get; set; }
            public GUIDConverter(int myInt)
            {
                this.myInt = myInt;
            }
            public Guid ConvertToGuid()
            {
                byte[] byteArray = new byte[16];  // A GUID is 16 bytes long

                // Fill the byte array with the int value
                byte[] intBytes = BitConverter.GetBytes(myInt);
                Array.Copy(intBytes, byteArray, intBytes.Length);

                // Generate a GUID by filling the rest of the bytes with random values (or use a default value)
                return new Guid(byteArray);
            }
        }
    }
}
