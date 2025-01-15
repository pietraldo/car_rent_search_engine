namespace car_rent.Server.DataProvider
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Net.Http.Headers;
    using System.Security.Claims;
    using System.Text;
    using Microsoft.AspNetCore.WebUtilities;
    using Microsoft.IdentityModel.Tokens;
    using JsonSerializer = System.Text.Json.JsonSerializer;
    using car_rent.Server.Model;
    using car_rent.Server.DTOs;
    using System.Text.Json.Serialization;
    using car_rent_api2.Server.Database;
    using System.Net.Http;
    using car_rent.Server.Migrations;



    public class CarRentalDataProvider1 : CarRentalDataProvider
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _accessToken;
        private readonly string _apiUrl1;

        public CarRentalDataProvider1(IHttpClientFactory httpClientFactory, string[] apiUrls)
        {
            _httpClientFactory = httpClientFactory;
            _accessToken = GenerateAccessToken();
            _apiUrl1 = apiUrls[1];
        }

        public override string GetProviderName()
        {
            return "CarRental";
        }
        
        public override async Task<bool> ReturnCar(string rentId)
        {
            var url = $"{_apiUrl1}/api/Rents/set-rent-status-ready-to-return";
            var client = GetClientWithBearerToken();
            var json = JsonSerializer.Serialize(int.Parse(rentId));
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, data);
            response.EnsureSuccessStatusCode();

            return true;
        }

        public override async Task<RentInfoFromApi?> RentCar(string offerId, ApplicationUser user, string clientId)
        {
            var url = $"{_apiUrl1}/api/rents/create-new-rent";
            var newRentParameters = new NewRentParametersDto() { OfferId = offerId, Email = user.Email };
            var json = JsonSerializer.Serialize(newRentParameters);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var client = GetClientWithBearerToken();
            var response = await client.PostAsync(url, data);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var new_search_rent = JsonSerializer.Deserialize<NewSearchRentDto>(responseString);

            var rent = new RentInfoFromApi() { RentId = new_search_rent.RentalCompanyRentId.ToString(), StartDate = new_search_rent.StartDate, EndDate = new_search_rent.EndDate, CarBrand = new_search_rent.Brand, CarModel = new_search_rent.Model, CarYear = -1, Price = 33542 };

            return rent;
        }

        public override async Task<CarDetailsToDisplay> GetCarDetailsToDisplay(string offerId)
        {
            OfferFromApi offer = await GetOneOfferFromApi(offerId);
            Car car = new Car(offer.Car.Brand,offer.Car.Model, -1,  "");

            List<CarDetail> carDetails = new List<CarDetail>();
            List<CarService> carServices = new List<CarService>();

            return new CarDetailsToDisplay() { Car=car, Price=offer.Price, CarDetails=carDetails, CarServices=carServices, Location=offer.Car.Location, StartDate=offer.StartDate, EndDate=offer.EndDate };
        }

        public override async Task<OfferFromApi> GetOneOfferFromApi(string offerId)
        {
            var url = $"{_apiUrl1}/api/offers/offer/{offerId}";
            var client = GetClientWithBearerToken();
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var offer = JsonSerializer.Deserialize<CachedOfferDto>(responseString);
            return new OfferFromApi() { IdPlusProvider = offerId, Price = (double)offer.Price, Car = new CarFromAPi() { Brand = offer.Brand, Model = offer.Model } };
        }

        public override async Task<List<OfferFromApi>> GetOfferToDisplays(DateTime startDate, DateTime endDate, string search_brand, string search_model, string clientId, string email)
        {

            if (email.IsNullOrEmpty())
            {
                email = "emptyEmail";
            }
            var url = $"{_apiUrl1}/api/offers/offer-list?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}&email={email}&brand={search_brand}&model={search_model}";
            var client = GetClientWithBearerToken();
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var offers = JsonSerializer.Deserialize<List<OfferForCarSearchDto>>(responseString);


            List<OfferFromApi> result = new List<OfferFromApi>();
            foreach (var offer in offers)
            {
                OfferFromApi offerFromApi = new OfferFromApi();
                offerFromApi.IdPlusProvider = AddProviderName(offer.OfferId);
                offerFromApi.Car = new CarFromAPi();
                offerFromApi.Car.Brand = offer.Brand;
                offerFromApi.Car.Model = offer.Model;
                offerFromApi.Car.Year = -1;
                offerFromApi.Car.Photo = "";
                offerFromApi.Car.Location = new Location();
                offerFromApi.Car.Location.Name = offer.Location;
                offerFromApi.ClientId = clientId;
                offerFromApi.Price = (double)offer.Price;
                offerFromApi.StartDate = offer.StartDate;
                offerFromApi.EndDate = offer.EndDate;
                result.Add(offerFromApi);
            }

            return result;
        }



        private HttpClient GetClientWithBearerToken()
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
            return client;
        }

        private string GenerateAccessToken()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("CAR_RENTAL_SECRET_KEY"));
            List<Claim> claims = new List<Claim>();
            Claim backendClaim = new Claim("Backend", "1");
            claims.Add(backendClaim);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Expires = DateTime.UtcNow.AddMinutes(60)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }


    }

}
