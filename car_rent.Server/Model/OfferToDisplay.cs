using System.Text.Json.Serialization;

namespace car_rent.Server.Model
{
    public class OfferToDisplay
    {

        public Guid Id { get; set; }


        public CarToDisplay Car { get; set; }


        public string ClientId { get; set; }


        public double Price { get; set; }


        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
