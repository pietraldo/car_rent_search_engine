import { useState, useEffect } from 'react';
import { useParams, useLocation } from 'react-router-dom';
import '../Style/CarDetails.css'; // Import the CSS file

const CarDetails = () => {
    const { carId } = useParams(); // Get carId from URL parameters
    const location = useLocation(); // Get location to extract query params
    const urlParams = new URLSearchParams(location.search);
    const picture = urlParams.get('picture'); // Extract picture from query params

    const [carDetails, setCarDetails] = useState([]);
    const [services, setServices] = useState([]);
    const [locationData, setLocationData] = useState(null);
    const [photo, setPhoto] = useState(picture); // Set initial photo from the query param
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchCarData = async () => {
            try {
                const detailsResponse = await fetch(`https://localhost:7083/api/Car/cardetails/${carId}`);
                const servicesResponse = await fetch(`https://localhost:7083/api/Car/carservices/${carId}`);
                const locationResponse = await fetch(`https://localhost:7083/api/Car/location/${carId}`);

                if (!detailsResponse.ok || !servicesResponse.ok || !locationResponse.ok) {
                    throw new Error('Failed to fetch car data');
                }

                const carDetailsData = await detailsResponse.json();
                const servicesData = await servicesResponse.json();
                const locationData = await locationResponse.json();

                setCarDetails(carDetailsData);
                setServices(servicesData);
                setLocationData(locationData);

            } catch (err) {
                setError(err.message);
            } finally {
                setLoading(false);
            }
        };

        fetchCarData();
    }, [carId, photo]); // Re-run the effect if carId or photo changes

    if (loading) return <p>Loading car details...</p>;
    if (error) return <p>Error: {error}</p>;

    return (
        <div className="car-details-container">
            {/* Car Photo Section */}
            <div className="car-photo">
                {photo ? (
                    <img src={photo} alt="Car" className="car-photo-img" />
                ) : (
                    <p>Loading photo...</p>
                )}
            </div>

            {/* Content Section */}
            <div className="car-info">

                {/* Specifications Section */}
                <div className="section">
                    <h2 className="subtitle">Specifications</h2>
                    {carDetails.length > 0 ? (
                        <ul className="list">
                            {carDetails.map((detail) => (
                                <li key={detail.id} className="list-item">
                                    <strong>{detail.description}:</strong> {detail.value || 'N/A'}
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
                    {services.length > 0 ? (
                        <ul className="list">
                            {services.map((service) => (
                                <li key={service.id} className="list-item">
                                    <strong>{service.name || 'Unnamed Service'}:</strong> {service.description || 'No description available'} (Price: ${service.price})
                                </li>
                            ))}
                        </ul>
                    ) : (
                        <p>No services available.</p>
                    )}
                </div>

                {/* Location Section */}
                <div className="section">
                    <h2 className="subtitle">Location</h2>
                    {locationData ? (
                        <div className="location">
                            <p><strong>Garage Name:</strong> {locationData.name || 'N/A'}</p>
                            <p><strong>Address:</strong> {locationData.address}</p>
                            <p><strong>Coordinates:</strong> {locationData.latitude}, {locationData.longitude}</p>
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

