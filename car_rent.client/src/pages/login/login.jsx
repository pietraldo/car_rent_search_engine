import { useState } from 'react';
import Button from "react-bootstrap/Button"; // Correct import
import "./login.css"

const login = () => {
    const [login, setLogin] = useState('');
    const [password, setPassword] = useState('');
    const [errorMessage, setErrorMessage] = useState('');

    const handleLogin = async () => {
        const loginData = { login, password };
        console.log(login);
        console.log(password);
        try {
            const response = await fetch('http://localhost:5173/login', {  // Adjust URL if needed
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(loginData),
            });
            const data = await response.json();
            
            if (response.ok) {
                // Login successful, handle success (e.g., redirect or show message)
                alert(data.message);
            } else {
                // Handle error (e.g., invalid credentials)
                setErrorMessage(data.message);
            }
        } catch (error) {
            console.error('Error during login:', error);
            setErrorMessage('An error occurred, please try again later.');
        }
    };
    return (
        <div>
            <h1>Login</h1>
            <input
                name="login"
                placeholder="Login"
                className="inputElement"
                value={login}
                onChange={(e) => setLogin(e.target.value)}
            />
            <input
                name="password"
                type="password"
                placeholder="Password"
                className="inputElement"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
            />
            <Button className="button" onClick={handleLogin}>Login</Button>

            {errorMessage && <p style={{ color: 'red' }}>{errorMessage}</p>}
        </div>
    );
};

export default login;