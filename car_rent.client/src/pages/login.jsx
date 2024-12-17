import Button from "react-bootstrap/Button"; 
import "../Style/login.css"

const login = () => {
    return (
        <div>
            <h1>Login</h1>
            <input name="login" placeholder="Login" className="inputElement"/>
            <input name="password" type="password" placeholder="Password" className="inputElement" />
            <Button className="button">Login</Button>
        </div>
    );
};

export default login;