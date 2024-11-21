using Microsoft.AspNetCore.Mvc;

namespace car_rent.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CarController : ControllerBase
    {
        public CarController()
        {

        }

        [HttpGet(Name = "GetCars")]
        public IEnumerable<Car> Get()
        {
            Car car1 = new("Lightning McQueen", "Race Car", 2006, "Red", "mcqueen.jpg");
            Car car2 = new("Mater", "Tow Truck", 1951, "Rusty Brown", "mater_tow_truck.jpg");
            Car car3 = new("Doc Hudson", "Hudson Hornet", 1951, "Dark Blue", "doc_hudson.jpg");
            Car car4 = new("Sally Carrera", "Porsche 911", 2002, "Blue", "sally.jpg");
            Car car5 = new("Ramone", "Chevrolet Impala", 1959, "Purple with Flames", "ramone.jpg");

            return [car1, car2, car3, car4, car5];
        }
        [HttpGet("get-cars2", Name = "GetCars2")]
        public IEnumerable<Car> Get2()
        {
            Car car1 = new("Lightning McQueen", "Race Car", 2006, "Red", "mcqueen.jpg");
            Car car2 = new("Mater", "Tow Truck", 1951, "Rusty Brown", "mater_tow_truck.jpg");
            Car car3 = new("Doc Hudson", "Hudson Hornet", 1951, "Dark Blue", "doc_hudson.jpg");

            return [car1, car2, car3];
        }
    }
}
