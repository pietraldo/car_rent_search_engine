// Dropdown.js
import { useState } from 'react';

function Dropdown({ options, onSelect }) {
    const [isOpen, setIsOpen] = useState(false);
    const [selected, setSelected] = useState(options[0]); // Default to the first option

    const toggleDropdown = () => setIsOpen(!isOpen);

    const handleSelect = (option) => {
        setSelected(option);
        setIsOpen(false);
        if (onSelect) onSelect(option); // Call onSelect callback with the chosen option
    };

    return (
        <div className="dropdown">
            <button className="dropdown-toggle" onClick={toggleDropdown}>
                {selected}▼ Wybierz marke
            </button>
            {isOpen && (
                <ul className="dropdown-menu">
                    {options.map((option, index) => (
                        <li key={index} onClick={() => handleSelect(option)}>
                            <span className="dropdown-text">{option}</span>
                            <input type="checkbox" className="dropdown-checkbox" />
                        </li>
                    ))}
                </ul>

            )}
        </div>
    );
}

export default Dropdown;
