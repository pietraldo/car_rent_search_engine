import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import NavigationBar from './components/NavigationBar';
import Element from './components/Element';
import BookingDatePicker from './components/BookingDatePicker';
import Filter from './components/Filter';
import CollapsibleSectionGeneric from './components/CollapsibleSectionGeneric';
import CarDetails from './pages/CarDetails'
import History from './pages/history'
import MainPage from './pages/MainPage';
import FillData from "@/pages/FillData.jsx";

function App() {
    return (
        <Router>
            <div>
                <NavigationBar />
                <Routes>
                    <Route path="/" element={<MainPage />} />
                    <Route path="/fillData" element={<FillData />} />
                    <Route path="/history" element={<History/> } />  
                </Routes>
            </div>
        </Router>
    );
}

export default App;