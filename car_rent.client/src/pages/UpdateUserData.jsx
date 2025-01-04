import { useState } from "react";
import { useNavigate } from "react-router-dom";
import "../Style/filldata.css";

function UpdateUserData() {
    const [firstName, setFirstName] = useState("");
    const [lastName, setLastName] = useState("");
    const [city, setCity] = useState("");
    const [country, setCountry] = useState("");
    const [houseNumber, setHouseNumber] = useState("");
    const [drivingLicenseIssueDate, setDrivingLicenseIssueDate] = useState("");
    const [dateOfBirth, setDateOfBirth] = useState("");
    const [error, setError] = useState("");
    const navigate = useNavigate();

    const handleChange = (e) => {
        const { name, value } = e.target;
        switch (name) {
            case "firstName":
                setFirstName(value);
                break;
            case "lastName":
                setLastName(value);
                break;
            case "city":
                setCity(value);
                break;
            case "country":
                setCountry(value);
                break;
            case "houseNumber":
                setHouseNumber(value);
                break;
            case "drivingLicenseIssueDate":
                setDrivingLicenseIssueDate(value);
                break;
            case "dateOfBirth":
                setDateOfBirth(value);
                break;
            default:
                break;
        }
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        if (!firstName || !lastName || !city || !country || !houseNumber || !drivingLicenseIssueDate || !dateOfBirth) {
            setError("Please fill in all fields.");
        } else {
            setError("");
            fetch("/api/User/updateUserData", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({
                    firstName,
                    lastName,
                    city,
                    country,
                    houseNumber,
                    drivingLicenseIssueDate,
                    dateOfBirth,
                }),
            })
                .then((response) => {
                    if (response.ok) {
                        setError("Data submitted successfully.");
                        navigate("/");
                    } else {
                        setError("Error submitting data.");
                    }
                })
                .catch((error) => {
                    console.error(error);
                    setError("Error submitting data.");
                });
        }
    };

    return (
        <div className="containerbox">
            <h3>Fill Data</h3>
            <form onSubmit={handleSubmit}>
                <div>
                    <label htmlFor="firstName">First Name:</label>
                    <input
                        type="text"
                        id="firstName"
                        name="firstName"
                        value={firstName}
                        onChange={handleChange}
                    />
                </div>
                <div>
                    <label htmlFor="lastName">Last Name:</label>
                    <input
                        type="text"
                        id="lastName"
                        name="lastName"
                        value={lastName}
                        onChange={handleChange}
                    />
                </div>
                <div>
                    <label htmlFor="city">City:</label>
                    <input
                        type="text"
                        id="city"
                        name="city"
                        value={city}
                        onChange={handleChange}
                    />
                </div>
                <div>
                    <label htmlFor="country">Country:</label>
                    <input
                        type="text"
                        id="country"
                        name="country"
                        value={country}
                        onChange={handleChange}
                    />
                </div>
                <div>
                    <label htmlFor="houseNumber">House Number:</label>
                    <input
                        type="text"
                        id="houseNumber"
                        name="houseNumber"
                        value={houseNumber}
                        onChange={handleChange}
                    />
                </div>
                <div>
                    <label htmlFor="drivingLicenseIssueDate">Driving License Issue Date:</label>
                    <input
                        type="date"
                        id="drivingLicenseIssueDate"
                        name="drivingLicenseIssueDate"
                        value={drivingLicenseIssueDate}
                        onChange={handleChange}
                    />
                </div>
                <div>
                    <label htmlFor="dateOfBirth">Date of Birth:</label>
                    <input
                        type="date"
                        id="dateOfBirth"
                        name="dateOfBirth"
                        value={dateOfBirth}
                        onChange={handleChange}
                    />
                </div>
                <div>
                    <button type="submit" className="submitButton">Change Data</button>
                </div>
            </form>
            {error && <p className="error">{error}</p>}
        </div>
    );
}

export default UpdateUserData;