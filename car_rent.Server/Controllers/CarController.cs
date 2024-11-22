using car_rent.Server.Model;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;

namespace car_rent.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CarController : ControllerBase
    {

        private readonly SearchEngineDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl; 
                                                                    
        public CarController(HttpClient httpClient, string car_rent_company_api1, SearchEngineDbContext context)
        {
            _httpClient = httpClient;
            _apiUrl = car_rent_company_api1;
            _context = context;

        }

        [HttpGet(Name = "GetCars")]
        public async Task<ActionResult<IEnumerable<OfferToDisplay>>> Get(DateTime startDate, DateTime endDate, string search_brand = "", string search_model = "")
        {
            int clientId = 0;
            var requestUrl = $"{_apiUrl}/api/offer?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}&brand={search_brand}&model={search_model}&clientId={clientId}";

            try
            {
                // Make the HTTP GET request
                var responseContent = await _httpClient.GetStringAsync(requestUrl);


                List<OfferToDisplay> offersToDisplay = new List<OfferToDisplay>();

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

                        OfferToDisplay offerToDisplay = new OfferToDisplay
                        {
                            Id = Guid.Parse(offer.GetProperty("id").GetString()),
                            Car = carObj,
                            ClientId = offer.GetProperty("clientId").GetInt32(),
                            Price = offer.GetProperty("price").GetDouble(),
                            StartDate = DateTime.Parse(offer.GetProperty("startDate").GetString()),
                            EndDate = DateTime.Parse(offer.GetProperty("endDate").GetString())
                        };
                        offersToDisplay.Add(offerToDisplay);
                    }
                }

                return Ok(offersToDisplay);
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

        [HttpGet("sendEmail/{offerId}")]
        public async Task<ActionResult<string>> SendEmail(string offerId)
        {
            //TODO: if user not logged in return failure

            //TODO: Implement sending an email

            string url = $"{Request.Scheme}://{Request.Host}";

            // Build the confirmation link
            string confirmationLink = $"{url}/Car/confirmationLink/{offerId}";

            Console.WriteLine(confirmationLink);
            return Ok(confirmationLink);
        }

        [HttpGet("confirmationLink/{offerId}")]
        public async Task<ActionResult> ConfirmationLink(string offerId)
        {
            // TODO: Get user id

            var response=await _httpClient.GetAsync($"{_apiUrl}/api/Offer/rentcar/{offerId}");
            return Ok(response);
        }

    }
}
