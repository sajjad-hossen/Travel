import { useEffect, useRef, type FC } from 'react';
import L from 'leaflet';
import 'leaflet/dist/leaflet.css';
import icon from 'leaflet/dist/images/marker-icon.png';
import iconShadow from 'leaflet/dist/images/marker-shadow.png';

const defaultIcon = L.icon({
  iconUrl: icon,
  shadowUrl: iconShadow,
  iconSize: [25, 41],
  iconAnchor: [12, 41]
});

interface AttractionMapProps {
  attractionLat: number;
  attractionLng: number;
  attractionName: string;
  townLat?: number;
  townLng?: number;
  townName?: string;
}

export const AttractionMap: FC<AttractionMapProps> = ({
  attractionLat,
  attractionLng,
  attractionName,
  townLat,
  townLng,
  townName
}) => {
  const containerRef = useRef<HTMLDivElement>(null);
  const mapRef = useRef<L.Map | null>(null);

  useEffect(() => {
    if (!containerRef.current || mapRef.current) return;

    const map = L.map(containerRef.current).setView([attractionLat, attractionLng], 12);
    mapRef.current = map;

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      attribution: '&copy; OpenStreetMap contributors',
      maxZoom: 18
    }).addTo(map);

    L.marker([attractionLat, attractionLng], { icon: defaultIcon })
      .addTo(map)
      .bindPopup(attractionName)
      .openPopup();

    if (townLat != null && townLng != null) {
      L.marker([townLat, townLng], { icon: defaultIcon })
        .addTo(map)
        .bindPopup(townName || 'Main Town');

      L.polyline(
        [[townLat, townLng], [attractionLat, attractionLng]],
        { color: '#10b981', dashArray: '6 6', weight: 2 }
      ).addTo(map);

      map.fitBounds([[townLat, townLng], [attractionLat, attractionLng]], { padding: [40, 40] });
    }

    return () => {
      map.remove();
      mapRef.current = null;
    };
  }, [attractionLat, attractionLng, townLat, townLng]);

  return <div ref={containerRef} style={{ width: '100%', height: '320px', borderRadius: 'var(--radius-lg)', overflow: 'hidden' }} />;
};
