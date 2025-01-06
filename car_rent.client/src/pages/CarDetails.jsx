import { useState, useEffect } from 'react';
import { useParams, useLocation } from 'react-router-dom';
import '../Style/CarDetails.css'; // Import the CSS file

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

    useEffect(() => {
        const fetchCarData = async () => {
            try {
                const response = await fetch(`/Car/${offerId}`);
               
                if (!response.ok) {
                    throw new Error('Failed to fetch car data');
                }
                const carDetailsData = await response.json();

                setCarDetails(carDetailsData);

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
                    {carDetails.detailsDescription ? (
                        <ul className="list">
                            <li className="list-item">
                                <strong>Details:</strong> {carDetails.detailsDescription}
                            </li>
                        </ul>
                    ) : (
                        <p>No car details available.</p>
                    )}
                </div>

                {/* Services Section */}
                <div className="section">
                    <h2 className="subtitle">Available Services</h2>
                    {carDetails.servicesDescription ? (
                        <ul className="list">
                            <li className="list-item">
                                <strong>Services:</strong> {carDetails.servicesDescription} (Price: ${carDetails.servicesPrice ?? 0})
                            </li>
                        </ul>
                    ) : (
                        <p>No services available.</p>
                    )}
                </div>

                {/* Location Section */}
                <div className="section">
                    <h2 className="subtitle">Location</h2>
                    {carDetails.locationName ? (
                        <div className="location">
                            <p><strong>Garage Name:</strong> {carDetails.locationName}</p>
                            <p><strong>Address:</strong> {carDetails.locationAddress}</p>
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
