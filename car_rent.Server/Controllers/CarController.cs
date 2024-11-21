using car_rent_api2.Server.Database;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;

namespace car_rent.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CarController : ControllerBase
    {

        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = "https://localhost:7083";  // External API URL
        public CarController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet(Name = "GetCars")]
        public async Task<ActionResult<IEnumerable<Car>>> Get(DateTime startDate, DateTime endDate, string search_brand = "", string search_model = "")
        {
            int clientId = 0;
            var requestUrl = $"{_apiUrl}/api/offer?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}&brand={search_brand}&model={search_model}&clientId={clientId}";

            try
            {
                // Make the HTTP GET request
                var responseContent = await _httpClient.GetStringAsync(requestUrl);


                List<Car> cars = new List<Car>();

                using (JsonDocument doc = JsonDocument.Parse(responseContent))
                {
                    // Assuming the JSON structure is an array of offers
                    var offersArray = doc.RootElement.EnumerateArray();

                    foreach (var offer in offersArray)
                    {
                        var car = offer.GetProperty("car");
                        var model = car.GetProperty("model").GetString();
                        var brand = car.GetProperty("brand").GetString();
                        var year = car.GetProperty("year").GetInt32();
                        var color = car.GetProperty("details")[0].GetProperty("value").GetString();  // Assume first detail is color
                        var picture = _apiUrl+"/"+car.GetProperty("photo").GetString();

                        // Create a new Car object and add it to the list
                        Car carObj = new (model, brand, year, color, picture);
                        cars.Add(carObj);
                    }
                }

                return Ok(cars);
            }
            catch (Exception ex)
            {
                // Handle any errors during the HTTP request
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("get-cars2", Name = "GetCars2")]
        public IEnumerable<Car> Get2()
        {
            Car car1 = new("Lightning McQueen", "Race Car", 2006, "Red", "mcqueen.jpg");
            Car car2 = new("Mater", "Tow Truck", 1951, "Rusty Brown", "mater_tow_truck.jpg");
            Car car3 = new("Doc Hudson", "Hudson Hornet", 1951, "Dark Blue", "doc_hudson.jpg");

            return [car1, car2, car3];
        }
    }
}
