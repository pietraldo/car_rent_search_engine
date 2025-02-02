﻿namespace car_rent.Server.DataProvider
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
    using Microsoft.AspNetCore.Mvc;
    using car_rent_api2.Server.Database;
    using System.Net;
    using System.Text.Json.Serialization;

   
    public class CarRentalDataProvider2 : CarRentalDataProvider
    {
        private readonly HttpClient _httpClient;
        private readonly string _accessToken;
        private readonly string _apiUrl2;

        public CarRentalDataProvider2(HttpClient httpClient, string[] apiUrls)
        {
            _httpClient = httpClient;
            _apiUrl2 = apiUrls[0];
        }

        public override string GetProviderName()
        {
            return "DriveEasy";
        }

        public override async Task<RentInfoFromApi?> RentCar(string offerId, ApplicationUser user, string clientId)
        {
            bool result = await CheckIfClientExistsInApi(clientId, user);
            if (!result) return null;

            // Proceed with the rent car operation
            var rentCarResponse = await _httpClient.GetAsync($"{_apiUrl2}/api/Offer/rentCar/{offerId}/{clientId}");
            if (!rentCarResponse.IsSuccessStatusCode)
            {
                return null;
            }

            var rentCarResponseContent = await rentCarResponse.Content.ReadAsStringAsync();
            var rent = JsonSerializer.Deserialize<RentInfoFromApi>(rentCarResponseContent);
            return rent;
        }

        public override async Task<bool> ReturnCar(string rentId)
        {
            var rentCarResponse = await _httpClient.GetAsync($"{_apiUrl2}/api/Rent/readyToReturn/{rentId}");
            return rentCarResponse.IsSuccessStatusCode;
        }

        public override async Task<OfferFromApi?> GetOneOfferFromApi(string offerId)
        {
            var offerResponse = await _httpClient.GetAsync($"{_apiUrl2}/api/Offer/id/{offerId}");
            if (!offerResponse.IsSuccessStatusCode)
            {
                return null;
            }
            var json = await offerResponse.Content.ReadAsStreamAsync();
            var jsonString = await offerResponse.Content.ReadAsStringAsync();
            var offer = await JsonSerializer.DeserializeAsync<OfferFromApi>(json);
            return offer;
        }

        public override async Task<List<OfferFromApi>> GetOfferToDisplays(DateTime startDate, DateTime endDate, string search_brand, string search_model, string clientId, string email)
        {
            var requestUrl = $"{_apiUrl2}/api/offer?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}&brand={search_brand}&model={search_model}&clientId={clientId}";

            try
            {
                var responseContent = await _httpClient.GetStringAsync(requestUrl);
                var offerFromApi = JsonSerializer.Deserialize<OfferFromApi[]>(responseContent);

                foreach (var offer in offerFromApi)
                {
                    offer.IdPlusProvider = AddProviderName(offer.IdPlusProvider);
                    offer.Car.Photo = $"{_apiUrl2}/{offer.Car.Photo}";
                }

                return offerFromApi != null ? offerFromApi.ToList() : new List<OfferFromApi>();
            }
            catch (Exception ex)
            {
                return new List<OfferFromApi>();
            }
        }

        public override async Task<CarDetailsToDisplay> GetCarDetailsToDisplay(string offerId)
        {
            var requestUrl = $"{_apiUrl2}/api/offer/id/{offerId}";
            try
            {
                var responseContent = await _httpClient.GetStringAsync(requestUrl);
                var carDetails = JsonSerializer.Deserialize<CarDetailsToDisplay>(responseContent);

                if (carDetails != null)
                {
                    carDetails.Car.Picture = $"{_apiUrl2}/{carDetails.Car.Picture}";
                    return carDetails;
                }

                return new CarDetailsToDisplay();
            }
            catch (Exception ex)
            {
                return new CarDetailsToDisplay();
            }
        }

        private async Task<bool> CheckIfClientExistsInApi(string clientId, ApplicationUser user)
        {
            // Check if the user exists in the external API
            var checkClientResponse = await _httpClient.GetAsync($"{_apiUrl2}/api/Client/checkClient/{clientId}");
            if (checkClientResponse.StatusCode == HttpStatusCode.NotFound)
            {
                // Prepare and send user information to the external API
                string name = user.UserName ?? "";
                string surname = user.LastName ?? "";
                var userInformation = new { Id = clientId, Name = name, Surname = surname, Email = user.Email };
                var userInformationJson = JsonSerializer.Serialize(userInformation);
                var content = new StringContent(userInformationJson, Encoding.UTF8, "application/json");

                var createClientResponse = await _httpClient.PostAsync($"{_apiUrl2}/api/Client/createClient", content);
                if (!createClientResponse.IsSuccessStatusCode)
                {
                    return false;
                }
            }
            else if (!checkClientResponse.IsSuccessStatusCode)
            {
                return false;
            }
            return true;
        }


    }

}
