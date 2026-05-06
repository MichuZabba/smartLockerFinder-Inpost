import type { SearchRequestBody, SearchResponse } from '../models'

const API_ENDPOINT = 'http://localhost:8080/api/ParcelLocker/Fetch'

/**
 * Search for parcel lockers based on city, street, and filters
 * @param request Search request with city, street, and filter options
 * @returns Promise with search results
 */
export async function searchParcelLockers(request: SearchRequestBody): Promise<SearchResponse> {
  const response = await fetch(API_ENDPOINT, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(request),
  });

  const responseText = await response.text();

  if (!response.ok) {
    throw new Error(`Backend returned ${response.status}: ${responseText}`);
  }

  return JSON.parse(responseText) as SearchResponse;
}
