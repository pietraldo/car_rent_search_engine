import React, { useState } from 'react';
import '../Style/SearchBar.css'

const SearchBar = () => {

    const [query, setQuery] = useState("")

    function search(e) {
        e.preventDefault()
        setQuery(e.target.value)
    }

    return (
        <div className="searchContainer">
            <input
                type="text"
                className="searchInput"
                placeholder="Search"
                onChange={search}
                value={query}
            />
            <button className="searchButton" />
        </div>
    );
};

export default SearchBar;
