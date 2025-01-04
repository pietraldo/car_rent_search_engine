import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { useState, useEffect } from 'react';
import NavigationBar from './components/NavigationBar';
import History from './pages/history'
import MainPage from './pages/MainPage';
import UpdateUserData from "@/pages/UpdateUserData.jsx";
import RentDetails from './pages/RentDetails';
import CarDetails from './pages/CarDetails';
import SuccessfulRent from './pages/SuccessfulRent';

function App() {
    const [userMail, setUserMail] = useState(null);
    function getCookie(name) {
        const value = `; ${document.cookie}`;
        const parts = value.split(`; ${name}=`);

        if (parts.length === 2) {
            // Decode the cookie value
            return decodeURIComponent(parts.pop().split(";").shift());
        }
    }
    useEffect(() => {
        // Retrieve the user email cookie when the component mounts
        const email = getCookie("UserEmail"); // Cookie name: "UserEmail"
        if (email) {
            setUserMail(email); // Set user email to state
        }
    }, []);
    return (
        <Router>
            <div>
                <NavigationBar />
                <Routes>
                    <Route path="/" element={<MainPage />} />
                    <Route path="/fillData" element={<UpdateUserData />} />
                    <Route path="/history" element={<History />} />  
                    <Route path="/rent_details" element={<RentDetails />} />
                    <Route path="/cardetails/:carId" element={<CarDetails />} />
                    <Route path="/successfulRent" element={<SuccessfulRent/>} />
                </Routes>
            </div>
        </Router>
    );
}

export default App;