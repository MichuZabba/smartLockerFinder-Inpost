const NOMINATIM_URL = 'https://nominatim.openstreetmap.org/search'

export async function geocodeAddress(city: string, street?: string): Promise<{ lat: number; lng: number } | null> {
  const query = [street?.trim(), city.trim()].filter(Boolean).join(', ')

  const params = new URLSearchParams({
    q: query,
    format: 'json',
    limit: '1',
    countrycodes: 'pl',
  })

  const response = await fetch(`${NOMINATIM_URL}?${params}`, {
    headers: {
      'Accept-Language': 'pl',
    },
  })

  if (!response.ok) throw new Error(`Geocoding error: ${response.status}`)

  const data = await response.json()
  if (!data.length) return null

  return {
    lat: parseFloat(data[0].lat),
    lng: parseFloat(data[0].lon),
  }
}