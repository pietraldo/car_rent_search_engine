/* eslint-disable react/prop-types */
import Button from 'react-bootstrap/Button';
import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import '../Style/Element.css';


function Element({ car, apiUrl}) {
    const [buttonText, setButtonText] = useState("Rent me!");
    const navigate = useNavigate();

    console.log(car);

    const handleClick = () => {

        setButtonText((prevText) => (prevText !== "Reserved!" ? "Reserved!" : "Rent me!"));

        async function sendEmail() {
            console.log(car.startDate, car.endDate, car.brand, car.price);
            const response = await fetch(`Car/sendEmail/${car.offerId}`);

            if (response.ok)
                alert("Email sent! Please confirm your rent");
            const link = await response.text();
            console.log(link);
        }
        sendEmail();
    };
     
    return (
        <div className="carContainer">
            <img
                src={car.picture || "..//..//dist//default.jpg"}
                alt={car.model}
                className="carImage"
                onError={(e) => {
                    e.target.onerror = null; // Prevent infinite loop in case the default image also fails
                    e.target.src = "..//..//dist//default.jpg"; // Set a fallback image
                }}
            />
            <div style={{ flex: 1 }}>
                <h2 className="carTitle">{car.model}</h2>
                <p className="carDescription">
                    Brand: {car.brand} <br />
                    Year: {car.year} <br />
                </p>
            </div>
            <div className="rentInfo">
                <p className="carPrice">Price: ${car.price}</p>
                <Button className="rentButton" onClick={handleClick}>
                    {buttonText}
                </Button>
                <Button
                    className="detailsButton"
                    onClick={() => navigate(`/cardetails/${car.offerId}`)}
                >
                    Show details
                </Button>
            </div>
        </div>
    );
}

export default Element;
