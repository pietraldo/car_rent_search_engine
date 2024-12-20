using System.Text.Json.Serialization;

namespace car_rent_api2.Server.Database
{
    public class Rent
    {
        public Guid Rent_ID { get; set; }
        public int RentId_in_company { get; set; }
        public DateTime Rent_date { get; set; }
        public DateTime Return_date { get; set; }
        public Guid User_ID { get; set; }
        public string Status { get; set; }
        public Guid Company_ID { get; set; }
        public Guid Offer_ID { get; set; }
        // Navigation properties
        [JsonIgnore]
        public ApplicationUser User { get; set; }
        public Offer Offer { get; set; }
        public Company Company { get; set; }

    }
}
