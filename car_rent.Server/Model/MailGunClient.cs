using RestSharp;
using RestSharp.Authenticators;

namespace car_rent.Server.Model;

public class MailGunClient(string apiKey)
{
    public RestResponse SendEmail(string email, string subject, string message)
    {
        var client = new RestClient(new Uri("https://api.mailgun.net/v3"),
            options => options.Authenticator = new HttpBasicAuthenticator("api", apiKey));

        var request = new RestRequest();
        request.AddParameter("domain", "sandboxf4a120576c474abd8326c33eb705cee8.mailgun.org", ParameterType.UrlSegment);
        request.Resource = "{domain}/messages";
        request.AddParameter("from", "Excited User <mailgun@sandboxf4a120576c474abd8326c33eb705cee8.mailgun.org>");
        request.AddParameter("to", email);
        request.AddParameter("subject", subject);
        request.AddParameter("html", message);
        request.Method = Method.Post;
        return client.Execute(request);
    }
}