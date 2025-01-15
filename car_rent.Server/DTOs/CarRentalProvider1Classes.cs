using System.Text.Json.Serialization;

namespace car_rent.Server.DTOs
{
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
    public class NewRentParametersDto
    {
        public string OfferId { get; set; }
        public string Email { get; set; }
    }
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
}
