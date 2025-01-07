import { useState } from "react";
import { useNavigate } from "react-router-dom";
import UpdateUserData from "@/pages/UpdateUserData.jsx";

function FillUserDataAfterLogin() {
    return <>
        <div class="container">
            <h1 class="header">Successfully registered! To use our services, you need to give more information.</h1>
            <UpdateUserData />
        </div>            
    </>
}

export default FillUserDataAfterLogin;