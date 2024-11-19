using Microsoft.AspNetCore.Mvc;

public class MessageRequest
{
    public string Message { get; set; }
}

namespace car_rent_api2.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static List<string> rozmowa = new List<string>() { "Siema Julka"};

        public WeatherForecastController()
        {
        }

        [HttpPost]
        public IActionResult Add([FromBody] MessageRequest request)
        {
            if (string.IsNullOrEmpty(request.Message))
            {
                return BadRequest("Message cannot be empty.");
            }

            rozmowa.Add(request.Message);
            return Ok();  // Return a success status
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<string> Get()
        {
            return rozmowa;
        }
    }
}
