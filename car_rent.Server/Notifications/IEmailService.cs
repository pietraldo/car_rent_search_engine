using RestSharp;

namespace car_rent.Server.Notifications;

public interface IEmailService
{
    RestResponse SendEmail(string email, string subject, string message);
}