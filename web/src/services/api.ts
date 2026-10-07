import type {
  LocationDto,
  RouteSearchResultDto,
  DestinationDetailDto,
  TransportOptionDto,
  AttractionDto,
  AccommodationDto,
  AdvisoryDto,
  AttractionDetailDto,
  FeedbackDto,
  AuthUser
} from '../types/travel';

const API_BASE = 'http://localhost:5000/api/v1';

const USER_TOKEN_KEY = 'travelbd_user_token';
const USER_INFO_KEY = 'travelbd_user_info';

export const authStore = {
  getToken(): string | null {
    return localStorage.getItem(USER_TOKEN_KEY);
  },
  getUser(): AuthUser | null {
    const raw = localStorage.getItem(USER_INFO_KEY);
    return raw ? JSON.parse(raw) : null;
  },
  save(token: string, user: AuthUser) {
    localStorage.setItem(USER_TOKEN_KEY, token);
    localStorage.setItem(USER_INFO_KEY, JSON.stringify(user));
  },
  clear() {
    localStorage.removeItem(USER_TOKEN_KEY);
    localStorage.removeItem(USER_INFO_KEY);
  },
  isAdmin(): boolean {
    return this.getUser()?.role === 'Admin';
  }
};

const getAuthHeaders = (): Record<string, string> => {
  const token = authStore.getToken();
  return {
    'Content-Type': 'application/json',
    ...(token ? { Authorization: `Bearer ${token}` } : {})
  };
};

export const authApi = {
  async register(name: string, email: string, password: string): Promise<AuthUser> {
    const res = await fetch(`${API_BASE}/auth/register`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ name, email, password })
    });
    if (!res.ok) {
      const err = await res.json().catch(() => ({}));
      throw new Error(err.message || 'Registration failed');
    }
    const data = await res.json();
    const user: AuthUser = { userId: data.userId, name: data.name, email: data.email, role: data.role };
    authStore.save(data.token, user);
    return user;
  },

  async login(email: string, password: string): Promise<AuthUser> {
    const res = await fetch(`${API_BASE}/auth/login`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ email, password })
    });
    if (!res.ok) {
      const err = await res.json().catch(() => ({}));
      throw new Error(err.message || 'Login failed');
    }
    const data = await res.json();
    const user: AuthUser = { userId: data.userId, name: data.name, email: data.email, role: data.role };
    authStore.save(data.token, user);
    return user;
  },

  logout() {
    authStore.clear();
  }
};

export const attractionApi = {
  async getById(id: string): Promise<AttractionDetailDto> {
    const res = await fetch(`${API_BASE}/attractions/${id}`);
    if (!res.ok) throw new Error('Failed to load attraction details');
    return res.json();
  }
};

export const feedbackApi = {
  async list(params: { locationId?: string; attractionId?: string }): Promise<FeedbackDto[]> {
    const qs = new URLSearchParams();
    if (params.locationId) qs.set('locationId', params.locationId);
    if (params.attractionId) qs.set('attractionId', params.attractionId);
    const res = await fetch(`${API_BASE}/feedback?${qs.toString()}`);
    if (!res.ok) throw new Error('Failed to load reviews');
    return res.json();
  },

  async create(data: { locationId?: string; attractionId?: string; rating: number; comment: string }): Promise<FeedbackDto> {
    const res = await fetch(`${API_BASE}/feedback`, {
      method: 'POST',
      headers: getAuthHeaders(),
      body: JSON.stringify(data)
    });
    if (!res.ok) {
      const err = await res.json().catch(() => ({}));
      throw new Error(err.message || 'Failed to submit review');
    }
    return res.json();
  }
};

export interface AdminRouteSegmentDto {
  id: string;
  originId: string;
  originName: string;
  destinationId: string;
  destinationName: string;
  distanceKm: number;
  avgDurationMinutes: number;
  isActive: boolean;
  transportOptions: TransportOptionDto[];
}

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

const getAdminHeaders = () => getAuthHeaders();

