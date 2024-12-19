import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import "../Style/rentDetails.css";

const RentDetails = () => {
    const navigate = useNavigate();
    const [rentalHistory, setRentalHistory] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [userEmail, setUserEmail] = useState(null);

    // Helper function to get the value of a cookie
    function getCookie(name) {
        const value = `; ${document.cookie}`;
        const parts = value.split(`; ${name}=`);
        if (parts.length === 2) {
            return decodeURIComponent(parts.pop().split(";").shift());
        }
        return null;
    }

    // Retrieve the user email from cookies when the component mounts
    useEffect(() => {
        const email = getCookie("UserEmail");
        if (email) {
            setUserEmail(email);
        } else {
            setError("User email not found in cookies.");
        }
    }, []);

    // Fetch rental history
    const fetchHistory = async () => {
        try {
            setLoading(true);
            if (!userEmail) {
                throw new Error("User email is not available.");
            }

            const response = await fetch('https://localhost:7029/Rentals', {
                method: 'GET',
                headers: {
                    'X-User-Email': userEmail, // Pass user email as a header
                },
            });

            if (!response.ok) {
                throw new Error("Failed to fetch history data");
            }

            const data = await response.json();
            setRentalHistory(data);
        } catch (err) {
            console.error('Error fetching history:', err);
            setError("Failed to fetch rental history. Please try again later.");
        } finally {
            setLoading(false);
        }
    };

    // Trigger fetch when userEmail is available
    useEffect(() => {
        if (userEmail) {
            fetchHistory();
        }
    }, [userEmail]);

    // Handle loading state
    if (loading) {
        return <p>Loading rental history...</p>;
    }

    // Handle error state
    if (error) {
        return <p className="text-danger">{error}</p>;
    }

    // Handle empty rental history
    if (rentalHistory.length === 0) {
        return <p className="text-center">No rental history found.</p>;
    }

    // Render rental history table
    return (
        <div>
            <h1>Rent details (to be developed)</h1>
        </div>
    );
};

export default RentDetails;
