/* eslint-disable react/prop-types */
import { useState } from "react";
import "../Style/CollapsibleSectionGeneric.css" 

const CollapsibleSectionGeneric = ({ title, children, className }) => {
    const [isVisible, setIsVisible] = useState(false);

    const toggleVisibility = () => {
        setIsVisible((prev) => !prev);
    };

    return (
        <div className={`collapsible-section ${className}`}>
            <button className="toggle-button" onClick={toggleVisibility}>
                {isVisible ? `Hide ${title}` : `Show ${title}`}
            </button>

            {isVisible && <div className="section-content">{children}</div>}
        </div>
    );
};

export default CollapsibleSectionGeneric;
