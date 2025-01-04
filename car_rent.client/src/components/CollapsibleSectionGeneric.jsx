/* eslint-disable react/prop-types */
import "../Style/CollapsibleSectionGeneric.css";

const CollapsibleSectionGeneric = ({ title, children, className, isOpen, toggle }) => {
    return (
        <div className={`collapsible-section ${className}`}>
            <button className="toggle-button" onClick={toggle}>
                {isOpen ? `Hide ${title}` : `Show ${title}`}
            </button>

            {isOpen && <div className="section-content">{children}</div>}
        </div>
    );
};

export default CollapsibleSectionGeneric;
