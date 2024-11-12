import React from 'react';
import { GoogleLogin } from '@react-oauth/google';

const Login = () => {
    const handleLoginSuccess = (credentialResponse) => {
        console.log('Login Success:', credentialResponse);
        // Send credentialResponse to your backend to authenticate
    };

    const handleLoginFailure = (error) => {
        console.error('Login Failed:', error);
    };

    return (
        <div>
            <h2>Sign in with Google</h2>
            <GoogleLogin
                onSuccess={handleLoginSuccess}
                onError={handleLoginFailure}
            />
        </div>
    );
};

export default Login;