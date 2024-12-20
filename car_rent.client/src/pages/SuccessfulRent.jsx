import React from 'react';
import "../Style/SuccessfulRent.css"
import { useNavigate } from 'react-router-dom';

function SuccessfulRent()
{
    const navigate = useNavigate();
    return (
        <div class="container" >
            <h1 class="header">Successful Rent</h1>
            <p class="message">
                Your car rental was successful! You can view the details in your rental history.
            </p>
            <button
                class="link"
                className="desktopMenuButton"
                onClick={() => navigate('/history')}
            >
                View details
            </button>
        </div>
    );
};


export default SuccessfulRent;