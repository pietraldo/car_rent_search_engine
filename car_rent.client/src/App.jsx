import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import NavigationBar from './components/NavigationBar';
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
                </Routes>
            </div>
        </Router>
    );
}

export default App;