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
    using Microsoft.AspNetCore.Identity;
    using System.Net.Http;

    
    public class CarRentalDataProvider2 : ICarRentalDataProvider
    {
        private readonly HttpClient _httpClient;
        private readonly string _accessToken;
        private readonly string _apiUrl= "http://localhost:5173";

        public CarRentalDataProvider2(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public string GetProviderName()
        {
            return "CarRental";
        }

        public async Task<List<OfferFromApi>> GetOfferToDisplays(DateTime startDate, DateTime endDate, string search_brand, string search_model, string clientId)
        {
            var requestUrl = $"{_apiUrl}/api/offer?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}&brand={search_brand}&model={search_model}&clientId={clientId}";

            try
            {
                var responseContent = await _httpClient.GetStringAsync(requestUrl);
                var offerFromApi = JsonSerializer.Deserialize<OfferFromApi[]>(responseContent);
               
                return offerFromApi!=null ? offerFromApi.ToList():new List<OfferFromApi>();
            }
            catch (Exception ex)
            {
                return new List<OfferFromApi>();
            }
        }

        
    }

}
