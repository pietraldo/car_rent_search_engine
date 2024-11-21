namespace car_rent.Server.Database
{
    public class Offer
    {
        public int Offer_ID { get; set; }
        public double Price { get; set; }
        public Car Car;
        public Rent? Rent { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Client_ID { get; set; }
    }
}
