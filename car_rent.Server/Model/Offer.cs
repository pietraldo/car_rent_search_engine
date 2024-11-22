using car_rent.Server.Model;

namespace car_rent.Server.Model
{
    public class Offer
    {
        public int Offer_ID { get; set; }
        public int Car_ID { get; set; }
        public double Price { get; set; }
        public Car Car { get; set; }
        public Rent? Rent { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Client_ID { get; set; }
    }
}
