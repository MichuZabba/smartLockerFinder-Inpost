/**
 * Search Response Model
 * Represents the response from the backend after a parcel locker search
 */

import type { ParcelLockerResult } from './ParcelLockerResult'
 
export interface SearchResponse {
  errorMessage: string | null
  parcelLockerData: ParcelLockerResult[]
}
