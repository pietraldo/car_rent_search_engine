using car_rent.Server.Model;
using System.Text.Json.Serialization;

namespace car_rent.Server.DTOs;

public class OfferFromApi
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    [JsonPropertyName("car")]
    public CarFromAPi Car { get; set; }

    [JsonPropertyName("clientId")]
    public string ClientId { get; set; }
    [JsonPropertyName("price")]
    public double Price { get; set; }
    [JsonPropertyName("startDate")]
    public DateTime StartDate { get; set; }
    [JsonPropertyName("endDate")]
    public DateTime EndDate { get; set; }
}

public class CarFromAPi
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("model")]
    public string Model { get; set; }

    [JsonPropertyName("brand")]
    public string Brand { get; set; }

    [JsonPropertyName("year")]
    public int Year { get; set; }

    [JsonPropertyName("photo")]
    public string Photo { get; set; }

    [JsonPropertyName("location")]
    public Location Location { get; set; }
}