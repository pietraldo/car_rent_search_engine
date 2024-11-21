import { useNavigate } from 'react-router-dom';
import { useState, useEffect } from 'react';
import './history.css';

function History() {
    const [isLoggedIn, setIsLoggedIn] = useState(null); // Initialize as `null` to indicate loading state
    const [historyData, setHistoryData] = useState([]);
    const [errorMessage, setErrorMessage] = useState('');
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
    }, []); // Runs only once on component mount

    useEffect(() => {
        // Ensure `fetchHistory` only runs if `isLoggedIn` is determined
        if (isLoggedIn === null) {
            return; // Still loading, do nothing
        }

        if (!isLoggedIn) {
            navigate('/login');
        } else {
            fetchHistory();
        }
    }, [isLoggedIn]); // Dependency on `isLoggedIn`

    const fetchHistory = async () => {
        try {
            const response = await fetch(`http://localhost:5109/history/get-history?user_id=3`, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                },
            });

            if (response.ok) {
                const data = await response.json();
                console.log(data);
                setHistoryData(data);
            } else {
                setErrorMessage('Failed to retrieve history. Please try again.');
            }
        } catch (error) {
            console.error('Error retrieving history:', error);
            setErrorMessage('An error occurred. Please try again.');
        }
    };

    if (isLoggedIn === null) {
        // Show a loading indicator while the login status is being determined
        return <div>Loading...</div>;
    }

    return (
        <div className="history-container">
            <h1 className="history-title">Rental History</h1>
            {errorMessage && <p className="error-message">{errorMessage}</p>}
            {historyData.length === 0 ? (
                <p>No rental history found.</p>
            ) : (
                <table className="history-table">
                    <thead>
                        <tr>
                            <th>Rent ID</th>
                            <th>Rent Date</th>
                            <th>Return Date</th>
                            <th>Status</th>
                            <th>Company</th>
                            <th>Price</th>
                            <th>Model</th>
                            <th>Brand</th>
                            <th>Year</th>
                        </tr>
                    </thead>
                        <tbody>
                            {historyData.map((rent) => {
                                console.log("Rent Data:", rent); // Log each rent object
                                return (
                                    <tr key={rent.rent_ID}>
                                        <td>{rent.rent_ID}</td>
                                        <td>{new Date(rent.rent_date).toLocaleDateString()}</td>
                                        <td>{new Date(rent.return_date).toLocaleDateString()}</td>
                                        <td>{rent.status}</td>
                                        <td>{rent.company_ID }</td>
                                        <td>{rent.offer_ID}</td>
                                        <td>{rent.model}</td>
                                        <td>{rent.brand}</td>
                                        <td>{rent.year}</td>
                                    </tr>
                                );
                            })}
                        </tbody>
                </table>
            )}
        </div>
    );
}

export default History;
