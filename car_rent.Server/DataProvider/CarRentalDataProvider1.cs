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

        public async Task<List<OfferFromApi>> GetOfferToDisplays(DateTime startDate, DateTime endDate, string search_brand, string search_model, string clientId)
        {
            if(clientId.IsNullOrEmpty())
            {
                clientId = "ad123";
            }
            var url = $"{_apiUrl}/api/offers/offer-list?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}&email={clientId}&brand={search_brand}&model={search_model}";
            var client = GetClientWithBearerToken();
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var offers = JsonSerializer.Deserialize<List<OfferForCarSearchDto>>(responseString);

            List<OfferFromApi> result = new List<OfferFromApi>();
            foreach(var offer in offers)
            {
                OfferFromApi offerFromApi = new OfferFromApi();
                offerFromApi.Id = offer.OfferId;
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


        public async Task<List<Car>> GetCars()
        {
            //var url = "https://localhost:7077/api/Cars/car-list";
            //var client = GetClientWithBearerToken();
            //var response = await client.GetAsync(url);
            //response.EnsureSuccessStatusCode();
            //var responseString = await response.Content.ReadAsStringAsync();
            //var cars = JsonSerializer.Deserialize<List<Car>>(responseString);
            //return cars;
            Car car2 = new Car("BMW", "X5", 2019, "bmw.jpg");
            return new List<Car> { car2 };
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
