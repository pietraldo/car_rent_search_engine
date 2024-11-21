namespace car_rent.Server.Database
{
    public class Company
    {
        public int Company_ID { get; set; }
        public string Name { get; set; }
        public ICollection<Rent> Rents { get; set; }
    }
}
