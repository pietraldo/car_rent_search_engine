import { useState } from 'react';
import { pl } from 'date-fns/locale';
import DatePicker from "react-datepicker";
import './reserve.css';

const Reserve = () => {
    const [startDate, setStartDate] = useState(new Date());
    const [isOpen, setIsOpen] = useState(false); // State to manage dropdown visibility

    const toggleDatePicker = () => {
        setIsOpen(prev => !prev); // Toggle the visibility
    };

    return (
        //<div className="reserve-container">
        //    <button onClick={toggleDatePicker} className="date-picker-button">
        //        {startDate.toLocaleDateString('pl-PL')} {/* Display the selected date */}
        //    </button>
        //    {isOpen && ( // Render DatePicker only when isOpen is true
                <div className="date-picker-dropdown">
                    <DatePicker
                        locale={pl}
                        selected={startDate}
                        onChange={(date) => {
                            setStartDate(date);
                            setIsOpen(false); // Close the date picker when a date is selected
                        }}
                    />
                </div>
            //)}
        //</div>
    );
};

export default Reserve;