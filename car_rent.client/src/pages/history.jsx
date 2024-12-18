import { useEffect, useState } from 'react';
import "../Style/history.css";

const History = () => {
    const [rentalHistory, setRentalHistory] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    async function getHistory() {
        try {
            const response = await fetch('https://localhost:7029/Rentals');
            console.log(response);
            if (!response.ok) {
                throw new Error("Failed to fetch history data");
            }
            const data = await response.json();
            setRentalHistory(data);
        } catch (error) {
            console.log('Error fetching history: ', error);
            setError("Failed to fetch rental history. Please try again later.");
        }
    }

    useEffect(() => {
        fetchHistory();
    }, []);

    const fetchHistory = async () => {
        setLoading(true);
        await getHistory();
        setLoading(false);
    };

    if (loading) return <p>Loading rental history...</p>;
    if (error) return <p className="text-danger">{error}</p>;

    // If no history, show a message
    if (rentalHistory.length === 0) {
        return <p className="text-center">No rental history found.</p>;
    }

    return (
        <div>
            <table className="table table-bordered text-center">
                <thead className="bg-success text-white">
                    <tr>
                        <th>Rent ID</th>
                        <th>Rent Date</th>
                        <th>Return Date</th>
                        <th>Year</th>
                        <th>Company</th>
                        <th>Price</th>
                        <th>Model</th>
                        <th>Brand</th>
                        <th>Status</th>
                    </tr>
                </thead>
                <tbody>
                    {rentalHistory.map((rental) => (
                        <tr key={rental.rent_ID}>
                            <td>{new Date(rental.rent_date).toLocaleDateString()}</td>
                            <td>{new Date(rental.return_date).toLocaleDateString()}</td>
                            <td>{rental.company?.name || "N/A"}</td> {/* Assuming `company` has a `name` property */}
                            <td>{rental?.price || "N/A"}</td>
                            <td>{rental?.model || "N/A"}</td>
                            <td>{rental?.brand || "N/A"}</td>
                            <td>{rental?.year || "N/A"}</td>
                            <td>
                                {rental.status !== 'returned' ? (
                                    <button className="btn btn-return">Return</button>
                                ) : (
                                    rental.status
                                )}
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
};

export default History;
