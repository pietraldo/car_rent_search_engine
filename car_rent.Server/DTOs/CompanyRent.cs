using car_rent.Server.Model;
using System.Text.Json.Serialization;

namespace car_rent.Server.DTOs;

public class CompanyRent
{
    [JsonPropertyName("start")]
    public DateTime Start { get; set; }

    [JsonPropertyName("end")]
    public DateTime End { get; set; }

    [JsonPropertyName("carBrand")]
    public string CarBrand { get; set; }

    [JsonPropertyName("carModel")]
    public string CarModel { get; set; }

    [JsonPropertyName("carYear")]
    public int CarYear { get; set; }

    [JsonPropertyName("price")]
    public float Price { get; set; }
}