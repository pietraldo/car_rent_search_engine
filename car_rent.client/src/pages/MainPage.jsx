import { useEffect, useState } from 'react'; // React hooks
import { BrowserRouter as Router, Routes, Route, useLocation } from 'react-router-dom'; // React Router
import { CSSTransition, TransitionGroup } from 'react-transition-group'; // Transition utilities

// Components
import Element from '../components/Element';
import BookingDatePicker from '../components/BookingDatePicker';
import Filter from '../components/Filter';
import CollapsibleSectionGeneric from '../components/CollapsibleSectionGeneric';
import CarDetails from '../pages/CarDetails';

// Styles
import '../Style/App.css';


function MainPage()
{
    const [cars, setCars] = useState([]); // All cars fetched from the backend
    const [filteredCars, setFilteredCars] = useState([]); // Cars after filtering
    const [selectedBrands, setSelectedBrands] = useState([]); // Selected brands
    const [selectedModels, setSelectedModels] = useState([]); // Selected models
    const [selectedYears, setSelectedYears] = useState([]); // Selected years
    const [selectedColors, setSelectedColors] = useState([]); // Selected colors
    const apiUrl = import.meta.env.VITE_API_URL;
    const location = useLocation();
    const [isLoading, setIsLoading] = useState(true);
    const [currentPage, setCurrentPage] = useState(1);
    const carsPerPage = 5;

    const [startDate, setStartDate] = useState(new Date());
    const [endDate, setEndDate] = useState(() =>
    {
        const tomorrow = new Date();
        tomorrow.setDate(tomorrow.getDate() + 1);
        return tomorrow;
    });

    const searchForCars = () =>
    {
        fetchCars();
    }

    // Update selected values
    const handleToggleSelection = (value, selectedValues, setSelectedValues) =>
    {
        if (selectedValues.includes(value))
        {
            setSelectedValues(selectedValues.filter((item) => item !== value));
        } else
        {
            setSelectedValues([...selectedValues, value]);
        }
    }
    useEffect(() =>
    {
        fetchCars();
    }, []);

    const fetchCars = async () =>
    {
        setIsLoading(true);
        await getCars();
        setIsLoading(false);
    }

    // Fetch car data from the API
    async function getCars()
    {
        try
        {
            const formattedStartDate = startDate.toISOString();
            const formattedEndDate = endDate.toISOString();
            const response = await fetch(`/Car?startdate=${formattedStartDate}&enddate=${formattedEndDate}`);
            if (!response.ok)
            {
                throw new Error('Failed to fetch car data.');
            }
            const rawResponseText = await response.text();
            console.log(rawResponseText);

            const data = JSON.parse(rawResponseText);
            console.log(data[0]);

            

            const aa = data.map(bb =>
            {
                var cc = {};
                cc.model = bb.car.model;
                cc.year = bb.car.year;
                cc.brand = bb.car.brand;
                cc.picture = bb.car.picture;
                cc.price = bb.price;
                cc.endDate = bb.endDate;
                cc.startDate = bb.startDate;
                cc.offerId = bb.id;
                return cc;
            }
            );
            
            setCars(aa);
            setFilteredCars(aa);
        } catch (error)
        {
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
    useEffect(() =>
    {
        let filtered = cars;

        if (selectedBrands.length > 0)
        {
            filtered = filtered.filter(car => selectedBrands.includes(car.brand));
        }

        if (selectedModels.length > 0)
        {
            filtered = filtered.filter(car => selectedModels.includes(car.model));
        }

        if (selectedYears.length > 0)
        {
            filtered = filtered.filter(car => selectedYears.includes(car.year));
        }

        if (selectedColors.length > 0)
        {
            filtered = filtered.filter(car => selectedColors.includes(car.color));
        }

        setFilteredCars(filtered);
        setCurrentPage(1);
    }, [selectedBrands, selectedModels, selectedYears, selectedColors, cars]);

    const totalPages = Math.ceil(filteredCars.length / carsPerPage);
    const startIndex = (currentPage - 1) * carsPerPage;
    const currentCars = filteredCars.slice(startIndex, startIndex + carsPerPage);
    const handlePageChange = (page) =>
    {
        setCurrentPage(page);
    };

    const contents = filteredCars.length === 0
        ? <p><em>No cars available. Try adjusting the filters.</em></p>
        : (
            <div>
                {currentCars.map((car) => (
                    <Element key={car.id} car={car} apiUrl={apiUrl}/>
                ))}
                {/* Pagination Controls */}
                <div className="pagination">
                    {Array.from({ length: totalPages }, (_, i) => (
                        <button
                            key={i}
                            onClick={() => handlePageChange(i + 1)}
                            className={currentPage === i + 1 ? "active" : ""}
                        >
                            {i + 1}
                        </button>
                    ))}
                </div>
            </div>
        );

    return (
        <div>
            {location.pathname === "/" && (
                isLoading ? (
                    <p>Loading filters...</p>
                ) : (
                    <div>
                        <div className="filters">
                            <h2>Filter by:</h2>

                            {/* Filters */}
                            <CollapsibleSectionGeneric title="Brands">
                                <Filter
                                    options={uniqueBrands}
                                    selectedValues={selectedBrands}
                                    onToggle={(brand) =>
                                        handleToggleSelection(brand, selectedBrands, setSelectedBrands)
                                    }
                                />
                            </CollapsibleSectionGeneric>
                            <CollapsibleSectionGeneric title="Models">
                                <Filter
                                    options={uniqueModels}
                                    selectedValues={selectedModels}
                                    onToggle={(model) =>
                                        handleToggleSelection(model, selectedModels, setSelectedModels)
                                    }
                                />
                            </CollapsibleSectionGeneric>
                            <CollapsibleSectionGeneric title="Years">
                                <Filter
                                    options={uniqueYears}
                                    selectedValues={selectedYears}
                                    onToggle={(year) =>
                                        handleToggleSelection(year, selectedYears, setSelectedYears)
                                    }
                                />
                            </CollapsibleSectionGeneric>
                            <CollapsibleSectionGeneric title="Colors">
                                <Filter
                                    options={uniqueColors}
                                    selectedValues={selectedColors}
                                    onToggle={(color) =>
                                        handleToggleSelection(color, selectedColors, setSelectedColors)
                                    }
                                />
                            </CollapsibleSectionGeneric>

                            {/* Booking Date Picker */}
                            <CollapsibleSectionGeneric title="Booking Dates">
                                <BookingDatePicker startDate={startDate} endDate={endDate} setEndDate={setEndDate} setStartDate={setStartDate} />
                            </CollapsibleSectionGeneric>
                            <button onClick={searchForCars}>Search</button>
                        </div>
                        <div className="contents"> {contents} </div>
                    </div>
                )
            )
            }


            <TransitionGroup>
                <CSSTransition key={location.key} classNames="fade" timeout={300}>
                    <Routes location={location}>
                        <Route path="/car_details/:offerId" element={<CarDetails />} />
                        <Route path="/" />
                    </Routes>
                </CSSTransition>
            </TransitionGroup>
        </div>
    );
};

export default MainPage;