import { useNavigate } from 'react-router-dom';
import '../Style/NavigationBar.css';
import logo from '../assets/logo2.png';

const NavigationBar = () => {
    const navigate = useNavigate();
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
                <li className="googleButtonHolder">
                    <form action="/api/Identity/google-login" method="post">
                        <button type="submit" name="login-with-google" value="login-with-google"
                            className="desktopMenuButton">Login with
                            Google
                        </button>
                    </form>
                </li>
            </div>
        </nav>
    );
};
export default NavigationBar;