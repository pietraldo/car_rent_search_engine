/* eslint-disable react/prop-types */
import { MapContainer, TileLayer, Marker, Popup } from 'react-leaflet';
import L from 'leaflet';
import 'leaflet/dist/leaflet.css';

// Import marker assets
import markerIcon2x from 'leaflet/dist/images/marker-icon-2x.png';
import markerIcon from 'leaflet/dist/images/marker-icon.png';
import markerShadow from 'leaflet/dist/images/marker-shadow.png';

// Fix for missing marker icons after deployment
delete L.Icon.Default.prototype._getIconUrl;

L.Icon.Default.mergeOptions({
    iconRetinaUrl: markerIcon2x,
    iconUrl: markerIcon,
    shadowUrl: markerShadow,
});

const LocationMap = ({ lat, lon, name }) => {
    const position = [lat, lon]; // Coordinates for the marker
    const garName = name;

    return (
        <div style={{ height: '400px', width: '100%' }}>
            <MapContainer center={position} zoom={13} style={{ width: '100%', height: '100%' }}>
                <TileLayer
                    url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
                />
                <Marker position={position}>
                    <Popup>
                        <strong>Address:</strong> {garName}
                    </Popup>
                </Marker>
            </MapContainer>
        </div>
    );
};

export default LocationMap;
