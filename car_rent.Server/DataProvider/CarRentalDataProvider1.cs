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

    public class OfferForCarSearchDto
    {
        [JsonPropertyName("offerId")]
        public string OfferId { get; set; }
        [JsonPropertyName("brand")]
        public string Brand { get; set; }
        [JsonPropertyName("model")]
        public string Model { get; set; }
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        [JsonPropertyName("conditions")]
        public string Conditions { get; set; }
        [JsonPropertyName("companyName")]
        public string CompanyName { get; set; }
        [JsonPropertyName("location")]
        public string Location { get; set; }
        [JsonPropertyName("startDate")]
        public DateTime StartDate { get; set; }
        [JsonPropertyName("endDate")]
        public DateTime EndDate { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
    }

    public class CachedOfferDto
    {
        [JsonPropertyName("carId")]
        public int CarId { get; set; }
        [JsonPropertyName("brand")]
        public string Brand { get; set; }
        [JsonPropertyName("model")]
        public string Model { get; set; }
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        public string Conditions { get; set; }
        [JsonPropertyName("companyName")]
        public string CompanyName { get; set; }
        [JsonPropertyName("location")]
        public string Location { get; set; }
        [JsonPropertyName("startDate")]
        public DateTime StartDate { get; set; }
        [JsonPropertyName("endDate")]
        public DateTime EndDate { get; set; }
    }

    public class CarRentalDataProvider1 : ICarRentalDataProvider
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _accessToken;
        private readonly string _apiUrl = "https://localhost:7077";

        public CarRentalDataProvider1(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _accessToken = GenerateAccessToken();
        }

        public string GetProviderName()
        {
            return "CarRental";
        }

        public string AddProviderName(string offerId)
        {
            return offerId + "*" + GetProviderName();
        }
        public string RemoveProviderName(string offerIdPlusProvider)
        {
            return offerIdPlusProvider.Split("*")[0];
        }

        public bool CheckIfMyOffer(string offerIdPlusProvider)
        {
            return offerIdPlusProvider.Contains(GetProviderName());
        }

        public class NewRentParametersDto
        {
            public string OfferId { get; set; }
            public string Email { get; set; }
        }

        public class NewSearchRentDto
        {
            [JsonPropertyName("brand")]
            public string Brand { get; set; }
            [JsonPropertyName("model")]
            public string Model { get; set; }
            [JsonPropertyName("email")]
            public string Email { get; set; }
            [JsonPropertyName("startDate")]
            public DateTime StartDate { get; set; }
            [JsonPropertyName("endDate")]
            public DateTime EndDate { get; set; }
            [JsonPropertyName("rentalCompanyRentId")]
            public int RentalCompanyRentId { get; set; }
        }

        public async Task<bool> ReturnCar(string rentId)
        {
            var url = $"{_apiUrl}/api/Rents/set-rent-status-ready-to-return";
            var client = GetClientWithBearerToken();
            var json = JsonSerializer.Serialize(int.Parse(rentId));
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url,data);
            response.EnsureSuccessStatusCode();

            return true;
        }

        public async Task<RentInfoFromApi?> RentCar(string offerId, ApplicationUser user, string clientId)
        {
            var url = $"{_apiUrl}/api/rents/create-new-rent";
            var newRentParameters = new NewRentParametersDto() { OfferId = offerId, Email = user.Email };
            var json = JsonSerializer.Serialize(newRentParameters);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var client = GetClientWithBearerToken();
            var response = await client.PostAsync(url, data);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var new_search_rent = JsonSerializer.Deserialize<NewSearchRentDto>(responseString);

            var rent =new RentInfoFromApi() { RentId = new_search_rent.RentalCompanyRentId.ToString(), StartDate = new_search_rent.StartDate, EndDate = new_search_rent.EndDate, CarBrand = new_search_rent.Brand, CarModel = new_search_rent.Model, CarYear = -1, Price = 33542};

            return rent;
        }

        public async Task<OfferFromApi> GetOneOfferFromApi(string offerId)
        {
            var url = $"{_apiUrl}/api/offers/offer/{offerId}";
            var client = GetClientWithBearerToken();
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var offer = JsonSerializer.Deserialize<CachedOfferDto>(responseString);
            return new OfferFromApi() { IdPlusProvider = offerId, Price= (double)offer.Price, Car = new CarFromAPi() {Brand= offer.Brand, Model = offer.Model } };
        }

        public async Task<List<OfferFromApi>> GetOfferToDisplays(DateTime startDate, DateTime endDate, string search_brand, string search_model, string clientId, string email)
        {
            
            if(email.IsNullOrEmpty())
            {
                email = "emptyEmail";
            }
            var url = $"{_apiUrl}/api/offers/offer-list?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}&email={email}&brand={search_brand}&model={search_model}";
            var client = GetClientWithBearerToken();
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var offers = JsonSerializer.Deserialize<List<OfferForCarSearchDto>>(responseString);


            List<OfferFromApi> result = new List<OfferFromApi>();
            foreach(var offer in offers)
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
            var key = Encoding.UTF8.GetBytes("ded6e96cb51a701c4dc9edacb75b74bb3405a6be6637f2cb06148a396e39b15a");
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
