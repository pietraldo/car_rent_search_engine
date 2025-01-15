using car_rent_api2.Server.Database;
using car_rent.Server.DTOs;
using car_rent.Server.Model;

namespace car_rent.Server.DataProvider
{
    public abstract class CarRentalDataProvider : ICarRentalDataProvider
    {
        public abstract Task<List<OfferFromApi>> GetOfferToDisplays(DateTime startDate, DateTime endDate,
            string search_brand, string search_model,
            string clientId = "", string email = "");

        public abstract Task<OfferFromApi> GetOneOfferFromApi(string offerId);

        public abstract Task<RentInfoFromApi?> RentCar(string offerId, ApplicationUser user, string clientId);

        public abstract Task<CarDetailsToDisplay> GetCarDetailsToDisplay(string offerId);

        public abstract Task<bool> ReturnCar(string rentId);

        public abstract string GetProviderName();

        public string AddProviderName(string offerId)
        {
            return offerId + "*" + GetProviderName();
        }

        public string RemoveProviderName(string offerIdPlusProvider)
        {
            return offerIdPlusProvider.Split("*")[0];
        }

        public bool CheckIfMyOffer(string offerIdPlusProvider)
        {
            return offerIdPlusProvider.Contains(GetProviderName());
        }
    }
}
