using System.Text.Json.Serialization;

namespace car_rent.Server.Model;

public class CarToDisplay(string brand, string model, int year, string picture)
{
    [JsonPropertyName("model")]
    public string Model { get; set; } = model;
    [JsonPropertyName("brand")]
    public string Brand { get; set; } = brand;
    [JsonPropertyName("year")]
    public int Year { get; set; } = year;

    [JsonPropertyName("picture")]
    public string Picture { get; set; } = picture;
}