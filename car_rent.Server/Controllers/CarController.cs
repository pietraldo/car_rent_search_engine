using car_rent_api2.Server.Database;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using car_rent.Server.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using System.Text;
using System.ComponentModel.Design;
using car_rent.Server.Migrations;
using System.Text.Json.Serialization;
using car_rent.Server.Notifications;

namespace car_rent.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CarController : ControllerBase
    {

        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SearchEngineDbContext _context;
        private readonly INotificationService _notificationService;

        public CarController(HttpClient httpClient, string car_rent_company_api1, UserManager<ApplicationUser> userManager, SearchEngineDbContext context, INotificationService notificationService)
        {
            _httpClient = httpClient;
            _apiUrl = car_rent_company_api1;
            _userManager = userManager;
            _context = context;
            _notificationService = notificationService;
        }

        [HttpGet(Name = "GetCars")]
        public async Task<ActionResult<IEnumerable<OfferToDisplay>>> Get(DateTime startDate, DateTime endDate, string search_brand = "", string search_model = "")
        {
            var user = await _userManager.GetUserAsync(User);
            string clientId = (user == null ? "" : user.Id.ToString());

            var requestUrl = $"{_apiUrl}/api/offer?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}&brand={search_brand}&model={search_model}&clientId={clientId}";

            try
            {
                var responseContent = await _httpClient.GetStringAsync(requestUrl);


                List<OfferToDisplay> offersToDisplay = new List<OfferToDisplay>();

                var jsonArray = JsonSerializer.Deserialize<JsonElement[]>(responseContent);

                foreach (var offer in jsonArray)
                {
                    var car = offer.GetProperty("car");
                    var model = car.GetProperty("model").GetString();
                    var brand = car.GetProperty("brand").GetString();
                    var year = car.GetProperty("year").GetInt32();
                    var picture = _apiUrl + "/" + car.GetProperty("photo").GetString();


                    CarToDisplay carObj = new CarToDisplay(brand, model, year, picture);

                    // Create a new OfferToDisplay object
                    OfferToDisplay offerToDisplay = new OfferToDisplay
                    {
                        Id = Guid.Parse(offer.GetProperty("id").GetString()),
                        Car = carObj,
                        ClientId = offer.GetProperty("clientId").GetString(),
                        Price = offer.GetProperty("price").GetDouble(),
                        StartDate = DateTime.Parse(offer.GetProperty("startDate").GetString()),
                        EndDate = DateTime.Parse(offer.GetProperty("endDate").GetString())
                    };

                    offersToDisplay.Add(offerToDisplay);
                }




                return Ok(offersToDisplay);
            }
            catch (Exception ex)
            {
                // Handle any errors during the HTTP request
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [Authorize]
        [HttpGet("sendEmail/{offerId}")]
        public async Task<ActionResult<string>> SendEmail(string offerId)
        {
            //TODO: if user not logged in return failure

            //TODO: Implement sending an email

            string url = $"{Request.Scheme}://{Request.Host}";

            // Build the confirmation link
            string confirmationLink = $"{url}/Car/confirmationLink/{offerId}";
            
            var offerResponse = await _httpClient.GetAsync($"{_apiUrl}/api/Offer/id/{offerId}");
            if (!offerResponse.IsSuccessStatusCode)
            {
                return StatusCode(500, "Error getting offer from external API");
            }
            
            var json = await offerResponse.Content.ReadAsStreamAsync();
            var jsonString = await offerResponse.Content.ReadAsStringAsync();
            var offer = await JsonSerializer.DeserializeAsync<OfferToDisplay>(json);

            var user = await _userManager.GetUserAsync(User);
            
            _notificationService.Notify(offer, confirmationLink, user);

            return Ok("Confirmation email sent");
        }

        [Authorize]
        [HttpGet("confirmationLink/{offerId}")]
        public async Task<ActionResult> ConfirmationLink(string offerId)
        {
            // Get the authenticated user
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized("User not found");
            }
            string clientId = user.Id.ToString();

            // Check if the user exists in the external API
            var checkClientResponse = await _httpClient.GetAsync($"{_apiUrl}/api/Client/checkClient/{clientId}");
            if (checkClientResponse.StatusCode == HttpStatusCode.NotFound)
            {
                // Prepare and send user information to the external API
                string name = user.UserName ?? "";
                string surname = user.LastName ?? "";
                var userInformation = new { Id = clientId, Name = name, Surname = surname, Email = user.Email };
                var userInformationJson = JsonSerializer.Serialize(userInformation);
                var content = new StringContent(userInformationJson, Encoding.UTF8, "application/json");

                var createClientResponse = await _httpClient.PostAsync($"{_apiUrl}/api/Client/createClient", content);
                if (!createClientResponse.IsSuccessStatusCode)
                {
                    return StatusCode((int)createClientResponse.StatusCode, "Failed to create client in external API");
                }
            }
            else if (!checkClientResponse.IsSuccessStatusCode)
            {
                return StatusCode((int)checkClientResponse.StatusCode, "Error checking client in external API");
            }

            // Proceed with the rent car operation
            var rentCarResponse = await _httpClient.GetAsync($"{_apiUrl}/api/Offer/rentCar/{offerId}/{clientId}");
            if (!rentCarResponse.IsSuccessStatusCode)
            {
                return StatusCode((int)rentCarResponse.StatusCode, "Error renting car in external API");
            }

            var rentCarResponseContent = await rentCarResponse.Content.ReadAsStringAsync();
            var rentId = JsonSerializer.Deserialize<int>(rentCarResponseContent);

            await AddRentToDb(rentId, offerId,user);



            return Redirect("/successfulRent");
        }

        private class CompanyRent
        {
            [JsonPropertyName("start")]
            public DateTime Start { get; set; }

            [JsonPropertyName("end")]
            public DateTime End { get; set; }

            [JsonPropertyName("carBrand")]
            public string CarBrand { get; set; }

            [JsonPropertyName("carModel")]
            public string CarModel { get; set; }

            [JsonPropertyName("carYear")]
            public int CarYear { get; set; }

            [JsonPropertyName("price")]
            public float Price { get; set; }
        }

        private async Task AddRentToDb(int rentId, string offerId, ApplicationUser user)
        {
            var rentCarResponse = await _httpClient.GetAsync($"{_apiUrl}/api/Rent/getrent/{rentId}");
            if (!rentCarResponse.IsSuccessStatusCode)
            {
                return;
            }
            var rentCarResponseContent = await rentCarResponse.Content.ReadAsStringAsync();
            var rent = JsonSerializer.Deserialize<CompanyRent>(rentCarResponseContent);

            var newRent = new Rent
            {
                RentId_in_company = rentId,
                Rent_date =rent.Start,
                Return_date = rent.End,
                User_ID = user.Id,
                Status = "Confirmed",
                Company_ID = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                Offer_ID = Guid.Parse(offerId)
            };

            var newOffer = new Offer()
            {
                Id = Guid.Parse(offerId),
                Price = rent.Price,
                Car = new Car(rent.CarBrand, rent.CarModel, rent.CarYear, string.Empty),
                Rent = newRent,
                ClientId = user.Id.ToString(),
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(7)
            };

            _context.Offers.Add(newOffer);
            _context.History.Add(newRent);

            _context.SaveChanges();
        }

    }
}
