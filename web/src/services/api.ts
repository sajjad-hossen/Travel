import type { LocationDto, RouteSearchResultDto, DestinationDetailDto } from '../types/travel';

const API_BASE = 'http://localhost:5000/api/v1';

export const travelApi = {
  async getDestinations(): Promise<LocationDto[]> {
    const res = await fetch(`${API_BASE}/locations/destinations`);
    if (!res.ok) throw new Error('Failed to load destinations');
    return res.json();
  },

  async searchLocations(q: string): Promise<LocationDto[]> {
    const res = await fetch(`${API_BASE}/locations/search?q=${encodeURIComponent(q)}`);
    if (!res.ok) throw new Error('Search failed');
    return res.json();
  },

  async searchRoutes(from: string, to: string, pref?: string): Promise<RouteSearchResultDto> {
    const url = `${API_BASE}/search/routes?from=${encodeURIComponent(from)}&to=${encodeURIComponent(to)}${
      pref ? `&pref=${encodeURIComponent(pref)}` : ''
    }`;
    const res = await fetch(url);
    if (!res.ok) {
      const err = await res.json().catch(() => ({}));
      throw new Error(err.message || 'No available routes found');
    }
    return res.json();
  },

  async getDestinationDetails(slug: string): Promise<DestinationDetailDto> {
    const res = await fetch(`${API_BASE}/destinations/${encodeURIComponent(slug)}`);
    if (!res.ok) throw new Error('Failed to load destination details');
    return res.json();
  }
};
