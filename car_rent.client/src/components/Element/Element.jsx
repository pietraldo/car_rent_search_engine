/* eslint-disable react/prop-types */
import Button from 'react-bootstrap/Button';
import { useState } from 'react';
import './Element.css';


function Element({ car, apiUrl }) {
    const [buttonText, setButtonText] = useState("Rent me!");
    
    const handleClick = () => {
        setButtonText((prevText) => (prevText !== "Rented!" ? "Rented!" : "Rent me!"));
    };

    return (
        <div className="carContainer">
            <img
                src={`${apiUrl}/images/${car.picture}`}
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
                <p className="carPrice">Price: $XX.XX</p> {/* Replace with actual price */}
                <Button className="rentButton" onClick={handleClick}>
                    {buttonText}
                </Button>
            </div>
        </div>
    );
}

export default Element;
