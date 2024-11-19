namespace car_rent_api2.Server.Database
{
    public class Rent
    {
        public int Rent_ID { get; set; }
        public DateTime Rent_date { get; set; }
        public DateTime Return_date { get; set; }
        public int User_ID { get; set; }
        public string Status { get; set; }
        public int Company_ID { get; set; }
        public int Offer_ID { get; set; }
        // Navigation properties
        public User User { get; set; }
        public Offer Offer { get; set; }
        public Company Company { get; set; }

    }
}
