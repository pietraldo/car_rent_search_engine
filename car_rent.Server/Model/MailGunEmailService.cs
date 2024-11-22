using RestSharp;

namespace car_rent.Server.Model;

public class MailGunEmailService : IEmailService
{
    private readonly MailGunClient _client;

    public MailGunEmailService()
    {
        var apiKey = Environment.GetEnvironmentVariable("MAILGUN_API_KEY");
        _client = new MailGunClient(apiKey);
    }

    public RestResponse SendEmail(string email, string subject, string message)
    {
        return _client.SendEmail(email, subject, message);
    }
}