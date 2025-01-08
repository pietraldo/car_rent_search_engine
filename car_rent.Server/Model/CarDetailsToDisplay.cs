using System.Text.Json.Serialization;

namespace car_rent.Server.Model
{
    public class CarDetailsToDisplay
    {
        [JsonPropertyName("car")]
        public Car Car { get; set; }
        [JsonPropertyName("price")]
        public double? Price { get; set; }
        [JsonPropertyName("carDetails")]
        public List<CarDetail>? CarDetails { get; set; }
        [JsonPropertyName("carServices")]
        public List<CarService>? CarServices { get; set; }
        [JsonPropertyName("location")]
        public Location Location {  get; set; }     
        [JsonPropertyName("startDate")]
        public DateTime StartDate { get; set; }
        [JsonPropertyName("endDate")]
        public DateTime EndDate { get; set; }
    }

    public class CarDetail
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("value")]
        public string Value { get; set; }
    }

    public class CarService
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("price")]
        public double Price { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; } 
    }
    public class Location
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }
        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }
        [JsonPropertyName("address")]
        public string Address { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