export const adminApi = {
  // ── Locations ───────────────────────────────────────────────────────────────
  async getAllLocations(): Promise<LocationDto[]> {
    const res = await fetch(`${API_BASE}/admin/locations`, {
      headers: getAdminHeaders()
    });
    if (!res.ok) throw new Error('Failed to load locations (Admin Authorization Required)');
    return res.json();
  },

  async createLocation(data: Partial<LocationDto>): Promise<LocationDto> {
    const res = await fetch(`${API_BASE}/admin/locations`, {
      method: 'POST',
      headers: getAdminHeaders(),
      body: JSON.stringify(data)
    });
    if (!res.ok) {
      const err = await res.json().catch(() => ({}));
      throw new Error(err.message || 'Failed to create location');
    }
    return res.json();
  },

  async updateLocation(id: string, data: Partial<LocationDto>): Promise<LocationDto> {
    const res = await fetch(`${API_BASE}/admin/locations/${id}`, {
      method: 'PUT',
      headers: getAdminHeaders(),
      body: JSON.stringify(data)
    });
    if (!res.ok) {
      const err = await res.json().catch(() => ({}));
      throw new Error(err.message || 'Failed to update location');
    }
    return res.json();
  },

  async deleteLocation(id: string): Promise<void> {
    const res = await fetch(`${API_BASE}/admin/locations/${id}`, {
      method: 'DELETE',
      headers: getAdminHeaders()
    });
    if (!res.ok) throw new Error('Failed to delete location');
  },

  // ── Attractions ─────────────────────────────────────────────────────────────
  async addAttraction(data: Partial<AttractionDto> & { locationId: string }): Promise<AttractionDto> {
    const res = await fetch(`${API_BASE}/admin/locations/attractions`, {
      method: 'POST',
      headers: getAdminHeaders(),
      body: JSON.stringify(data)
    });
    if (!res.ok) throw new Error('Failed to add attraction');
    return res.json();
  },

  async updateAttraction(id: string, data: Partial<AttractionDto>): Promise<AttractionDto> {
    const res = await fetch(`${API_BASE}/admin/locations/attractions/${id}`, {
      method: 'PUT',
      headers: getAdminHeaders(),
      body: JSON.stringify(data)
    });
    if (!res.ok) throw new Error('Failed to update attraction');
    return res.json();
  },

  async deleteAttraction(id: string): Promise<void> {
    const res = await fetch(`${API_BASE}/admin/locations/attractions/${id}`, {
      method: 'DELETE',
      headers: getAdminHeaders()
    });
    if (!res.ok) throw new Error('Failed to delete attraction');
  },

  // ── Accommodations ──────────────────────────────────────────────────────────
  async addAccommodation(data: Partial<AccommodationDto> & { locationId: string }): Promise<AccommodationDto> {
    const res = await fetch(`${API_BASE}/admin/locations/accommodations`, {
      method: 'POST',
      headers: getAdminHeaders(),
      body: JSON.stringify(data)
    });
    if (!res.ok) throw new Error('Failed to add accommodation');
    return res.json();
  },

  async updateAccommodation(id: string, data: Partial<AccommodationDto>): Promise<AccommodationDto> {
    const res = await fetch(`${API_BASE}/admin/locations/accommodations/${id}`, {
      method: 'PUT',
      headers: getAdminHeaders(),
      body: JSON.stringify(data)
    });
    if (!res.ok) throw new Error('Failed to update accommodation');
    return res.json();
  },

  async deleteAccommodation(id: string): Promise<void> {
    const res = await fetch(`${API_BASE}/admin/locations/accommodations/${id}`, {
      method: 'DELETE',
      headers: getAdminHeaders()
    });
    if (!res.ok) throw new Error('Failed to delete accommodation');
  },

  // ── Advisories ──────────────────────────────────────────────────────────────
  async addAdvisory(data: Partial<AdvisoryDto> & { locationId: string }): Promise<AdvisoryDto> {
    const res = await fetch(`${API_BASE}/admin/locations/advisories`, {
      method: 'POST',
      headers: getAdminHeaders(),
      body: JSON.stringify(data)
    });
    if (!res.ok) throw new Error('Failed to add advisory');
    return res.json();
  },

  async updateAdvisory(id: string, data: Partial<AdvisoryDto>): Promise<AdvisoryDto> {
    const res = await fetch(`${API_BASE}/admin/locations/advisories/${id}`, {
      method: 'PUT',
      headers: getAdminHeaders(),
      body: JSON.stringify(data)
    });
    if (!res.ok) throw new Error('Failed to update advisory');
    return res.json();
  },

  async deleteAdvisory(id: string): Promise<void> {
    const res = await fetch(`${API_BASE}/admin/locations/advisories/${id}`, {
      method: 'DELETE',
      headers: getAdminHeaders()
    });
    if (!res.ok) throw new Error('Failed to delete advisory');
  },

  // ── Route Segments ──────────────────────────────────────────────────────────
  async getAllRoutes(): Promise<AdminRouteSegmentDto[]> {
    const res = await fetch(`${API_BASE}/admin/routes`, {
      headers: getAdminHeaders()
    });
    if (!res.ok) throw new Error('Failed to load routes (Admin Authorization Required)');
    return res.json();
  },

  async createRoute(data: { originId: string; destinationId: string; distanceKm: number; avgDurationMinutes: number; isActive: boolean }): Promise<AdminRouteSegmentDto> {
    const res = await fetch(`${API_BASE}/admin/routes`, {
      method: 'POST',
      headers: getAdminHeaders(),
      body: JSON.stringify(data)
    });
    if (!res.ok) {
      const err = await res.json().catch(() => ({}));
      throw new Error(err.message || 'Failed to create route');
    }
    return res.json();
  },

  async updateRoute(id: string, data: { distanceKm: number; avgDurationMinutes: number; isActive: boolean }): Promise<AdminRouteSegmentDto> {
    const res = await fetch(`${API_BASE}/admin/routes/${id}`, {
      method: 'PUT',
      headers: getAdminHeaders(),
      body: JSON.stringify(data)
    });
    if (!res.ok) throw new Error('Failed to update route');
    return res.json();
  },

  async deleteRoute(id: string): Promise<void> {
    const res = await fetch(`${API_BASE}/admin/routes/${id}`, {
      method: 'DELETE',
      headers: getAdminHeaders()
    });
    if (!res.ok) throw new Error('Failed to delete route');
  },

  // ── Transport Options ───────────────────────────────────────────────────────
  async addTransportOption(data: Partial<TransportOptionDto> & { routeSegmentId: string }): Promise<TransportOptionDto> {
    const res = await fetch(`${API_BASE}/admin/routes/transport`, {
      method: 'POST',
      headers: getAdminHeaders(),
      body: JSON.stringify(data)
    });
    if (!res.ok) throw new Error('Failed to add transport option');
    return res.json();
  },

  async updateTransportOption(id: string, data: Partial<TransportOptionDto>): Promise<TransportOptionDto> {
    const res = await fetch(`${API_BASE}/admin/routes/transport/${id}`, {
      method: 'PUT',
      headers: getAdminHeaders(),
      body: JSON.stringify(data)
    });
    if (!res.ok) throw new Error('Failed to update transport option');
    return res.json();
  },

  async deleteTransportOption(id: string): Promise<void> {
    const res = await fetch(`${API_BASE}/admin/routes/transport/${id}`, {
      method: 'DELETE',
      headers: getAdminHeaders()
    });
    if (!res.ok) throw new Error('Failed to delete transport option');
  }
};
