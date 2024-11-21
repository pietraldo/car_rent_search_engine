import { useState } from "react";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import './BookingDatePicker.css'

const BookingDatePicker = () => {
    const [startDate, setStartDate] = useState(null);
    const [endDate, setEndDate] = useState(null);

    return (
        <div className="booking-date-picker">
            <div className="date-picker-dropdown">
                <div className="date-picker-container">
                    <label className="label">Start Date:</label>
                    <DatePicker
                        selected={startDate}
                        onChange={(date) => setStartDate(date)}
                        selectsStart
                        startDate={startDate}
                        endDate={endDate}
                        minDate={new Date()}
                        placeholderText="Select start date"
                        className="custom-date-picker"
                    />
                </div>

                <div className="date-picker-container">
                    <label className="label">End Date:</label>
                    <DatePicker
                        selected={endDate}
                        onChange={(date) => setEndDate(date)}
                        selectsEnd
                        startDate={startDate}
                        endDate={endDate}
                        minDate={startDate || new Date()}
                        placeholderText="Select end date"
                        className="custom-date-picker"
                    />
                </div>
                <button
                    className="booking-button"
                    disabled={!startDate || !endDate}
                    onClick={() =>
                        alert(`Booking from ${startDate.toLocaleDateString()} to ${endDate.toLocaleDateString()}`)
                    }
                >
                    Reserve Now
                </button>
            </div>
        </div>
    );
};

export default BookingDatePicker;
