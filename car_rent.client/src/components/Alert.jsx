// Alert.js
import '../Style/Alert.css';

function Alert(kolor, napis)
{
    // Tworzymy element alertu
    const alertDiv = document.createElement('div');
    alertDiv.className = 'alert';
    alertDiv.style.backgroundColor = kolor; // Ustawiamy kolor t³a
    alertDiv.textContent = napis; // Ustawiamy tekst alertu

    // Dodajemy alert do body
    document.body.appendChild(alertDiv);

    // Usuwamy alert po 3 sekundach
    setTimeout(() =>
    {
        alertDiv.classList.add('alert-hide'); // Dodajemy klasê ukrywaj¹c¹ alert
        setTimeout(() => document.body.removeChild(alertDiv), 500); // Usuwamy element po animacji
    }, 3000);
}

export default Alert;
