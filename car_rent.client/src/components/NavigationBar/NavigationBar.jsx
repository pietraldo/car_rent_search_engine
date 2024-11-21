import { Link } from 'react-router-dom'; // For route-based navigation
import './NavigationBar.css';
import logo from '../../assets/logo2.png';

const NavigationBar = () => {
    return (
        <nav className="navBar">
            <img src={logo} alt="logo" className="logo" />
            <div className="desktopMenu">
                <Link to="/" className="desktopMenuListItem">Home</Link>
                <Link to="/login" className="desktopMenuListItem">Login</Link>
                <li>
                    <form action="/api/Identity/google-login" method="post">
                        <button type="submit" name="login-with-google" value="login-with-google"
                                className="navbar-link">Login with
                            Google
                        </button>
                    </form>
                </li>
            </div>
        </nav>
    );
};

export default NavigationBar;
