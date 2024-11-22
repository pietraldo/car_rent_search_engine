import Button from 'react-bootstrap/Button';
import { useNavigate } from 'react-router-dom';
import { useState, useEffect } from 'react';
import './Element.css';

function Element({ car, apiUrl }) {
    const [buttonText, setButtonText] = useState("Rent me!");
    const [isLoggedIn, setIsLoggedIn] = useState(false);
    const navigate = useNavigate();

    useEffect(() => {
        const updateLoginStatus = () => {
            const loggedInStatus = localStorage.getItem("isLoggedIn") === "true";
            setIsLoggedIn(loggedInStatus); 
        };

        updateLoginStatus();

        window.addEventListener("storage", updateLoginStatus);

        return () => {
            window.removeEventListener("storage", updateLoginStatus);
        };
    }, []);

    const handleClick = async () => {
        if (!isLoggedIn) {
            navigate('/login');
        } else {
            //try {
            //    const userId = 1;
            //    const rentDate = new Date();
            //    const returnDate = new Date(rentDate);
            //    returnDate.setDate(rentDate.getDate() + 3);

            //    const response = await fetch('http://localhost:5109/rent/reserve', {
            //        method: 'POST',
            //        headers: {
            //            'Content-Type': 'application/json',
            //        },
            //        body: JSON.stringify({
            //            user_id: 1,
            //            rent_date: rentDate.toISOString(),
            //            return_date: returnDate.toISOString(),
            //            status: 'reserved',
            //            company_id: 1,
            //            offer_id: 2,
            //        }),
            //    });

            //    console.log(response);
            //    if (response.ok) {
            //        const result = await response.text();
            //        alert(result); // Show success message
            //        setButtonText("Rented!");
            //    } else {
            //        alert('Failed to reserve the car. Please try again.');
            //    }
            //} catch (error) {
            //    console.error('Error reserving car:', error);
            //    alert('An error occurred. Please try again.');
            //}
            async function sendEmail() {
                const response = await fetch(`Car/sendEmail/${car.offerId}`);
                if (response.ok) {
                    alert("Email sent! Please confirm your rent");
                    setButtonText("rented!");
                    isLoggedIn("true");
                }
                const link = await response.text();
                console.log(link);
            }
            sendEmail();
        }
    };


    return (
        <div className="carContainer">
            <img

                src={`${car.picture}`}
                alt={car.model}
                className="carImage"
            />
            <div style={{ flex: 1 }}>
                <h2 className="carTitle">{car.Model}</h2>
                <p className="carDescription">
                    Brand: {car.Brand} <br />
                    Color: {car.Color} <br />
                    Year: {car.Year} <br />
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
