namespace car_rent.Server.Model
{
    public class Car(string model, string brand, int year, string color, string picture)
    {
        public int Car_ID { get; set; }
        public string Model { get; set; } = model;
        public string Brand { get; set; } = brand;
        public int Year { get; set; } = year;
        public string? Color { get; set; } = color;

        public string GetCarInfo()
        {
            return $"Model: {Model}, Brand: {Brand}, Year: {Year}, Color: {Color}";
        }

        public string? Picture { get; set; } = picture;
        public ICollection<Offer> Offers { get; set; }
    }
}
