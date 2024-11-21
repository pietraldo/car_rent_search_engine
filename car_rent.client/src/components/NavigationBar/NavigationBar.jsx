import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import './NavigationBar.css';
import logo from '../../assets/logo2.png';

const NavigationBar = () => {
    const [isLoggedIn, setIsLoggedIn] = useState(
        localStorage.getItem("isLoggedIn") === "true"
    );
    const [userName, setUserName] = useState("");

    const login = (username = 'User') => {
        localStorage.setItem("isLoggedIn", "true");
        setIsLoggedIn(true); // Sync state with localStorage
        setUserName(username);
        window.location.reload();
    };

    const logout = () => {
        localStorage.setItem("isLoggedIn", "false");
        setIsLoggedIn(false); // Sync state with localStorage
        setUserName("");
        window.location.reload();
    };

    useEffect(() => {
        const handleStorageChange = () => {
            const loggedInStatus = localStorage.getItem("isLoggedIn") === "true";
            setIsLoggedIn(loggedInStatus); // Sync state with localStorage changes
        };

        // Add event listener for `storage` events
        window.addEventListener("storage", handleStorageChange);

        // Cleanup on component unmount
        return () => {
            window.removeEventListener("storage", handleStorageChange);
        };
    }, []);

    return (
        <nav className="navBar">
            <img src={logo} alt="logo" className="logo" />
            <div className="desktopMenu">
                <Link to="/" className="desktopMenuListItem">Home</Link>
                <Link to="/history" className="desktopMenuListItem">History</Link>
                {isLoggedIn ? (
                    <button className="desktopMenuListItem" onClick={logout}>Logout</button>
                ) : (
                    <button className="desktopMenuListItem" onClick={login}>Login</button>
                )}
            </div>
        </nav>
    );
};

export default NavigationBar;
