namespace car_rent.Server
{
    public class Car(string model, string brand, int year, string picture)
    {
        public int Id { get; set; }
        public string Model { get; set; } = model;
        public string Brand { get; set; } = brand;
        public int Year { get; set; } = year;

        public string GetCarInfo()
        {
            return $"Model: {Model}, Brand: {Brand}, Year: {Year}";
        }

        public string Picture { get; set; } = picture;
    }
}
