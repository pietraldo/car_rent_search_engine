import { Link } from 'react-router-dom'; // For route-based navigation
import './NavigationBar.css';
import logo from '../../assets/logo2.png';

const NavigationBar = () => {
    return (
        <nav className="navBar">
            <img src={logo} alt="logo" className="logo" />
            <div className="desktopMenu">
                <Link to="/" className="desktopMenuListItem">Strona startowa</Link>
                <Link to="/login" className="desktopMenuListItem">Zaloguj</Link>
            </div>
        </nav>
    );
};

export default NavigationBar;