using RestSharp;

namespace car_rent.Server.Model;

public interface IEmailService
{
    RestResponse SendEmail(string email, string subject, string message);
}