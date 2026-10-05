export interface LocationDto {
  id: string;
  name: string;
  banglaName?: string;
  slug: string;
  type: string;
  district: string;
  division: string;
  latitude?: number;
  longitude?: number;
  isMajorHub: boolean;
  isTouristDestination: boolean;
  description?: string;
  heroImageUrl?: string;
}

export interface TransportOptionDto {
  id: string;
  mode: 'Bus' | 'Train' | 'Flight' | 'Launch' | 'Cng' | 'ChanderGari' | 'Ferry' | 'Boat';
  tier: string;
  operatorName: string;
  minCostBdt: number;
  maxCostBdt: number;
  frequencyPerDay: number;
  departureStation?: string;
  arrivalStation?: string;
  bookingLinksJson?: string;
  scheduleNotes?: string;
}

export interface RouteStepDto {
  stepNumber: number;
  originId: string;
  originName: string;
  destinationId: string;
  destinationName: string;
  distanceKm: number;
  avgDurationMinutes: number;
  transportOptions: TransportOptionDto[];
}

export interface RoutePlanDto {
  planType: string;
  totalDurationMinutes: number;
  totalMinCostBdt: number;
  totalMaxCostBdt: number;
  totalHops: number;
  steps: RouteStepDto[];
}

export interface RouteSearchResultDto {
  origin: LocationDto;
  destination: LocationDto;
  plans: RoutePlanDto[];
}

export interface AttractionDto {
  id: string;
  name: string;
  banglaName?: string;
  description?: string;
  bestTimeToVisit?: string;
  entryFeeBdt: number;
  imageUrl?: string;
  category?: string;
}

export interface AccommodationDto {
  id: string;
  name: string;
  budgetLevel: string;
  approxPriceRange: string;
  address?: string;
  contactPhone?: string;
  bookingUrl?: string;
  rating: number;
  highlightFeature?: string;
}

export interface AdvisoryDto {
  id: string;
  category: string;
  title: string;
  content: string;
  isMandatory: boolean;
}

export interface DestinationDetailDto {
  location: LocationDto;
  attractions: AttractionDto[];
  accommodations: AccommodationDto[];
  advisories: AdvisoryDto[];
}
