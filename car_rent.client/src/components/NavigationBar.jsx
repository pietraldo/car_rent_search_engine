import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import '../Style/NavigationBar.css';
import logo from '../assets/logo2.png';

const NavigationBar = () => {
    const navigate = useNavigate();

    // Helper function to get a cookie value by name
    function getCookie(name) {
        const value = `; ${document.cookie}`;
        const parts = value.split(`; ${name}=`);
        if (parts.length === 2) {
            return decodeURIComponent(parts.pop().split(";").shift());
        }
        return null;
    }

    // Helper function to delete a cookie by name
    function deleteCookie(name) {
        document.cookie = `${name}=; max-age=0; path=/`;
    }

    // Clear cookies on the first page load
    useEffect(() => {
        // Check if it's the first time loading the page
        const isFirstLoad = !localStorage.getItem('hasVisited');

        if (isFirstLoad) {
            // Clear the cookies on the first page load
            deleteCookie('UserEmail');

            // Mark that the user has visited the page
            localStorage.setItem('hasVisited', 'true');
        }
    }, []);

    const email = getCookie("UserEmail");
    const [userEmail, setUserEmail] = useState(null);

    // Update the userEmail state if cookie exists
    useEffect(() => {
        const email = getCookie("UserEmail");
        if (email) {
            setUserEmail(email);
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
