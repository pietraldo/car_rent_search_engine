/* eslint-disable react/prop-types */
import { MapContainer, TileLayer, Marker, Popup } from 'react-leaflet';
import L from 'leaflet';
import 'leaflet/dist/leaflet.css'; // Import Leaflet CSS

const LocationMap = ({ lat, lon }) => {
    const position = [lat, lon]; // Coordinates for the marker

    return (
        <div style={{ height: '400px', width: '100%' }}>
            <MapContainer center={position} zoom={13} style={{ width: '100%', height: '100%' }}>
                <TileLayer
                    url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
                />
                <Marker position={position}>
                    <Popup>
                        <strong>Location:</strong> {lat}, {lon}
                    </Popup>
                </Marker>
            </MapContainer>
        </div>
    );
};

export default LocationMap;
