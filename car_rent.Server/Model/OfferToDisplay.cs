using System.Text.Json.Serialization;

namespace car_rent.Server.Model
{
    public class OfferToDisplay
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("car")]
        public CarToDisplay Car { get; set; }

        [JsonPropertyName("clientId")]
        public string ClientId { get; set; }

        [JsonPropertyName("price")]
        public double Price { get; set; }

        [JsonPropertyName("startDate")]
        public DateTime StartDate { get; set; }

        [JsonPropertyName("endDate")]
        public DateTime EndDate { get; set; }
    }
}
