namespace car_rent_api2.Server.Database
{
    public class Offer
    {
        public int Offer_ID { get; set; }
        public double Price { get; set; }
        public string Brand { get; set; }
        public Rent Rent { get; set; }
    }
}
