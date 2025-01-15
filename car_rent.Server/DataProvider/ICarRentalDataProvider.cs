using car_rent.Server.DTOs;
using car_rent.Server.Model;
using car_rent_api2.Server.Database;

namespace car_rent.Server.DataProvider
{
    public interface ICarRentalDataProvider
    {
        Task<List<OfferFromApi>> GetOfferToDisplays(DateTime startDate, DateTime endDate, string search_brand, string search_model, string clientId="", string email="");
        Task<OfferFromApi> GetOneOfferFromApi(string offerId);
        Task<RentInfoFromApi?> RentCar(string offerId, ApplicationUser user, string clientId);
        Task<bool> ReturnCar(string rentId);

        string GetProviderName();
        string AddProviderName(string offerId);
        string RemoveProviderName(string offerIdPlusProvider);
        bool CheckIfMyOffer(string offerIdPlusProvider);
    }
}
