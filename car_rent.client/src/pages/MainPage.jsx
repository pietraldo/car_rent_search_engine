import { useEffect, useState } from 'react';
import { BrowserRouter as Router, Routes, Route, useLocation } from 'react-router-dom';
import { CSSTransition, TransitionGroup } from 'react-transition-group';
import Button from 'react-bootstrap/Button';

// Components
import Element from '../components/Element';
import BookingDatePicker from '../components/BookingDatePicker';
import Filter from '../components/Filter';
import CollapsibleSectionGeneric from '../components/CollapsibleSectionGeneric';
import CarDetails from '../pages/CarDetails';
import Box from '@mui/material/Box';
import Slider from '@mui/material/Slider';
import CircularProgress from '@mui/material/CircularProgress';

// Styles
import '../Style/App.css';

function MainPage() {
    const [cars, setCars] = useState([]);
    const [priceRange, setPriceRange] = useState([0, 10000]); // Default price range (min, max)
    const [filteredCars, setFilteredCars] = useState([]);
    const [selectedBrands, setSelectedBrands] = useState([]);
    const [selectedModels, setSelectedModels] = useState([]);
    const [selectedYears, setSelectedYears] = useState([]);
    const [selectedLocations, setSelectedLocations] = useState([]);
    const [openSection, setOpenSection] = useState(null); // Track the currently open section
    const apiUrl = import.meta.env.VITE_API_URL;
    const location = useLocation();
    const [isLoading, setIsLoading] = useState(true);
    const [currentPage, setCurrentPage] = useState(1);
    const [maxPrice, setMaxPrice] = useState(10000);
    const [loggedIn, setLoggedIn] = useState(false);
    const carsPerPage = 5;

    const [startDate, setStartDate] = useState(new Date());
    const [endDate, setEndDate] = useState(() => {
        const tomorrow = new Date();
        tomorrow.setDate(tomorrow.getDate() + 1);
        return tomorrow;
    });


    const fetchCars = async () => {
        setIsLoading(true);
        try {
            const formattedStartDate = startDate.toISOString();
            const formattedEndDate = endDate.toISOString();
            const response = await fetch(`/Car?startdate=${formattedStartDate}&enddate=${formattedEndDate}`);
            if (!response.ok) {
                throw new Error('Failed to fetch car data.');
            }
            const rawResponseText = await response.text();
            const data = JSON.parse(rawResponseText);

            
            const carsData = data.map((item) => ({
                model: item.car.model,
                year: item.car.year,
                brand: item.car.brand,
                picture: item.car.photo,
                price: item.price,
                endDate: item.endDate,
                startDate: item.startDate,
                offerId: item.id,
                location: item.car.location.name
            }));

            setCars(carsData);
            setFilteredCars(carsData);
            console.log(carsData);
            if (carsData.length > 0) {
                const newMaxPrice = Math.max(...carsData.map((car) => car.price));
                console.log(newMaxPrice);
                setMaxPrice(newMaxPrice);
                setPriceRange([0, newMaxPrice]); // Use newMaxPrice directly
            } else {
                setMaxPrice(10000);
            }

        } catch (error) {
            console.error('Error fetching cars:', error);
            setCars([]);
            setFilteredCars([]);
            setMaxPrice(10000);
        }
        setIsLoading(false);
    };

    // Helper function to get the value of a cookie
    function getCookie(name) {
        const value = `; ${document.cookie}`;
        const parts = value.split(`; ${name}=`);
        if (parts.length === 2) {
            return decodeURIComponent(parts.pop().split(";").shift());
        }
        return null;
    }
    
    useEffect(() => {
        let email = getCookie("UserEmail");
        if (email) {
            setLoggedIn(true);
        }
        fetchCars();
    }, []);

    const uniqueBrands = cars.length > 0 ? [...new Set(cars.map((car) => car.brand))] : [];
    const uniqueModels = cars.length > 0 ? [...new Set(cars.map((car) => car.model))] : [];
    const uniqueYears = cars.length > 0 ? [...new Set(cars.map((car) => car.year).filter((x) => {return x !== -1;}))] : [];
    const uniqueLocations = cars.length > 0 ? [...new Set(cars.map((car) => car.location))] : [];

    useEffect(() => {
        let filtered = cars;

        if (selectedBrands.length > 0) {
            filtered = filtered.filter((car) => selectedBrands.includes(car.brand));
        }

        if (selectedModels.length > 0) {
            filtered = filtered.filter((car) => selectedModels.includes(car.model));
        }

        if (selectedYears.length > 0) {
            filtered = filtered.filter((car) => selectedYears.includes(car.year));
        }

        if (selectedLocations.length > 0) {
            filtered = filtered.filter((car) => selectedLocations.includes(car.location));
        }

        filtered = filtered.filter(
            (car) => car.price >= priceRange[0] && car.price <= priceRange[1]
        );

        setFilteredCars(filtered);
        setCurrentPage(1);
    }, [selectedBrands, selectedModels, selectedYears, cars, priceRange, selectedLocations]);

    const toggleSection = (section) => {
        setOpenSection((prev) => (prev === section ? null : section));
    };

    const handleSliderChange = (event, newValue) => {
        setPriceRange(newValue);
    };

    const totalPages = Math.ceil(filteredCars.length / carsPerPage);
    const startIndex = (currentPage - 1) * carsPerPage;
    const currentCars = filteredCars.slice(startIndex, startIndex + carsPerPage);

    const handlePageChange = (page) => {
        setCurrentPage(page);
    };

    const contents =
        filteredCars.length === 0 ? (
            <p><em>No cars available. Try adjusting the filters.</em></p>
        ) : (
            <div>
                {currentCars.map((car) => (
                    <Element key={car.offerId} car={car} apiUrl={apiUrl} loggedIn={loggedIn}/>
                ))}
                <div className="pagination">
                    {Array.from({ length: totalPages }, (_, i) => (
                        <button
                            key={i}
                            onClick={() => handlePageChange(i + 1)}
                            className={currentPage === i + 1 ? 'active' : ''}
                        >
                            {i + 1}
                        </button>
                    ))}
                </div>
            </div>
        );

    return (
        <div>
            {location.pathname === '/' && (
                <div className="filter-wrapper">
                    <div className="filters">
                        <CollapsibleSectionGeneric
                            title="Brands"
                            isOpen={openSection === 'Brands'}
                            toggle={() => toggleSection('Brands')}
                        >
                            {isLoading ? <p>Loading brands...</p> : (
                                <Filter
                                    options={uniqueBrands}
                                    selectedValues={selectedBrands}
                                    onToggle={(brand) =>
                                        setSelectedBrands(
                                            selectedBrands.includes(brand)
                                                ? selectedBrands.filter((item) => item !== brand)
                                                : [...selectedBrands, brand]
                                        )
                                    }
                                />
                            )}
                        </CollapsibleSectionGeneric>
                        <CollapsibleSectionGeneric
                            title="Models"
                            isOpen={openSection === 'Models'}
                            toggle={() => toggleSection('Models')}
                        >
                            {isLoading ? <p>Loading models...</p> : (
                                <Filter
                                    options={uniqueModels}
                                    selectedValues={selectedModels}
                                    onToggle={(model) =>
                                        setSelectedModels(
                                            selectedModels.includes(model)
                                                ? selectedModels.filter((item) => item !== model)
                                                : [...selectedModels, model]
                                        )
                                    }
                                />
                            )}
                        </CollapsibleSectionGeneric>
                        <CollapsibleSectionGeneric
                            title="Years"
                            isOpen={openSection === 'Years'}
                            toggle={() => toggleSection('Years')}
                        >
                            {isLoading ? <p>Loading years...</p> : (
                                <Filter
                                    options={uniqueYears}
                                    selectedValues={selectedYears}
                                    onToggle={(year) =>
                                        setSelectedYears(
                                            selectedYears.includes(year)
                                                ? selectedYears.filter((item) => item !== year)
                                                : [...selectedYears, year]
                                        )
                                    }
                                />
                            )}
                        </CollapsibleSectionGeneric>
                        <CollapsibleSectionGeneric
                            title="Locations"
                            isOpen={openSection === 'Locations'}
                            toggle={() => toggleSection('Locations')}
                        >
                            {isLoading ? <p>Loading locations...</p> : (
                                <Filter
                                    options={uniqueLocations}
                                    selectedValues={selectedLocations}
                                    onToggle={(location) =>
                                        setSelectedLocations(
                                            selectedLocations.includes(location)
                                                ? selectedLocations.filter((item) => item !== location)
                                                : [...selectedLocations, location]
                                        )
                                    }
                                />
                            )}
                        </CollapsibleSectionGeneric>
                        <CollapsibleSectionGeneric
                            title="Booking Dates"
                            isOpen={openSection === 'Booking Dates'}
                            toggle={() => toggleSection('Booking Dates')}
                        >
                            <BookingDatePicker
                                startDate={startDate}
                                endDate={endDate}
                                setStartDate={setStartDate}
                                setEndDate={setEndDate}
                            />
                        </CollapsibleSectionGeneric>
                        <CollapsibleSectionGeneric
                            title="Price Range"
                            isOpen={openSection === 'Price Range'}
                            toggle={() => toggleSection('Price Range')}
                        >
                            <div className="price-range">
                                <Box sx={{ width: 200 }}>
                                    <Slider
                                        getAriaLabel={() => 'Price range'}
                                        value={priceRange}
                                        onChange={handleSliderChange}
                                        valueLabelDisplay="auto"
                                        valueLabelFormat={(value) => `$${value}`}
                                        min={0}
                                        max={maxPrice}
                                        step={1}
                                    />
                                </Box>
                            </div>
                        </CollapsibleSectionGeneric>
                        <Button onClick={fetchCars} className="searchButton">Search</Button>
                    </div>
                    <div className="contents">{isLoading ? (
                        <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100%', width: '100%' }}>
                            <CircularProgress size={60} />
                        </Box>
                    ) : contents}</div>
                </div>
            )}
            <TransitionGroup>
                <CSSTransition key={location.key} classNames="fade" timeout={300}>
                    <Routes location={location}>
                        <Route path="/cardetails/:offerId" element={<CarDetails />} />
                        <Route path="/" />
                    </Routes>
                </CSSTransition>
            </TransitionGroup>
        </div>
    );
}

export default MainPage;