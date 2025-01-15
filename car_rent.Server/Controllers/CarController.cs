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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SearchEngineDbContext _context;
        private readonly INotificationService _notificationService;
        private readonly List<ICarRentalDataProvider> _carRentalProviders;

        public CarController(HttpClient httpClient, UserManager<ApplicationUser> userManager,
            SearchEngineDbContext context, INotificationService notificationService, IEnumerable<ICarRentalDataProvider> carRentalProviders)
        {
            _httpClient = httpClient;
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

            var offerTasks = _carRentalProviders.Select(provider =>
            {
                return Task.Run(async () =>
                {
                    try
                    {
                        return await provider.GetOfferToDisplays(startDate, endDate, search_brand, search_model, clientId, email);
                    }
                    catch
                    {
                        // Log or handle individual task failure here
                        return Enumerable.Empty<OfferFromApi>();
                    }
                });
            });

            // Run all tasks and allow up to 5 seconds for completion
            var timeoutTask = Task.Delay(5000);
            var allTasks = Task.WhenAll(offerTasks);

            // Wait for either all tasks to complete or timeout
            var completedTask = await Task.WhenAny(allTasks, timeoutTask);

            // Gather results from successfully completed tasks
            if (completedTask == allTasks)
            {
                // If all tasks completed within 5 seconds
                return allTasks.Result.SelectMany(x => x).ToList();
            }
            else
            {
                // Timeout: Gather completed results so far
                var completedOffers = new List<OfferFromApi>();
                foreach (var task in offerTasks)
                {
                    if (task.IsCompletedSuccessfully)
                    {
                        completedOffers.AddRange(await task);
                    }
                }
                return completedOffers;
            }
        }

        [HttpGet("getdetails/{offerIdPlusProvider}")]
        public async Task<ActionResult<CarDetailsToDisplay>> Get(string offerIdPlusProvider)
        {

            foreach (var provider in _carRentalProviders)
            {
                if (provider.CheckIfMyOffer(offerIdPlusProvider))
                {
                    string offerId = provider.RemoveProviderName(offerIdPlusProvider);
                    var carDetailsToDisplay = provider.GetCarDetailsToDisplay(offerId).Result;
                    return carDetailsToDisplay;
                }
            }

            return NotFound();
        }
        
        [HttpGet("sendEmail/{offerIdPlusProvider}")]
        public async Task<ActionResult<string>> SendEmail(string offerIdPlusProvider)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found");
            }

            string url = $"{Request.Scheme}://{Request.Host}";

            // Build the confirmation link
            string confirmationLink = $"{url}/Car/confirmationLink/{offerIdPlusProvider}";

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
