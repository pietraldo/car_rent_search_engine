using car_rent.Server;
using car_rent.Server.Model;
using System.Text.Json.Serialization;

namespace car_rent_api2.Server.Database
{
    public class Offer
    {
        public Guid Id { get; set; }
        public Car Car { get; set; }
        public string ClientId { get; set; }
        public double Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [JsonIgnore]
        public Rent? Rent { get; set; }
    }
}
