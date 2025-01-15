using car_rent.Server.Model;
using car_rent.Server.DTOs;
namespace car_rent.Server.Notifications;


public class HtmlMessageGenerator : IMessageGenerator
{
    public string GenerateOfferMessage(OfferFromApi offer, string confirmationLink)
    {
        return $@"<html>
<body>
<h1>Car Rent - Confirm Your Order</h1>
<p>Hello,</p>
<p>Thank you for your order. Please confirm your order by clicking the button below:</p>
<a href=""{confirmationLink}"" style=""padding: 16px; background-color: #4CAF50; color: white; text-align: center; text-decoration: none; display: inline-block; font-size: 16px;"">
Confirm Order
</a>
<p>Order details:</p>
<ul>
<li>Car: {offer.Car.Brand} {offer.Car.Model}</li>
<li>Year: {offer.Car.Year}</li>
<li>Price: {offer.Price}</li> 
</ul>
</body>
</html>";
    }
}

public interface IMessageGenerator
{
    public string GenerateOfferMessage(OfferFromApi offer, string confirmationLink);
}