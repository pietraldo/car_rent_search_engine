using System.Text.Json.Serialization;

namespace car_rent.Server.Model;

public class CarToDisplay(string brand, string model, int year, string picture)
{

    public string Model { get; set; } = model;
 
    public string Brand { get; set; } = brand;
   
    public int Year { get; set; } = year;

    
    public string Picture { get; set; } = picture;
}