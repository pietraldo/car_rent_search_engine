import { useState, useEffect } from 'react';
import { useLocation } from 'react-router-dom';
import '../Style/CarDetails.css';
import LocationMap from '../components/LocationMap';
import Button from 'react-bootstrap/Button';

// Utility function to format dates
const formatDate = (dateString) => {
    if (!dateString) return "N/A";
    const date = new Date(dateString);
    const options = { year: 'numeric', month: 'long', day: 'numeric' };
    return date.toLocaleDateString('en-US', options);
};

const CarDetails = () => {
    const location = useLocation();
    const pathParts = location.pathname.split('/');
    const offerId = pathParts[2];
    const urlParams = new URLSearchParams(location.search);
    const pictureQuery = urlParams.get('picture');
    const [carDetails, setCarDetails] = useState({});
    const [photo, setPhoto] = useState(pictureQuery || '');
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [selectedServices, setSelectedServices] = useState([]);
    const [currentPrice, setCurrentPrice] = useState(0); // State for current price
    const [isRented, setIsRented] = useState(false);

    const handleServiceSelection = (event, serviceId, servicePrice) => {
        if (event.target.checked) {
            setSelectedServices((prevSelected) => [...prevSelected, serviceId]);
            setCurrentPrice((prevPrice) => prevPrice + servicePrice);
        } else {
            setSelectedServices((prevSelected) =>
                prevSelected.filter((id) => id !== serviceId)
            );
            setCurrentPrice((prevPrice) => prevPrice - servicePrice);
        }
    };

    const handleClick = () => {
        if (!isRented) {
            setIsRented(true);
            async function sendEmail() {
                console.log(carDetails.startDate, carDetails.endDate, carDetails.brand, carDetails.price);
                const response = await fetch(`Car/sendEmail/${carDetails.offerId}`);
                if (response.ok) {
                    alert("Email sent! Please confirm your rent.");
                    const link = await response.text();
                    console.log(link);
                }
            }
            sendEmail();
        } else {
            alert("You have already rented this car.");
        }
    };

    useEffect(() => {
        const fetchCarData = async () => {
            try {
                const response = await fetch(`/Car/getdetails/${offerId}`);
                if (!response.ok) {
                    throw new Error('Failed to fetch car data');
                }
                const carDetailsData = await response.json();
                setCarDetails(carDetailsData);
                setCurrentPrice(carDetailsData.price || 0); 
                console.log(carDetailsData);
                if (carDetailsData.picture) {
                    setPhoto(carDetailsData.picture);
                }
            } catch (err) {
                setError(err.message);
            } finally {
                setLoading(false);
            }
        };
        fetchCarData();
    }, [offerId]);

    if (loading) return <p>Loading car details...</p>;
    if (error) return <p>Error: {error}</p>;

    const buttonText = isRented
        ? "Reserved!"
        : `Rent from ${formatDate(carDetails.startDate)} to ${formatDate(carDetails.endDate)}`;

    return (
        <div className="car-details-container">
            <div className="car-photo">
                <img
                    src={carDetails.car.picture || "..//..//dist//default.jpg"}
                    alt="Car"
                    className="car-photo-img"
                    onError={(e) => {
                        e.target.onerror = null;
                        e.target.src = "..//..//dist//default.jpg";

                    }}
                />
                <Button className="rentButton2" onClick={handleClick}>
                    {buttonText}
                </Button>
            </div>

            {/* Content Section */}
            <div className="car-info">
                {/* Current Price Section */}
                <div className="section">
                    <h2 className="subtitle">Total Price</h2>
                    <p><strong>Current Price:</strong> {currentPrice.toFixed(2)}$</p>
                </div>

                {/* Specifications Section */}
                <div className="section">
                    <h2 className="subtitle">Basic Info</h2>
                    <ul className="list">
                        <li className="list-item">
                            <span className="textInfo"><strong>Brand:</strong> {carDetails.car.brand} <br /> </span>
                            <span className="textInfo"><strong>Model:</strong> {carDetails.car.model} <br /> </span>
                            <span className="textInfo"><strong>Year:</strong> {carDetails.car.year} <br /> </span>
                            <span className="textInfo"><strong>Price:</strong> {carDetails.price}$ </span>
                        </li>
                    </ul>
                </div>

                {/* Car details Section */}
                <div className="section">
                    <h2 className="subtitle">Car details</h2>
                    {carDetails.carDetails && carDetails.carDetails.length > 0 ? (
                        <ul className="list">
                            {carDetails.carDetails.map((detail) => (
                                <li className="list-item" key={detail.id}>
                                    <span className="textInfo"><strong> {detail.description}</strong>
                                        { detail.value ? (
                                            `: ${detail.value}`
                                        ) : null }
                                        <br />
                                    </span>
                                </li>
                            ))}
                        </ul>
                    ) : (
                        <p>No car details available.</p>
                    )}
                </div>

                {/* Services Section */}
                <div className="section">
                    <h2 className="subtitle">Available Services</h2>
                    {carDetails.carServices && carDetails.carServices.length > 0 ? (
                        <ul className="list">
                            {carDetails.carServices.map((service) => (
                                <li className="list-item" key={service.id}>
                                    <label>
                                        <input
                                            type="checkbox"
                                            onChange={(e) => handleServiceSelection(e, service.id, service.price)}
                                        />
                                        <strong>{service.name}</strong><span className="service-description"> {`(${service.description})`}</span>
                                        <span className="service-price">
                                            {service.price}$
                                        </span>
                                    </label>
                                </li>
                            ))}
                        </ul>
                    ) : (
                        <p>No car services available.</p>
                    )}
                </div>

                
                {/* Location Section */}
                <div className="section">
                    <h2 className="subtitle">Location</h2>
                    {carDetails.location ? (
                        <div className="location">
                            <p><strong>Garage Name:</strong> {carDetails.location.name}</p>
                            <p><strong>Address:</strong> {carDetails.location.address}</p>
                            <LocationMap lat={carDetails.location.latitude} lon={carDetails.location.longitude} name={carDetails.location.address} />
                        </div>
                    ) : (
                        <p>No location information available.</p>
                    )}
                </div>
            </div>
        </div>
    );
};

export default CarDetails;
