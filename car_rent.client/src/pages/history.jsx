import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import "../Style/history.css";
import Alert from "../components/Alert";

const History = () => {
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

            const response = await fetch('/api2/Rentals/rents');

            if (!response.ok) {
                throw new Error("Failed to fetch history data");
            }

            const data = await response.json();
            console.log(data);
            setRentalHistory(data);
        } catch (err) {
            console.error('Error fetching history:', err);
            setError("Failed to fetch rental history. Please try again later.");
        } finally {
            setLoading(false);
        }
    };

    const returnOffer = (rentId) => async () =>
    {
        console.log("returning...");
        console.log(`/api2/Rentals/return/${rentId}`)
        const response = await fetch(`/api2/Rentals/return/${rentId}`);
        Alert("green", "Car returned succesfully!");
        fetchHistory();
    }

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
        <div className="container">
        <h1 className="text-center">Rental History</h1>
            <table className="table table-bordered text-center">
                <thead className="bg-success text-white">
                    <tr>
                        <th>Rent Date</th>
                        <th>Return Date</th>
                        <th>Price</th>
                        <th>Brand</th>
                        <th>Model</th>
                        <th>Car year</th>
                        <th>Status</th>
                        <th>Company</th>
                    </tr>
                </thead>
                <tbody>
                    {rentalHistory.map((rental) => (
                        <tr key={rental.rent_ID}> 
                            <td>{new Date(rental.rent_date).toLocaleDateString()}</td>
                            <td>{new Date(rental.return_date).toLocaleDateString()}</td>
                            <td>{rental.offer.price.toString() || "N/A"}</td>
                            <td>{rental.offer.car.brand.toString() || "N/A"}</td>
                            <td>{rental.offer.car.model.toString() || "N/A"}</td>
                            <td>{rental.offer.car.year.toString() || "N/A"}</td>
                            <td>
                                {rental.status !== 'Returned' ? (
                                    <button className="btn btn-return" onClick={returnOffer(rental.rentId_in_company) }>Return</button>
                                ) : (
                                    rental.status.toString()
                                )}
                            </td>
                            <td>{rental.company.name.toString() || "N/A"}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
};

export default History;
