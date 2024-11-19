import { useEffect, useState } from 'react';
import './App.css';
import NavigationBar from './components/NavigationBar/NavigationBar';
import Element from './components/Element/Element';
import { BrowserRouter as Router, Routes, Route, useLocation } from 'react-router-dom';
import { CSSTransition, TransitionGroup } from 'react-transition-group';
import Login from './pages/login/login';
function App() {
    const [cars, setCars] = useState([]); // All cars fetched from the backend
    const [filteredCars, setFilteredCars] = useState([]); // Cars after filtering
    const [selectedBrands, setSelectedBrands] = useState([]); // Selected brands
    const [selectedModels, setSelectedModels] = useState([]); // Selected models
    const [selectedYears, setSelectedYears] = useState([]); // Selected years
    const [selectedColors, setSelectedColors] = useState([]); // Selected colors
    const apiUrl = import.meta.env.VITE_API_URL;
    const location = useLocation();
    const [isLoading, setIsLoading] = useState(true);
    const [isFilterVisible, setIsFilterVisible] = useState({
        brands: false,
        models: false,
        years: false,
        colors: false
    });

    const toggleFilterVisibility = (filter) => {
        setIsFilterVisible(prevState => ({
            ...prevState,
            [filter]: !prevState[filter] // Toggle visibility for the selected filter group
        }));
    };
    useEffect(() => {
        async function fetchCars() {
            setIsLoading(true);
            await getCars();
            setIsLoading(false);
        }
        fetchCars();
    }, []);

    // Fetch car data from the API
    async function getCars() {
        try {
            const response = await fetch(`${apiUrl}/Car`);
            if (!response.ok) {
                throw new Error('Failed to fetch car data.');
            }
            const data = await response.json();
            setCars(data);
            setFilteredCars(data);
        } catch (error) {
            console.error('Error fetching cars:', error);
            setCars([]);
            setFilteredCars([]);
        }
    }

    // Extract unique options for filters
    const uniqueBrands = cars.length > 0 ? [...new Set(cars.map(car => car.brand))] : [];
    const uniqueModels = cars.length > 0 ? [...new Set(cars.map(car => car.model))] : [];
    const uniqueYears = cars.length > 0 ? [...new Set(cars.map(car => car.year))] : [];
    const uniqueColors = cars.length > 0 ? [...new Set(cars.map(car => car.color))] : [];

    // Filter cars based on selected options
    useEffect(() => {
        let filtered = cars;

        if (selectedBrands.length > 0) {
            filtered = filtered.filter(car => selectedBrands.includes(car.brand));
        }

        if (selectedModels.length > 0) {
            filtered = filtered.filter(car => selectedModels.includes(car.model));
        }

        if (selectedYears.length > 0) {
            filtered = filtered.filter(car => selectedYears.includes(car.year));
        }

        if (selectedColors.length > 0) {
            filtered = filtered.filter(car => selectedColors.includes(car.color));
        }

        setFilteredCars(filtered);
    }, [selectedBrands, selectedModels, selectedYears, selectedColors, cars]);

    // Handle checkbox changes
    const handleCheckboxChange = (value, selectedValues, setSelectedValues) => {
        if (selectedValues.includes(value)) {
            setSelectedValues(selectedValues.filter(item => item !== value)); // Remove the value
        } else {
            setSelectedValues([...selectedValues, value]); // Add the value
        }
    };

    const contents = filteredCars.length === 0
        ? <p><em>No cars available. Try adjusting the filters.</em></p>
        : (
            <div>
                {filteredCars.map((car) => (
                    <Element key={car.id} car={car} apiUrl={apiUrl} />
                ))}
            </div>
        );

    return (
        <div>
            <NavigationBar />

            {/* Filters Section */}
            {location.pathname === "/" && (
                isLoading ? (
                    <p>Loading filters...</p>
                ) : (
                    <div className="filters">
                        <h2>Filter by:</h2>

                        {/* Brands Filter */}
                        <div className="filter-group">
                            <button onClick={() => toggleFilterVisibility("brands")}>
                                {isFilterVisible.brands ? "Hide Brands" : "Show Brands"}
                            </button>
                            {isFilterVisible.brands && (
                                <div className="filter-option-container">
                                    {uniqueBrands.map(brand => (
                                        <div key={brand} className="filter-element-wrapper">
                                            <input
                                                className="filter-element"
                                                type="checkbox"
                                                value={brand}
                                                checked={selectedBrands.includes(brand)}
                                                onChange={() =>
                                                    handleCheckboxChange(brand, selectedBrands, setSelectedBrands)
                                                }
                                            />
                                            <label>{brand}</label>
                                        </div>
                                        ))}
                                </div>
                            )}
                        </div>

                        {/* Models Filter */}
                        <div className="filter-group">
                            <button onClick={() => toggleFilterVisibility("models")}>
                                {isFilterVisible.models ? "Hide Models" : "Show Models"}
                            </button>
                            {isFilterVisible.models && (
                                <div className="filter-option-container">
                                    {uniqueModels.map(model => (
                                        <div key={model} className="filter-element-wrapper">
                                            <input
                                                className="filter-element"
                                                type="checkbox"
                                                value={model}
                                                checked={selectedModels.includes(model)}
                                                onChange={() => handleCheckboxChange(model, selectedModels, setSelectedModels)}
                                            />
                                            <label> {model} </label>
                                        </div>
                                    ))}
                                </div>
                                )}
                        </div>
                        {/* Years Filter */}
                        <div className="filter-group">
                            <button onClick={() => toggleFilterVisibility("years")}>
                                {isFilterVisible.years ? "Hide Years" : "Show Years"}
                            </button>
                            {isFilterVisible.years && (
                                <div className="filter-option-container">
                                    {uniqueYears.map(year => (
                                        <div key={year} className="filter-element-wrapper">
                                            <input
                                                className="filter-element"
                                                type="checkbox"
                                                value={year}
                                                checked={selectedYears.includes(year)}
                                                onChange={() => handleCheckboxChange(year, selectedYears, setSelectedYears)}
                                            />
                                            <label> {year} </label>
                                        </div>
                                    ))}
                                </div>
                            )}
                        </div>

                        {/* Colors Filter */}
                        <div className="filter-group">
                            <button onClick={() => toggleFilterVisibility("colors")}>
                                {isFilterVisible.colors ? "Hide Colors" : "Show Colors"}
                            </button>
                            {isFilterVisible.colors && (
                                <div className="filter-option-container">
                                    {uniqueColors.map(color => (
                                        <div key={color} className="filter-element-wrapper">
                                            <input
                                                className="filter-element"
                                                type="checkbox"
                                                value={color}
                                                checked={selectedColors.includes(color)}
                                                onChange={() => handleCheckboxChange(color, selectedColors, setSelectedColors)}
                                            />
                                            <label> {color} </label>
                                        </div>
                                    ))}
                                </div>
                            )}
                        </div>
                    </div>
                )
            )}

            <TransitionGroup>
                <CSSTransition key={location.key} classNames="fade" timeout={300}>
                    <Routes location={location}>
                        <Route path="/login" element={<Login />} />
                        <Route path="/" element={<div>{contents}</div>} />
                    </Routes>
                </CSSTransition>
            </TransitionGroup>
        </div>
    );
}

// Wrap your App with Router in your index.js or main file
export default function AppWrapper() {
    return (
        <Router>
            <App />
        </Router>
    );
}
