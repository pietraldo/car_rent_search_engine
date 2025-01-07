import { useState, useEffect } from 'react';
import { useLocation } from 'react-router-dom';
import '../Style/CarDetails.css'; // Import the CSS file
import LocationMap from '../components/LocationMap';
import Button from 'react-bootstrap/Button';

// Utility function to format dates
const formatDate = (dateString) => {
    if (!dateString) return "N/A";
    const date = new Date(dateString);
    const options = { year: 'numeric', month: 'long', day: 'numeric' }; // Example: January 7, 2025
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
    const [isRented, setIsRented] = useState(false); // Track rental status

    const handleServiceSelection = (event, serviceId) => {
        if (event.target.checked) {
            // Add service ID to the selected list
            setSelectedServices((prevSelected) => [...prevSelected, serviceId]);
        } else {
            // Remove service ID from the selected list
            setSelectedServices((prevSelected) =>
                prevSelected.filter((id) => id !== serviceId)
            );
        }
    };

    const handleClick = () => {
        if (!isRented) {
            setIsRented(true); // Set as rented
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
                const response = await fetch(`/Car/${offerId}`);
                if (!response.ok) {
                    throw new Error('Failed to fetch car data');
                }
                const carDetailsData = await response.json();
                setCarDetails(carDetailsData);
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
                <img src={carDetails.picture} alt="Car" className="car-photo-img" />
                <Button className="rentButton2" onClick={handleClick}>
                    {buttonText}
                </Button>
            </div>

            {/* Content Section */}
            <div className="car-info">
                {/* Specifications Section */}
                <div className="section">
                    <h2 className="subtitle">Basic info</h2>
                    <ul className="list">
                        <li className="list-item">
                            <strong>Brand:</strong> {carDetails.brand} <br />
                            <strong>Model:</strong> {carDetails.model} <br />
                            <strong>Year:</strong> {carDetails.year} <br />
                            <strong>Price:</strong> {carDetails.price}$
                        </li>
                    </ul>
                </div>
                <div className="section">
                    <h2 className="subtitle">Specifications</h2>
                    {carDetails.details && carDetails.details.length > 0 ? (
                        <ul className="list">
                            {carDetails.details.map((detail) => (
                                <li className="list-item" key={detail.id}>
                                    <strong>{detail.description}</strong> {detail.value ? `: ${detail.value}` : ''}
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
                    {carDetails.services && carDetails.services.length > 0 ? (
                        <ul className="list">
                            {carDetails.services.map((service) => (
                                <li className="list-item" key={service.id}>
                                    <label>
                                        <input
                                            type="checkbox"
                                            onChange={(e) => handleServiceSelection(e, service.id)}
                                        />
                                        <strong>{service.name} </strong> {`(${service.description})`} {service.price}$
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
                    {carDetails.locationName ? (
                        <div className="location">
                            <p><strong>Garage Name:</strong> {carDetails.locationName}</p>
                            <p><strong>Address:</strong> {carDetails.locationAddress}</p>
                            <LocationMap lat={carDetails.latitude} lon={carDetails.longitude} />
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
