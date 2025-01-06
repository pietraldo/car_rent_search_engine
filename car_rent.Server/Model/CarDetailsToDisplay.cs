using System.Text.Json.Serialization;

namespace car_rent.Server.Model
{
    public class CarDetailsToDisplay(string? detailsDescription, string? servicesDescription, double? servicesPrice, string? locationName, string? locationAddress, string? picture)
    {
        [JsonPropertyName("detailsDescription")]
        public string? DetailsDescription { get; set; } = detailsDescription;
        [JsonPropertyName("servicesDescription")]
        public string? ServicesDescription { get; set; } = servicesDescription;
        [JsonPropertyName("servicesPrice")]
        public double? ServicesPrice { get; set; } = servicesPrice;
        [JsonPropertyName("locationName")]
        public string? LocationName { get; set; } = locationName;
        [JsonPropertyName("locationAddress")]
        public string? LocationAddress { get; set; } = locationAddress;

        [JsonPropertyName("picture")]
        public string? Picture { get; set; } = picture;
    }
}
