using car_rent_api2.Server.Database;
using car_rent.Server.Model;

namespace car_rent.Server.Notifications;

public interface INotificationService
{
    public void Notify(OfferToDisplay offer, string confirmationLink, ApplicationUser user);
}