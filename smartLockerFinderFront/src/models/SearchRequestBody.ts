/**
 * Search Request Body Model
 * Represents the request payload for parcel locker searches
 */

import type { SearchFilters } from './SearchFilters'
import type { SearchLocation } from './SearchLocation'

export interface SearchRequestBody {
  location: SearchLocation
  filterFunctions: SearchFilters
}
