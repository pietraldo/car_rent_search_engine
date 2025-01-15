using System.Text.Json.Serialization;

namespace car_rent.Server.DTOs
{
    public class RentInfoFromApi
    {
        [JsonPropertyName("rentId")]
        public string RentId { get; set; }
        [JsonPropertyName("start")]
        public DateTime StartDate { get; set; }
        [JsonPropertyName("end")]
        public DateTime EndDate { get; set; }

        [JsonPropertyName("carBrand")]
        public string CarBrand { get; set; }
        [JsonPropertyName("carModel")]
        public string CarModel { get; set; }
        [JsonPropertyName("carYear")]
        public int CarYear { get; set; }
        [JsonPropertyName("price")]
        public float Price { get; set; }
    }
}
