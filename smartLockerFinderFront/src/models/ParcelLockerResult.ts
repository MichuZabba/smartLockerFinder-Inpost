/**
 * Parcel Locker Result Model
 * Represents a single parcel locker in search results
 */

import type { SearchLocation } from './SearchLocation'
 
export interface ParcelLockerResult {
  name: string
  city: string
  street: string
  returnEnabled: boolean
  allegroDelivery: boolean
  location: SearchLocation
}
