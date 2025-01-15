import { useState } from "react";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import '../Style/BookingDatePicker.css'

const BookingDatePicker = ({ startDate, endDate, setStartDate, setEndDate }) =>
{
    

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
                        dateFormat="dd/MM/yyyy"
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
                
            </div>
        </div>
    );
};

export default BookingDatePicker;
