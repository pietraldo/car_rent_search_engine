import { useEffect, useState } from 'react';
import './App.css';
import NavigationBar from './components/NavigationBar/NavigationBar';
import Element from './components/Element/Element';
<<<<<<< HEAD
import Login from './pages/login/login'; // Ensure this path is correct
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { CSSTransition, TransitionGroup } from 'react-transition-group';
=======
import Filter from './components/Filter/Filter';
import Dropdown from 'react-bootstrap/Dropdown';
import DropdownButton from 'react-bootstrap/DropdownButton';
import Login from './pages/login/login'; // Ensure this path is correct
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { CSSTransition, TransitionGroup } from 'react-transition-group';
import Calendar from "react-calendar"
>>>>>>> origin/master
import Reserve from './pages/reserve/reserve'
import SearchBar from './components/SearchBar/SearchBar';


function App() {
    const [cars, setCars] = useState([]);
    const [filteredCars, setFilteredCars] = useState([]);
    const [selectedModel, setSelectedModel] = useState(null);
    const apiUrl = import.meta.env.VITE_API_URL;
    console.log(apiUrl);

    useEffect(() => {
        getCars();
    }, []);

    async function getCars() {
        try {
            const response = await fetch('Car');
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            const data = await response.json();
            setCars(data);
        } catch (error) {
            console.error('Error fetching cars:', error);
        }
    }

    const uniqueModels = [...new Set(cars.map(car => car.model))];

    const contents = (selectedModel ? filteredCars : cars).length === 0
        ? <p><em>Loading... Car data should be here if backend starts</em></p>
        : (
            <div>
                {(selectedModel ? filteredCars : cars).map((car) => (
                    <Element key={car.model} car={car} apiUrl={apiUrl} />
                ))}
            </div>
        );

    const handleSelect = (model) => {
        setSelectedModel(model);
        if (model) {
            setFilteredCars(cars.filter(car => car.model === model));
        } else {
            setFilteredCars(cars); // Show all if no filter is selected
        }
    };

    return (
        <div>
            <NavigationBar />
            
            <TransitionGroup>
                <CSSTransition key={location.key} classNames="fade" timeout={300}>
                    <Routes location={location}>
                        <Route path="/login" element={<Login />} />
                        <Route path="/" element={
                            <div>
                                <SearchBar />
                                {contents}
                            </div>} />
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