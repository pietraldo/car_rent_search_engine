namespace car_rent_api2.Server.Database
{
    public class User
    {
        public int User_ID { get; set; }
        public string Name { get; set; }
        public string Last_name { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string House_number { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime Issue_date_driver_license { get; set; }
        public DateTime Date_of_birth { get; set; }
        // Navigation property
        public ICollection<Rent> Rents { get; set; }
    }
}
