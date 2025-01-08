using car_rent_api2.Server.Database;
using car_rent.Server.Model;
using RestSharp;

namespace car_rent.Server.Notifications;

public class MailGunService : INotificationService, IEmailService
{
    private readonly MailGunClient _client;
    private readonly HtmlMessageGenerator _htmlMessageGenerator;

    public MailGunService()
    {
        var apiKey = Environment.GetEnvironmentVariable("MAILGUN_API_KEY");
        _client = new MailGunClient(apiKey);
        _htmlMessageGenerator = new HtmlMessageGenerator();
    }

    public RestResponse SendEmail(string email, string subject, string message)
    {
        return _client.SendEmail(email, subject, message);
    }

    public void Notify(OfferToDisplay offer, string confirmationLink, ApplicationUser user)
    {
        var email = user.Email;
        var subject = "[Car Rent] Confirm Your Order";
        var message = _htmlMessageGenerator.GenerateOfferMessage(offer, confirmationLink);
        SendEmail(email, subject, message);
    }
}