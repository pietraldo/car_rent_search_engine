/* eslint-disable react/prop-types */
import Button from 'react-bootstrap/Button';
import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import '../Style/Element.css';


function Element({ car, apiUrl}) {
    const [buttonText, setButtonText] = useState("Rent me!");
    const navigate = useNavigate();

    const handleClick = () => {
        setButtonText((prevText) => (prevText !== "Rented!" ? "Rented!" : "Rent me!"));

        async function sendEmail() {
            const response = await fetch(`Car/sendEmail/${car.offerId}`);
            if (response.ok)
                alert("Email sent! Please confirm your rent");
            const link = await response.text();
            console.log(link);
        }
        sendEmail();
    };
    const showDetails = () => { alert("The details will be shown..."); }
    return (
        <div className="carContainer" onClick={() => navigate(`/car_details/${car.offerId}`)}>
            <img
                src={`${car.picture}`}
                alt={car.model}
                className="carImage"
            />
            <div style={{ flex: 1 }}>
                <h2 className="carTitle">{car.model} </h2>
                <p className="carDescription">
                    Brand: {car.brand} <br />
                    Color: {car.color }  <br />
                    Year: {car.year } <br />
                </p>
            </div>
            <div className="rentInfo">
                <p className="carPrice">Price: ${ car.price}</p> {/* Replace with actual price */}
                <Button className="rentButton" onClick={handleClick}>
                    {buttonText}
                </Button>
            </div>
        </div>
    );
}

export default Element;
