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
using car_rent.Server.DTOs;
using System.Text.Json.Nodes;
using Microsoft.VisualBasic;
using car_rent.Server.Notifications;
using car_rent.Server.DataProvider;


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
        private readonly List<ICarRentalDataProvider> _carRentalProviders;

        public CarController(HttpClient httpClient, string car_rent_company_api1, UserManager<ApplicationUser> userManager,
            SearchEngineDbContext context, INotificationService notificationService, IEnumerable<ICarRentalDataProvider> carRentalProviders)
        {
            _httpClient = httpClient;
            _apiUrl = car_rent_company_api1;
            _userManager = userManager;
            _context = context;
            _notificationService = notificationService;
            _carRentalProviders = carRentalProviders.ToList();
        }




        [HttpGet(Name = "GetCars")]
        public async Task<ActionResult<IEnumerable<OfferFromApi>>> Get(DateTime startDate, DateTime endDate, string search_brand = "", string search_model = "")
        {
            var user = await _userManager.GetUserAsync(User);
            string clientId = (user == null ? "" : user.Id.ToString());
            string email = (user == null ? "" : user.Email);

            List<OfferFromApi> offers = _carRentalProviders.SelectMany(provider => provider.GetOfferToDisplays(startDate, endDate, search_brand, search_model, clientId, email).Result).ToList();

            return offers;
        }

        [HttpGet("getdetails/{offerId:guid}")]
        public async Task<ActionResult<CarDetailsToDisplay>> Get(string offerId)
        {
            var user = await _userManager.GetUserAsync(User);
            string clientId = user?.Id.ToString() ?? string.Empty;

            var requestUrl = $"{_apiUrl}/api/offer/id/{offerId}";
            try
            {
                var responseContent = await _httpClient.GetStringAsync(requestUrl);
                var carDetails = JsonSerializer.Deserialize<CarDetailsToDisplay>(responseContent);

                if (carDetails != null)
                {
                    carDetails.Car.Picture = $"{_apiUrl}/{carDetails.Car.Picture}";
                    return Ok(carDetails);
                }

                return NotFound("Car details not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("sendEmail/{offerIdPlusProvider}")]
        public async Task<ActionResult<string>> SendEmail(string offerIdPlusProvider)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("Offer not found");
            }

            string url = $"{Request.Scheme}://{Request.Host}";

            // Build the confirmation link
            string confirmationLink = $"{url}/Car/confirmationLink/{offerIdPlusProvider}";

            await Console.Out.WriteLineAsync(confirmationLink);

            OfferFromApi? offer = null;
            foreach (var provider in _carRentalProviders)
            {
                if (provider.CheckIfMyOffer(offerIdPlusProvider))
                {
                    string offerId = provider.RemoveProviderName(offerIdPlusProvider);
                    offer = provider.GetOneOfferFromApi(offerId).Result;
                    break;
                }
            }
            if (offer == null)
            {
                return NotFound("Offer not found");
            }
            _notificationService.Notify(offer, confirmationLink, user);


            return Ok("Confirmation email sent");
        }

        [Authorize]
        [HttpGet("confirmationLink/{offerIdPlusProvider}")]
        public async Task<ActionResult> ConfirmationLink(string offerIdPlusProvider)
        {
            // Get the authenticated user
            var user = await _userManager.GetUserAsync(User);
            string clientId = user.Id.ToString();

            RentInfoFromApi? rent = null;
            string offerId = string.Empty;
            string companyName = string.Empty;
            foreach (var provider in _carRentalProviders)
            {
                if (provider.CheckIfMyOffer(offerIdPlusProvider))
                {
                    offerId = provider.RemoveProviderName(offerIdPlusProvider);
                    companyName = provider.GetProviderName();
                    rent = provider.RentCar(offerId, user, clientId).Result;
                    break;
                }
            }
            if (rent == null)
            {
                return NotFound("Rent not found");
            }

            Company company = _context.Companies.FirstOrDefault(c=> c.Name==companyName);
            await AddRentToDb(rent, offerId, user, company);

            return Redirect("/successfulRent");
        }



        private async Task AddRentToDb(RentInfoFromApi rentInfoFromApi, string offerId, ApplicationUser user, Company company)
        {
            var newRent = new Rent
            {
                Rent_ID = company.Company_ID.ToString() + rentInfoFromApi.RentId,
                RentId_in_company = rentInfoFromApi.RentId,
                Rent_date = rentInfoFromApi.StartDate,
                Return_date = rentInfoFromApi.EndDate,
                User = user,
                Status = RentStatus.Reserved,
                Company = company,
                Offer_ID = offerId
            };

            var newOffer = new Offer()
            {
                Id = offerId,
                Price = rentInfoFromApi.Price,
                Car = new Car(rentInfoFromApi.CarBrand, rentInfoFromApi.CarModel, rentInfoFromApi.CarYear, string.Empty),
                Rent = newRent,
                ClientId = user.Id.ToString(),
                StartDate = rentInfoFromApi.StartDate,
                EndDate = rentInfoFromApi.EndDate
            };

            _context.Offers.Add(newOffer);
            _context.History.Add(newRent);

            _context.SaveChanges();
        }

    }
}
