import { useEffect, useMemo, useRef, useState } from 'react'
import { MapContainer, TileLayer, Marker, Popup, useMap, useMapEvents, ZoomControl } from 'react-leaflet'
import L from 'leaflet'
import 'leaflet/dist/leaflet.css'
import icon from 'leaflet/dist/images/marker-icon.png'
import iconShadow from 'leaflet/dist/images/marker-shadow.png'
import './App.css'
import { searchParcelLockers } from './services/parcelLockerService'
import { geocodeAddress } from './services/geocodingService'
import type { ParcelLockerResult } from './models'

const DefaultIcon = L.icon({
  iconUrl: icon,
  shadowUrl: iconShadow,
  iconSize: [25, 41],
  iconAnchor: [12, 41],
})
L.Marker.prototype.options.icon = DefaultIcon

function MapCenterUpdater({ center }: { center: { lat: number; lng: number } }) {
  const map = useMap()
  useEffect(() => {
    map.panTo([center.lat, center.lng])
  }, [center, map])
  return null
}

function MapMoveTracker({ onMoveEnd }: { onMoveEnd: (center: { lat: number; lng: number }, zoom: number) => void }) {
  const map = useMapEvents({
    moveend: () => {
      const c = map.getCenter()
      const z = map.getZoom()
      onMoveEnd({ lat: c.lat, lng: c.lng }, z)
    },
  })
  return null
}

