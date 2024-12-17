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
<<<<<<< HEAD:car_rent.client/src/components/NavigationBar/NavigationBar.jsx
                            className="desktopMenuButton">Login with
=======
                                className="desktopMenuButton">Login with
>>>>>>> f1ece068d45f8e8d1c702798c752a8fe1f3877cb:car_rent.client/src/components/NavigationBar.jsx
                            Google
                        </button>
                    </form>
                </li>
            </div>
        </nav>
    );
};
export default NavigationBar;