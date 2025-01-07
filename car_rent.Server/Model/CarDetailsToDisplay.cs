using System.Text.Json.Serialization;

namespace car_rent.Server.Model
{
    public class CarDetailsToDisplay(string? brand, string? model, int? year, double? price, List<CarDetail>? details, 
        List<CarService>? services, string? locationName, string? locationAddress, double latitude, double longitude, string? picture,
        DateTime startDate, DateTime endDate)
    {
        [JsonPropertyName("brand")]
        public string? Brand { get; set; } = brand;
        [JsonPropertyName("model")]
        public string? Model { get; set; } = model;
        [JsonPropertyName("year")]
        public int? Year { get; set; } = year;
        [JsonPropertyName("price")]
        public double? Price {  get; set; } = price;
        [JsonPropertyName("details")]
        public List<CarDetail>? Details { get; set; } = details;
        [JsonPropertyName("services")]
        public List<CarService>? Services { get; set; } = services;
        
        [JsonPropertyName("locationName")]
        public string? LocationName { get; set; } = locationName;
        [JsonPropertyName("locationAddress")]
        public string? LocationAddress { get; set; } = locationAddress;
        [JsonPropertyName("latitude")]
        public double Latitude { get; set; } = latitude;
        [JsonPropertyName("longitude")]
        public double Longitude { get; set; } = longitude;

        [JsonPropertyName("picture")]
        public string? Picture { get; set; } = picture;
        [JsonPropertyName("startDate")]
        public DateTime StartDate { get; set; } = startDate;

        [JsonPropertyName("endDate")]
        public DateTime EndDate { get; set; } = endDate;

    }
}
