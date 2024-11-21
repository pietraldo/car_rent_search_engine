/* eslint-disable react/prop-types */
import './Filter.css'; // Include styles if needed

const Filter = ({ options, selectedValues, onToggle }) => (
    <div className="filter-option-container">
        {options.map((option) => (
            <div key={option} className="filter-element-wrapper">
                <input
                    className="filter-element"
                    type="checkbox"
                    value={option}
                    checked={selectedValues.includes(option)}
                    onChange={() => onToggle(option)}
                />
                <label>{option}</label>
            </div>
        ))}
    </div>
);

export default Filter;