function App() {
  const [city, setCity] = useState('')
  const [street, setStreet] = useState('')
  const [allowReturn, setAllowReturn] = useState(true)
  const [allowAllegro, setAllowAllegro] = useState(true)
  const [blockedSignature, setBlockedSignature] = useState<string | null>(null)
  const [deliveryState, setDeliveryState] = useState('Czeka na wpisanie danych')
  const [mapCenter, setMapCenter] = useState({ lat: 52.2297, lng: 21.0122 })
  const [lockerCandidates, setLockerCandidates] = useState<ParcelLockerResult[]>([])
  const [mapZoom, setMapZoom] = useState(14)
  const mapPanelRef = useRef<HTMLDivElement>(null);

  const skipNextMoveRef = useRef(false)

  const activeFilters = useMemo(
    () => [
      allowReturn ? 'zwrot paczek' : null,
      allowAllegro ? 'Allegro delivery' : null,
    ].filter(Boolean) as string[],
    [allowAllegro, allowReturn],
  )

  const requestBody = useMemo(
    () => ({
      location: {
        longitude: mapCenter.lng,
        latitude: mapCenter.lat,
        limit: 300,
      },
      filterFunctions: {
        returnEnabled: allowReturn,
        allegroDelivery: allowAllegro,
      },
    }),
    [allowAllegro, allowReturn, mapCenter],
  )

  const requestSignature = useMemo(() => JSON.stringify(requestBody), [requestBody])

  useEffect(() => {
    if (city.trim() !== '') {
      const newZoom = street.trim() !== '' ? 17 : 14;
      setMapZoom(newZoom);
    }
  }, [city, street]);

  useEffect(() => {
    if (!city.trim()) return

    const timeoutId = window.setTimeout(async () => {
      try {
        setDeliveryState('Szukam lokalizacji…')
        const coords = await geocodeAddress(city, street)
        if (!coords) {
          setDeliveryState('Nie znaleziono lokalizacji.')
          return
        }
        skipNextMoveRef.current = true
        setMapCenter(coords)
      } catch {
        setDeliveryState('Błąd geokodowania.')
      }
    }, 600)

    return () => window.clearTimeout(timeoutId)
  }, [city, street])

  useEffect(() => {
    if (!city.trim()) return

    const timeoutId = window.setTimeout(async () => {
      try {
        setDeliveryState('Ładowanie paczkomatów…')
        const results = await searchParcelLockers(requestBody)
        setLockerCandidates(results.parcelLockerData)
        setDeliveryState(`Załadowano paczkomaty`)
        setBlockedSignature(null)
      } catch (error) {
        console.error('[searchParcelLockers] błąd:', error)
        setDeliveryState(`Błąd: ${error instanceof Error ? error.message : String(error)}`)
        setBlockedSignature(requestSignature)
      }
    }, 300)

    return () => window.clearTimeout(timeoutId)
  }, [requestBody, requestSignature, city])

  const handleMoveEnd = (center: { lat: number; lng: number }, zoom: number) => {
  if (skipNextMoveRef.current) {
    skipNextMoveRef.current = false
    return
  }
  setMapCenter(center)
  setMapZoom(zoom);
}

  const handleLockerClick = (locker: ParcelLockerResult) => {
    setMapCenter({
      lat: locker.location.latitude,
      lng: locker.location.longitude
    });
    
    setMapZoom(18); 
    
    if (mapPanelRef.current) {
    mapPanelRef.current.scrollIntoView({ 
      behavior: 'smooth', 
      block: 'start' 
    });
  }
  };

  const handleReset = () => {
    setCity('')
    setStreet('')
    setAllowReturn(true)
    setAllowAllegro(true)
    setBlockedSignature(null)
    setLockerCandidates([])
    setDeliveryState('Czeka na wpisanie danych')
    setMapCenter({ lat: 52.2297, lng: 21.0122 })
  }

  return (
    <main className="page-shell">
      <section className="search-panel single-column-panel">
        <div className="panel-heading compact">
          <p className="eyebrow">Inwentaryzator paczkomatów</p>
          <h1>Wyszukaj paczkomat w swojej okolicy.</h1>
        </div>

        <div className="form-grid">
          <label className="field">
            <span>Miasto</span>
            <input
              type="text"
              value={city}
              onChange={(e) => setCity(e.target.value)}
              placeholder="np. Warszawa"
            />
          </label>
          <label className="field">
            <span>Ulica (opcjonalnie)</span>
            <input
              type="text"
              value={street}
              onChange={(e) => setStreet(e.target.value)}
              placeholder="np. Marszałkowska"
            />
          </label>
        </div>

        <details className="filters-dropdown" open>
          <summary>
            <span>Aktywne filtry</span>
            <strong>{activeFilters.length ? activeFilters.join(' · ') : 'Brak'}</strong>
          </summary>
          <div className="filter-list">
            <label className="filter-row">
              <input type="checkbox" checked={allowReturn} onChange={(e) => setAllowReturn(e.target.checked)} />
              <div><strong>Możliwość zwrotu</strong></div>
            </label>
            <label className="filter-row">
              <input type="checkbox" checked={allowAllegro} onChange={(e) => setAllowAllegro(e.target.checked)} />
              <div><strong>Allegro Delivery</strong></div>
            </label>
          </div>
        </details>

        <div className="actions single-action-row">
          <button type="button" className="secondary-action" onClick={handleReset}>Wyczyść</button>
        </div>
        <p className="delivery-state">{deliveryState}</p>
      </section>

      <section className="map-panel" ref={mapPanelRef}>
        <div className="panel-heading compact">
          <h2>Mapa</h2>
          <p>Mapa centruje się na wpisanej lokalizacji.</p>
        </div>

        <div className="map-frame-wrap" style={{ height: '400px', width: '100%' }}>
          <MapContainer
            center={[mapCenter.lat, mapCenter.lng]}
            zoom={mapZoom}
            style={{ height: '100%', width: '100%' }}
          >
            <TileLayer url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png" />
            <MapCenterUpdater center={mapCenter} />
            <MapMoveTracker onMoveEnd={handleMoveEnd} />
            {lockerCandidates.map((locker, index) => (
              <Marker key={index} position={[locker.location.latitude, locker.location.longitude]}>
                <Popup>
                  <strong>{locker.name}</strong><br />
                  {locker.street}, {locker.city}
                </Popup>
              </Marker>
            ))}
          </MapContainer>
        </div>

        <div className="locker-grid">
          {lockerCandidates.map((locker, index) => (
            <article className="locker-card" key={index} onClick={()=>handleLockerClick(locker)} style={{cursor: 'pointer'}}>
              <strong>{locker.name}</strong>
              <span>{locker.street}, {locker.city}</span>
            </article>
          ))}
        </div>
      </section>
    </main>
  )
}

export default App