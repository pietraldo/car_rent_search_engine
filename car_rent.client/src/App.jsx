import { useEffect, useState } from 'react';
import './App.css';
import NavigationBar from './components/NavigationBar/NavigationBar';
import Element from './components/Element/Element';
import { BrowserRouter as Router, Routes, Route, useLocation } from 'react-router-dom';
import { CSSTransition, TransitionGroup } from 'react-transition-group';
import Login from './pages/login/login';
import Reserve from './pages/reserve/reserve';

function App() {
    const [cars, setCars] = useState([]); // All cars fetched from the backend
    const [filteredCars, setFilteredCars] = useState([]); // Cars after filtering
    const [selectedBrand, setSelectedBrand] = useState(""); // Selected brand
    const [selectedModel, setSelectedModel] = useState(""); // Selected model
    const [selectedYear, setSelectedYear] = useState(""); // Selected year
    const apiUrl = import.meta.env.VITE_API_URL;
    const location = useLocation(); // Required for CSSTransition

    const [isLoading, setIsLoading] = useState(true);

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
            setCars([]); // Ensure state is consistent even if the fetch fails
            setFilteredCars([]);
        }
    }

    // Extract unique options for dropdowns
    const uniqueBrands = cars.length > 0 ? [...new Set(cars.map(car => car.brand))] : [];
    const uniqueModels = cars.length > 0
        ? selectedBrand
            ? [...new Set(cars.filter(car => car.brand === selectedBrand).map(car => car.model))]
            : [...new Set(cars.map(car => car.model))]
        : [];
    const uniqueYears = cars.length > 0
        ? selectedBrand && selectedModel
            ? [...new Set(cars.filter(car => car.brand === selectedBrand && car.model === selectedModel).map(car => car.year))]
            : [...new Set(cars.map(car => car.year))]
        : [];

    // Filter cars based on selected options
    useEffect(() => {
        let filtered = cars;

        if (selectedBrand) {
            filtered = filtered.filter(car => car.brand === selectedBrand);
        }

        if (selectedModel) {
            filtered = filtered.filter(car => car.model === selectedModel);
        }

        if (selectedYear) {
            filtered = filtered.filter(car => car.year === selectedYear);
        }

        setFilteredCars(filtered);
    }, [selectedBrand, selectedModel, selectedYear, cars]);

    // Handle dropdown selections
    const handleBrandSelect = (brand) => {
        setSelectedBrand(brand);
        setSelectedModel(""); // Reset model selection when brand changes
        setSelectedYear(""); // Reset year selection when brand changes
    };

    const handleModelSelect = (model) => {
        setSelectedModel(model);
        setSelectedYear(""); // Reset year selection when model changes
    };

    const handleYearSelect = (year) => {
        setSelectedYear(year);
    };
    console.log('Unique Brands:', uniqueBrands);
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
            {isLoading ? (
                <p>Loading filters...</p>
            ) : (
                <div className="filters">
                    <h2>Filter by:</h2>
                    <select value={selectedBrand} onChange={(e) => handleBrandSelect(e.target.value)}>
                        <option value="">All Brands</option>
                        {uniqueBrands.map(brand => (
                            <option key={brand} value={brand}>{brand}</option>
                        ))}
                    </select>
                    <select value={selectedModel} onChange={(e) => handleModelSelect(e.target.value)}>
                        <option value="">All Models</option>
                        {uniqueModels.map(model => (
                            <option key={model} value={model}>{model}</option>
                        ))}
                    </select>
                    <select value={selectedYear} onChange={(e) => handleYearSelect(e.target.value)}>
                        <option value="">All Years</option>
                        {uniqueYears.map(year => (
                            <option key={year} value={year}>{year}</option>
                        ))}
                    </select>
                </div>
            )}

            {/* Cars Display */}
            {contents}

            <TransitionGroup>
                <CSSTransition key={location.key} classNames="fade" timeout={300}>
                    <Routes location={location}>
                        <Route path="/login" element={<Login />} />
                        <Route path="/" element={<div>{contents}</div>} />
                        <Route path="/reserve" element={<Reserve />} />
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
