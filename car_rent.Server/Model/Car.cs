namespace car_rent.Server.Model;

public class Car(string brand, string model, int year, string picture)
{
    public Guid Id { get; set; }
    public string Model { get; set; } = model;
    public string Brand { get; set; } = brand;
    public int Year { get; set; } = year;
    public string Picture { get; set; } = picture;
    
    public string GetCarInfo()
    {
        return $"Model: {Model}, Brand: {Brand}, Year: {Year}";
    }
}