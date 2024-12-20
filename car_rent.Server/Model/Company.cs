namespace car_rent_api2.Server.Database
{
    public class Company
    {
        public Guid Company_ID { get; set; }
        public string Name { get; set; }
        public Rent Rent { get; set; }
    }
}
