using car_rent.Server.DTOs;
using car_rent.Server.Model;

namespace car_rent.Server.DataProvider
{
    public interface ICarRentalDataProvider
    {
        Task<List<OfferFromApi>> GetOfferToDisplays(DateTime startDate, DateTime endDate, string search_brand, string search_model, string clientId);
    }
}
