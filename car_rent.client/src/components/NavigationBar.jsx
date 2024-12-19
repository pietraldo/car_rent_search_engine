import { useEffect, useState } from 'react';
import React from "react";
import { Link } from "react-router-dom";
import { useNavigate } from 'react-router-dom';
import '../Style/NavigationBar.css';
import logo from '../assets/logo2.png';


const NavigationBar = () => {
    const navigate = useNavigate();
    function getCookie(name) {
        const value = `; ${document.cookie}`;
        const parts = value.split(`; ${name}=`);

        if (parts.length === 2) {
            // Decode the cookie value
            return decodeURIComponent(parts.pop().split(";").shift());
        }
    }

    const email = getCookie("UserEmail");
    const [userEmail, setUserEmail] = useState(null);

    useEffect(() => {
        // Retrieve the user email cookie when the component mounts
        const email = getCookie("UserEmail"); // Cookie name: "UserEmail"
        if (email) {
            setUserEmail(email); // Set user email to state
        }
    }, []);

    return (
        <nav className="navBar">
            <img src={logo} alt="logo" className="logo" />
            <div className="desktopMenu">
                <button
                    className="desktopMenuButton"
                    onClick={() => navigate('/')}
                >
                    Home
                </button>
                <button
                    className="desktopMenuButton"
                    disabled={!userEmail}
                    onClick={() => navigate('/history')}
                    title={!userEmail ? "You must be logged in to view history" : ""}
                >
                    History
                </button>
                  <button
                    className="desktopMenuButton"
                    onClick={() => navigate('/fillData')}
                >
                    Update Data
                </button>
                <li className="googleButtonHolder">
                    <form action={userEmail ? "api/Identity/google-logout" : "/api/Identity/google-login"} method="post">
                        <button type="submit" name="login-with-google" value="login-with-google"
                            className="desktopMenuButton">
                            {userEmail ? `${userEmail}` : "Login with Google"}
                        </button>
                    </form>
                </li>
                
            </div>
        </nav>
    );
};
export default NavigationBar;