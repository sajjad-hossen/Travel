import { useState, useEffect } from 'react';
import { adminApi, type AdminRouteSegmentDto } from '../services/api';
import type { LocationDto } from '../types/travel';
import {
  MapPin,
  Route,
  Bus,
  Sparkles,
  Plus,
  Trash2,
  Edit2,
  X,
  AlertTriangle,
  Hotel,
  Camera
} from 'lucide-react';

export function AdminPanel({ onExit }: { onExit: () => void }) {
  const [activeTab, setActiveTab] = useState<'locations' | 'routes' | 'guides'>('locations');
  const [locations, setLocations] = useState<LocationDto[]>([]);
  const [routes, setRoutes] = useState<AdminRouteSegmentDto[]>([]);
  const [loading, setLoading] = useState(false);
  const [statusMessage, setStatusMessage] = useState<{ text: string; type: 'success' | 'error' } | null>(null);

  // ── Form States ─────────────────────────────────────────────────────────────
  const [showLocationModal, setShowLocationModal] = useState(false);
  const [editingLocation, setEditingLocation] = useState<LocationDto | null>(null);
  const [locForm, setLocForm] = useState({
    name: '',
    banglaName: '',
    slug: '',
    type: 'TouristSpot',
    district: '',
    division: 'Chattogram',
    latitude: '',
    longitude: '',
    isMajorHub: false,
    isTouristDestination: true,
    description: '',
    heroImageUrl: ''
  });

  const [showRouteModal, setShowRouteModal] = useState(false);
  const [routeForm, setRouteForm] = useState({
    originId: '',
    destinationId: '',
    distanceKm: '',
    avgDurationMinutes: '',
    isActive: true
  });

  const [showTransportModal, setShowTransportModal] = useState<string | null>(null); // routeId
  const [transportForm, setTransportForm] = useState({
    mode: 'Bus',
    tier: 'AcBus',
    operatorName: '',
    minCostBdt: '',
    maxCostBdt: '',
    frequencyPerDay: '1',
    departureStation: '',
    arrivalStation: '',
    scheduleNotes: ''
  });

  // Selected location for guides tab
  const [selectedGuideLocation, setSelectedGuideLocation] = useState<string>('');
  const [attractionForm, setAttractionForm] = useState({ name: '', banglaName: '', category: 'Nature', entryFeeBdt: '0', description: '', bestTimeToVisit: '' });
  const [hotelForm, setHotelForm] = useState({ name: '', budgetLevel: 'MidRange', approxPriceRange: '', address: '', contactPhone: '', rating: '4.5' });
  const [advisoryForm, setAdvisoryForm] = useState({ category: 'Permits', title: '', content: '', isMandatory: true });

  const loadData = async () => {
    setLoading(true);
    try {
      const [locs, rts] = await Promise.all([adminApi.getAllLocations(), adminApi.getAllRoutes()]);
      setLocations(locs);
      setRoutes(rts);
      if (locs.length > 0 && !selectedGuideLocation) {
        setSelectedGuideLocation(locs[0].id);
      }
    } catch (err: any) {
      notify(err.message || 'Failed to fetch data', 'error');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadData();
  }, []);

  const notify = (text: string, type: 'success' | 'error') => {
    setStatusMessage({ text, type });
    setTimeout(() => setStatusMessage(null), 3500);
  };

  // ── Location Handlers ───────────────────────────────────────────────────────
  const handleSaveLocation = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const payload: any = {
        name: locForm.name,
        banglaName: locForm.banglaName || undefined,
        slug: locForm.slug || undefined,
        type: locForm.type,
        district: locForm.district,
        division: locForm.division,
        latitude: locForm.latitude ? parseFloat(locForm.latitude) : undefined,
        longitude: locForm.longitude ? parseFloat(locForm.longitude) : undefined,
        isMajorHub: locForm.isMajorHub,
        isTouristDestination: locForm.isTouristDestination,
        description: locForm.description || undefined,
        heroImageUrl: locForm.heroImageUrl || undefined
      };

      if (editingLocation) {
        await adminApi.updateLocation(editingLocation.id, payload);
        notify('Location updated successfully!', 'success');
      } else {
        await adminApi.createLocation(payload);
        notify('Location created successfully!', 'success');
      }
      setShowLocationModal(false);
      setEditingLocation(null);
      loadData();
    } catch (err: any) {
      notify(err.message || 'Failed to save location', 'error');
    }
  };

  const handleDeleteLocation = async (id: string, name: string) => {
    if (!confirm(`Are you sure you want to delete ${name}? This will remove related routes and guides.`)) return;
    try {
      await adminApi.deleteLocation(id);
      notify('Location deleted', 'success');
      loadData();
    } catch (err: any) {
      notify(err.message, 'error');
    }
  };

  const openEditLocation = (loc: LocationDto) => {
    setEditingLocation(loc);
    setLocForm({
      name: loc.name,
      banglaName: loc.banglaName || '',
      slug: loc.slug,
      type: loc.type,
      district: loc.district,
      division: loc.division,
      latitude: loc.latitude?.toString() || '',
      longitude: loc.longitude?.toString() || '',
      isMajorHub: loc.isMajorHub,
      isTouristDestination: loc.isTouristDestination,
      description: loc.description || '',
      heroImageUrl: loc.heroImageUrl || ''
    });
    setShowLocationModal(true);
  };

  // ── Route Handlers ──────────────────────────────────────────────────────────
  const handleSaveRoute = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await adminApi.createRoute({
        originId: routeForm.originId,
        destinationId: routeForm.destinationId,
        distanceKm: parseFloat(routeForm.distanceKm) || 10,
        avgDurationMinutes: parseInt(routeForm.avgDurationMinutes) || 60,
        isActive: routeForm.isActive
      });
      notify('Route segment created!', 'success');
      setShowRouteModal(false);
      loadData();
    } catch (err: any) {
      notify(err.message, 'error');
    }
  };

  const handleDeleteRoute = async (id: string) => {
    if (!confirm('Delete this route segment and its transport options?')) return;
    try {
      await adminApi.deleteRoute(id);
      notify('Route segment deleted', 'success');
      loadData();
    } catch (err: any) {
      notify(err.message, 'error');
    }
  };

  // ── Transport Handlers ──────────────────────────────────────────────────────
  const handleSaveTransport = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!showTransportModal) return;
    try {
      await adminApi.addTransportOption({
        routeSegmentId: showTransportModal,
        mode: transportForm.mode as any,
        tier: transportForm.tier,
        operatorName: transportForm.operatorName,
        minCostBdt: parseFloat(transportForm.minCostBdt) || 0,
        maxCostBdt: parseFloat(transportForm.maxCostBdt) || parseFloat(transportForm.minCostBdt) || 0,
        frequencyPerDay: parseInt(transportForm.frequencyPerDay) || 1,
        departureStation: transportForm.departureStation || undefined,
        arrivalStation: transportForm.arrivalStation || undefined,
        scheduleNotes: transportForm.scheduleNotes || undefined
      });
      notify('Transport operator added!', 'success');
      setShowTransportModal(null);
      loadData();
    } catch (err: any) {
      notify(err.message, 'error');
    }
  };

  const handleDeleteTransport = async (id: string) => {
    if (!confirm('Delete this transport option?')) return;
    try {
      await adminApi.deleteTransportOption(id);
      notify('Transport option removed', 'success');
      loadData();
    } catch (err: any) {
      notify(err.message, 'error');
    }
  };

  // ── Guide Handlers ──────────────────────────────────────────────────────────
  const handleAddAttraction = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!selectedGuideLocation) return;
    try {
      await adminApi.addAttraction({
        locationId: selectedGuideLocation,
        name: attractionForm.name,
        banglaName: attractionForm.banglaName || undefined,
        category: attractionForm.category,
        entryFeeBdt: parseFloat(attractionForm.entryFeeBdt) || 0,
        description: attractionForm.description,
        bestTimeToVisit: attractionForm.bestTimeToVisit || undefined
      });
      notify('Attraction added!', 'success');
      setAttractionForm({ name: '', banglaName: '', category: 'Nature', entryFeeBdt: '0', description: '', bestTimeToVisit: '' });
      loadData();
    } catch (err: any) {
      notify(err.message, 'error');
    }
  };

  const handleAddHotel = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!selectedGuideLocation) return;
    try {
      await adminApi.addAccommodation({
        locationId: selectedGuideLocation,
        name: hotelForm.name,
        budgetLevel: hotelForm.budgetLevel as any,
        approxPriceRange: hotelForm.approxPriceRange,
        address: hotelForm.address,
        contactPhone: hotelForm.contactPhone,
        rating: parseFloat(hotelForm.rating) || 4.5
      });
      notify('Accommodation added!', 'success');
      setHotelForm({ name: '', budgetLevel: 'MidRange', approxPriceRange: '', address: '', contactPhone: '', rating: '4.5' });
      loadData();
    } catch (err: any) {
      notify(err.message, 'error');
    }
  };

  const handleAddAdvisory = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!selectedGuideLocation) return;
    try {
      await adminApi.addAdvisory({
        locationId: selectedGuideLocation,
        category: advisoryForm.category,
        title: advisoryForm.title,
        content: advisoryForm.content,
        isMandatory: advisoryForm.isMandatory
      });
      notify('Advisory added!', 'success');
      setAdvisoryForm({ category: 'Permits', title: '', content: '', isMandatory: true });
      loadData();
    } catch (err: any) {
      notify(err.message, 'error');
    }
  };

  return (
    <div style={{ minHeight: '100vh', background: 'var(--bg-main)', color: '#fff' }}>
      {/* Top Banner */}
      <div style={{ background: 'rgba(15, 23, 42, 0.95)', borderBottom: '1px solid var(--border)', padding: '1rem 2rem', display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
        <div style={{ display: 'flex', alignItems: 'center', gap: '0.75rem' }}>
          <div style={{ width: '32px', height: '32px', borderRadius: '8px', background: 'linear-gradient(135deg, #f59e0b 0%, #d97706 100%)', display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
            <Sparkles size={18} color="#fff" />
          </div>
          <div>
            <h2 style={{ fontSize: '1.15rem', fontWeight: 800, margin: 0, display: 'flex', alignItems: 'center', gap: '0.5rem' }}>
              GhurboBD Data Operations
              {loading && <span style={{ fontSize: '0.75rem', color: 'var(--primary-light)', fontWeight: 500 }}>(Syncing...)</span>}
            </h2>
            <span style={{ fontSize: '0.75rem', color: 'var(--text-muted)' }}>Real-world destinations, routes & transport rates management</span>
          </div>
        </div>

        <button
          onClick={onExit}
          style={{
            background: 'rgba(255,255,255,0.06)',
            border: '1px solid var(--border)',
            borderRadius: 'var(--radius-md)',
            padding: '0.45rem 1rem',
            color: '#fff',
            cursor: 'pointer',
            fontSize: '0.85rem',
            fontWeight: 600
          }}
        >
          Back to Live App
        </button>
      </div>

      {/* Notifications */}
      {statusMessage && (
        <div
          style={{
            padding: '0.75rem 2rem',
            background: statusMessage.type === 'success' ? 'rgba(16, 185, 129, 0.2)' : 'rgba(244, 63, 94, 0.2)',
            color: statusMessage.type === 'success' ? '#6ee7b7' : '#fca5a5',
            borderBottom: '1px solid var(--border)',
            fontSize: '0.9rem',
            fontWeight: 600,
            textAlign: 'center'
          }}
        >
          {statusMessage.text}
        </div>
      )}

      {/* Navigation Tabs */}
      <div style={{ maxWidth: '1200px', margin: '1.5rem auto 0', padding: '0 1.5rem', display: 'flex', gap: '1rem' }}>
        <button
          onClick={() => setActiveTab('locations')}
          style={{
            background: activeTab === 'locations' ? 'var(--primary)' : 'rgba(255,255,255,0.04)',
            border: '1px solid var(--border)',
            padding: '0.6rem 1.25rem',
            borderRadius: 'var(--radius-md)',
            color: '#fff',
            fontWeight: 600,
            cursor: 'pointer',
            display: 'flex',
            alignItems: 'center',
            gap: '0.5rem'
          }}
        >
          <MapPin size={16} /> Locations ({locations.length})
        </button>

        <button
          onClick={() => setActiveTab('routes')}
          style={{
            background: activeTab === 'routes' ? 'var(--primary)' : 'rgba(255,255,255,0.04)',
            border: '1px solid var(--border)',
            padding: '0.6rem 1.25rem',
            borderRadius: 'var(--radius-md)',
            color: '#fff',
            fontWeight: 600,
            cursor: 'pointer',
            display: 'flex',
            alignItems: 'center',
            gap: '0.5rem'
          }}
        >
          <Route size={16} /> Routes & Transport ({routes.length})
        </button>

        <button
          onClick={() => setActiveTab('guides')}
          style={{
            background: activeTab === 'guides' ? 'var(--primary)' : 'rgba(255,255,255,0.04)',
            border: '1px solid var(--border)',
            padding: '0.6rem 1.25rem',
            borderRadius: 'var(--radius-md)',
            color: '#fff',
            fontWeight: 600,
            cursor: 'pointer',
            display: 'flex',
            alignItems: 'center',
            gap: '0.5rem'
          }}
        >
          <Camera size={16} /> Destination Guides
        </button>
      </div>

      {/* Tab Contents */}
      <div style={{ maxWidth: '1200px', margin: '1.5rem auto 3rem', padding: '0 1.5rem' }}>
        
        {/* ── TAB 1: LOCATIONS ───────────────────────────────────────────────── */}
        {activeTab === 'locations' && (
          <div>
            <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '1.25rem' }}>
              <h3 style={{ margin: 0, fontSize: '1.25rem', fontWeight: 700 }}>All Districts, Hubs & Spots</h3>
              <button
                onClick={() => {
                  setEditingLocation(null);
                  setLocForm({
                    name: '', banglaName: '', slug: '', type: 'TouristSpot', district: '', division: 'Chattogram',
                    latitude: '', longitude: '', isMajorHub: false, isTouristDestination: true, description: '', heroImageUrl: ''
                  });
                  setShowLocationModal(true);
                }}
                className="btn-primary"
                style={{ padding: '0.5rem 1rem', display: 'flex', alignItems: 'center', gap: '0.4rem', fontSize: '0.85rem' }}
              >
                <Plus size={16} /> Add Location
              </button>
            </div>

            <div style={{ background: 'rgba(255,255,255,0.02)', border: '1px solid var(--border)', borderRadius: 'var(--radius-lg)', overflow: 'hidden' }}>
              <table style={{ width: '100%', borderCollapse: 'collapse', textAlign: 'left', fontSize: '0.9rem' }}>
                <thead style={{ background: 'rgba(255,255,255,0.04)', borderBottom: '1px solid var(--border)', color: 'var(--text-muted)' }}>
                  <tr>
                    <th style={{ padding: '0.8rem 1rem' }}>Location Name</th>
                    <th style={{ padding: '0.8rem 1rem' }}>Bangla</th>
                    <th style={{ padding: '0.8rem 1rem' }}>Type</th>
                    <th style={{ padding: '0.8rem 1rem' }}>District</th>
                    <th style={{ padding: '0.8rem 1rem' }}>Flags</th>
                    <th style={{ padding: '0.8rem 1rem', textAlign: 'right' }}>Actions</th>
                  </tr>
                </thead>
                <tbody>
                  {locations.map((l) => (
                    <tr key={l.id} style={{ borderBottom: '1px solid rgba(255,255,255,0.05)' }}>
                      <td style={{ padding: '0.8rem 1rem', fontWeight: 600 }}>{l.name}</td>
                      <td style={{ padding: '0.8rem 1rem', color: 'var(--text-muted)' }}>{l.banglaName || '—'}</td>
                      <td style={{ padding: '0.8rem 1rem' }}>
                        <span style={{ fontSize: '0.75rem', background: 'rgba(255,255,255,0.08)', padding: '0.2rem 0.5rem', borderRadius: '4px' }}>
                          {l.type}
                        </span>
                      </td>
                      <td style={{ padding: '0.8rem 1rem', color: 'var(--text-muted)' }}>{l.district}</td>
                      <td style={{ padding: '0.8rem 1rem' }}>
                        {l.isTouristDestination && (
                          <span style={{ fontSize: '0.75rem', color: '#10b981', marginRight: '0.5rem' }}>★ Tourist</span>
                        )}
                        {l.isMajorHub && (
                          <span style={{ fontSize: '0.75rem', color: '#38bdf8' }}>Transit Hub</span>
                        )}
                      </td>
                      <td style={{ padding: '0.8rem 1rem', textAlign: 'right' }}>
                        <button
                          onClick={() => openEditLocation(l)}
                          style={{ background: 'none', border: 'none', color: 'var(--primary-light)', cursor: 'pointer', marginRight: '0.75rem' }}
                        >
                          <Edit2 size={16} />
                        </button>
                        <button
                          onClick={() => handleDeleteLocation(l.id, l.name)}
                          style={{ background: 'none', border: 'none', color: '#f43f5e', cursor: 'pointer' }}
                        >
                          <Trash2 size={16} />
                        </button>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          </div>
        )}

        {/* ── TAB 2: ROUTES & TRANSPORT ───────────────────────────────────────── */}
        {activeTab === 'routes' && (
          <div>
            <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '1.25rem' }}>
              <h3 style={{ margin: 0, fontSize: '1.25rem', fontWeight: 700 }}>Inter-district Route Segments</h3>
              <button
                onClick={() => {
                  setRouteForm({
                    originId: locations[0]?.id || '',
                    destinationId: locations[1]?.id || '',
                    distanceKm: '150',
                    avgDurationMinutes: '240',
                    isActive: true
                  });
                  setShowRouteModal(true);
                }}
                className="btn-primary"
                style={{ padding: '0.5rem 1rem', display: 'flex', alignItems: 'center', gap: '0.4rem', fontSize: '0.85rem' }}
              >
                <Plus size={16} /> Create Route Segment
              </button>
            </div>

            <div style={{ display: 'flex', flexDirection: 'column', gap: '1rem' }}>
              {routes.map((r) => (
                <div key={r.id} style={{ background: 'rgba(255,255,255,0.02)', border: '1px solid var(--border)', borderRadius: 'var(--radius-lg)', padding: '1.25rem' }}>
                  <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                    <div style={{ display: 'flex', alignItems: 'center', gap: '0.75rem' }}>
                      <span style={{ fontSize: '1.1rem', fontWeight: 700, color: '#fff' }}>
                        {r.originName} ➔ {r.destinationName}
                      </span>
                      <span style={{ fontSize: '0.75rem', background: 'rgba(56, 189, 248, 0.1)', color: '#38bdf8', padding: '0.2rem 0.5rem', borderRadius: '4px' }}>
                        {r.distanceKm} km • ~{Math.floor(r.avgDurationMinutes / 60)}h {r.avgDurationMinutes % 60}m
                      </span>
                    </div>

                    <div style={{ display: 'flex', gap: '0.5rem' }}>
                      <button
                        onClick={() => {
                          setShowTransportModal(r.id);
                          setTransportForm({
                            mode: 'Bus', tier: 'AcBus', operatorName: '', minCostBdt: '800', maxCostBdt: '1200',
                            frequencyPerDay: '4', departureStation: '', arrivalStation: '', scheduleNotes: ''
                          });
                        }}
                        style={{ background: 'rgba(255,255,255,0.06)', border: '1px solid var(--border)', borderRadius: 'var(--radius-md)', padding: '0.4rem 0.8rem', color: 'var(--primary-light)', fontSize: '0.8rem', cursor: 'pointer', display: 'flex', alignItems: 'center', gap: '0.3rem' }}
                      >
                        <Plus size={14} /> Add Operator
                      </button>
                      <button
                        onClick={() => handleDeleteRoute(r.id)}
                        style={{ background: 'none', border: 'none', color: '#f43f5e', cursor: 'pointer', padding: '0.4rem' }}
                      >
                        <Trash2 size={16} />
                      </button>
                    </div>
                  </div>

                  {/* Transport Services Table */}
                  <div style={{ marginTop: '0.75rem', borderTop: '1px solid rgba(255,255,255,0.06)', paddingTop: '0.75rem' }}>
                    {r.transportOptions.length === 0 ? (
                      <span style={{ fontSize: '0.8rem', color: 'var(--text-dim)' }}>No transport options configured for this segment yet.</span>
                    ) : (
                      <div style={{ display: 'flex', flexWrap: 'wrap', gap: '0.6rem' }}>
                        {r.transportOptions.map((t) => (
                          <div
                            key={t.id}
                            style={{
                              background: 'rgba(255,255,255,0.04)',
                              border: '1px solid rgba(255,255,255,0.08)',
                              borderRadius: 'var(--radius-md)',
                              padding: '0.5rem 0.8rem',
                              fontSize: '0.8rem',
                              display: 'flex',
                              alignItems: 'center',
                              gap: '0.6rem'
                            }}
                          >
                            <Bus size={14} color="var(--primary-light)" />
                            <div>
                              <strong style={{ color: '#fff' }}>{t.operatorName}</strong> ({t.mode} - {t.tier})
                              <div style={{ color: 'var(--text-muted)', fontSize: '0.75rem' }}>
                                ৳{t.minCostBdt} - ৳{t.maxCostBdt} • {t.frequencyPerDay}x daily
                              </div>
                            </div>
                            <button
                              onClick={() => handleDeleteTransport(t.id)}
                              style={{ background: 'none', border: 'none', color: '#f43f5e', cursor: 'pointer', marginLeft: '0.4rem' }}
                            >
                              <X size={14} />
                            </button>
                          </div>
                        ))}
                      </div>
                    )}
                  </div>
                </div>
              ))}
            </div>
          </div>
        )}

        {/* ── TAB 3: DESTINATION GUIDES ───────────────────────────────────────── */}
        {activeTab === 'guides' && (
          <div>
            <div style={{ display: 'flex', alignItems: 'center', gap: '1rem', marginBottom: '1.5rem' }}>
              <label style={{ fontWeight: 600, fontSize: '0.9rem' }}>Select Destination to Manage:</label>
              <select
                value={selectedGuideLocation}
                onChange={(e) => setSelectedGuideLocation(e.target.value)}
                style={{
                  background: 'rgba(255,255,255,0.06)',
                  border: '1px solid var(--border)',
                  color: '#fff',
                  padding: '0.5rem 1rem',
                  borderRadius: 'var(--radius-md)',
                  fontSize: '0.9rem'
                }}
              >
                {locations.filter(l => l.isTouristDestination).map((l) => (
                  <option key={l.id} value={l.id} style={{ background: '#0f172a' }}>
                    {l.name} ({l.district})
                  </option>
                ))}
              </select>
            </div>

            <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fit, minmax(320px, 1fr))', gap: '1.5rem' }}>
              
              {/* Attraction Form */}
              <div style={{ background: 'rgba(255,255,255,0.02)', border: '1px solid var(--border)', borderRadius: 'var(--radius-lg)', padding: '1.25rem' }}>
                <div style={{ display: 'flex', alignItems: 'center', gap: '0.5rem', marginBottom: '1rem' }}>
                  <Camera size={18} color="var(--primary-light)" />
                  <h4 style={{ margin: 0, fontSize: '1rem' }}>Add Attraction / Spot</h4>
                </div>
                <form onSubmit={handleAddAttraction} style={{ display: 'flex', flexDirection: 'column', gap: '0.75rem' }}>
                  <input
                    placeholder="Attraction Name (e.g. Nilgiri, Inani Beach)"
                    required
                    value={attractionForm.name}
                    onChange={(e) => setAttractionForm({ ...attractionForm, name: e.target.value })}
                    style={{ background: 'rgba(255,255,255,0.06)', border: '1px solid var(--border)', padding: '0.5rem', borderRadius: '4px', color: '#fff' }}
                  />
                  <input
                    placeholder="Bangla Name"
                    value={attractionForm.banglaName}
                    onChange={(e) => setAttractionForm({ ...attractionForm, banglaName: e.target.value })}
                    style={{ background: 'rgba(255,255,255,0.06)', border: '1px solid var(--border)', padding: '0.5rem', borderRadius: '4px', color: '#fff' }}
                  />
                  <div style={{ display: 'flex', gap: '0.5rem' }}>
                    <input
                      placeholder="Category (Nature, Beach)"
                      value={attractionForm.category}
                      onChange={(e) => setAttractionForm({ ...attractionForm, category: e.target.value })}
                      style={{ flex: 1, background: 'rgba(255,255,255,0.06)', border: '1px solid var(--border)', padding: '0.5rem', borderRadius: '4px', color: '#fff' }}
                    />
                    <input
                      placeholder="Entry Fee (BDT)"
                      type="number"
                      value={attractionForm.entryFeeBdt}
                      onChange={(e) => setAttractionForm({ ...attractionForm, entryFeeBdt: e.target.value })}
                      style={{ width: '120px', background: 'rgba(255,255,255,0.06)', border: '1px solid var(--border)', padding: '0.5rem', borderRadius: '4px', color: '#fff' }}
                    />
                  </div>
                  <textarea
                    placeholder="Brief description..."
                    rows={2}
                    value={attractionForm.description}
                    onChange={(e) => setAttractionForm({ ...attractionForm, description: e.target.value })}
                    style={{ background: 'rgba(255,255,255,0.06)', border: '1px solid var(--border)', padding: '0.5rem', borderRadius: '4px', color: '#fff' }}
                  />
                  <button type="submit" className="btn-primary" style={{ padding: '0.5rem' }}>Save Attraction</button>
                </form>
              </div>

              {/* Hotel / Stay Form */}
              <div style={{ background: 'rgba(255,255,255,0.02)', border: '1px solid var(--border)', borderRadius: 'var(--radius-lg)', padding: '1.25rem' }}>
                <div style={{ display: 'flex', alignItems: 'center', gap: '0.5rem', marginBottom: '1rem' }}>
                  <Hotel size={18} color="var(--primary-light)" />
                  <h4 style={{ margin: 0, fontSize: '1rem' }}>Add Accommodation</h4>
                </div>
                <form onSubmit={handleAddHotel} style={{ display: 'flex', flexDirection: 'column', gap: '0.75rem' }}>
                  <input
                    placeholder="Hotel / Resort Name"
                    required
                    value={hotelForm.name}
                    onChange={(e) => setHotelForm({ ...hotelForm, name: e.target.value })}
                    style={{ background: 'rgba(255,255,255,0.06)', border: '1px solid var(--border)', padding: '0.5rem', borderRadius: '4px', color: '#fff' }}
                  />
                  <div style={{ display: 'flex', gap: '0.5rem' }}>
                    <select
                      value={hotelForm.budgetLevel}
                      onChange={(e) => setHotelForm({ ...hotelForm, budgetLevel: e.target.value })}
                      style={{ flex: 1, background: 'rgba(255,255,255,0.06)', border: '1px solid var(--border)', padding: '0.5rem', borderRadius: '4px', color: '#fff' }}
                    >
                      <option value="Budget" style={{ background: '#0f172a' }}>Budget</option>
                      <option value="MidRange" style={{ background: '#0f172a' }}>Mid-Range</option>
                      <option value="Luxury" style={{ background: '#0f172a' }}>Luxury</option>
                      <option value="Resort" style={{ background: '#0f172a' }}>Resort</option>
                      <option value="EcoCottage" style={{ background: '#0f172a' }}>Eco Cottage</option>
                    </select>
                    <input
                      placeholder="Approx Price (৳1,500 - ৳3,000)"
                      value={hotelForm.approxPriceRange}
                      onChange={(e) => setHotelForm({ ...hotelForm, approxPriceRange: e.target.value })}
                      style={{ flex: 1, background: 'rgba(255,255,255,0.06)', border: '1px solid var(--border)', padding: '0.5rem', borderRadius: '4px', color: '#fff' }}
                    />
                  </div>
                  <input
                    placeholder="Contact Phone / Mobile"
                    value={hotelForm.contactPhone}
                    onChange={(e) => setHotelForm({ ...hotelForm, contactPhone: e.target.value })}
                    style={{ background: 'rgba(255,255,255,0.06)', border: '1px solid var(--border)', padding: '0.5rem', borderRadius: '4px', color: '#fff' }}
                  />
                  <input
                    placeholder="Address / Location"
                    value={hotelForm.address}
                    onChange={(e) => setHotelForm({ ...hotelForm, address: e.target.value })}
                    style={{ background: 'rgba(255,255,255,0.06)', border: '1px solid var(--border)', padding: '0.5rem', borderRadius: '4px', color: '#fff' }}
                  />
                  <button type="submit" className="btn-primary" style={{ padding: '0.5rem' }}>Save Hotel</button>
                </form>
              </div>

              {/* Advisory / Permit Form */}
              <div style={{ background: 'rgba(255,255,255,0.02)', border: '1px solid var(--border)', borderRadius: 'var(--radius-lg)', padding: '1.25rem' }}>
                <div style={{ display: 'flex', alignItems: 'center', gap: '0.5rem', marginBottom: '1rem' }}>
                  <AlertTriangle size={18} color="#f59e0b" />
                  <h4 style={{ margin: 0, fontSize: '1rem' }}>Add Travel Permit / Advisory</h4>
                </div>
                <form onSubmit={handleAddAdvisory} style={{ display: 'flex', flexDirection: 'column', gap: '0.75rem' }}>
                  <div style={{ display: 'flex', gap: '0.5rem' }}>
                    <input
                      placeholder="Category (Permits, Military Escort)"
                      value={advisoryForm.category}
                      onChange={(e) => setAdvisoryForm({ ...advisoryForm, category: e.target.value })}
                      style={{ flex: 1, background: 'rgba(255,255,255,0.06)', border: '1px solid var(--border)', padding: '0.5rem', borderRadius: '4px', color: '#fff' }}
                    />
                    <label style={{ display: 'flex', alignItems: 'center', gap: '0.3rem', fontSize: '0.8rem', color: 'var(--text-muted)' }}>
                      <input
                        type="checkbox"
                        checked={advisoryForm.isMandatory}
                        onChange={(e) => setAdvisoryForm({ ...advisoryForm, isMandatory: e.target.checked })}
                      />
                      Mandatory
                    </label>
                  </div>
                  <input
                    placeholder="Advisory Title"
                    required
                    value={advisoryForm.title}
                    onChange={(e) => setAdvisoryForm({ ...advisoryForm, title: e.target.value })}
                    style={{ background: 'rgba(255,255,255,0.06)', border: '1px solid var(--border)', padding: '0.5rem', borderRadius: '4px', color: '#fff' }}
                  />
                  <textarea
                    placeholder="Advisory details, timing, checkpost requirements..."
                    required
                    rows={3}
                    value={advisoryForm.content}
                    onChange={(e) => setAdvisoryForm({ ...advisoryForm, content: e.target.value })}
                    style={{ background: 'rgba(255,255,255,0.06)', border: '1px solid var(--border)', padding: '0.5rem', borderRadius: '4px', color: '#fff' }}
                  />
                  <button type="submit" className="btn-primary" style={{ padding: '0.5rem' }}>Save Advisory</button>
                </form>
              </div>

            </div>
          </div>
        )}

      </div>

      {/* ── MODALS ───────────────────────────────────────────────────────────── */}
      {/* Location Modal */}
      {showLocationModal && (
        <div style={{ position: 'fixed', inset: 0, background: 'rgba(0,0,0,0.7)', display: 'flex', alignItems: 'center', justifyContent: 'center', zIndex: 100, padding: '1rem' }}>
          <div style={{ background: '#0f172a', border: '1px solid var(--border)', borderRadius: 'var(--radius-lg)', width: '100%', maxWidth: '560px', padding: '1.5rem', maxHeight: '90vh', overflowY: 'auto' }}>
            <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '1rem' }}>
              <h3 style={{ margin: 0, fontSize: '1.15rem' }}>{editingLocation ? 'Edit Location' : 'Add New Location'}</h3>
              <button onClick={() => setShowLocationModal(false)} style={{ background: 'none', border: 'none', color: '#fff', cursor: 'pointer' }}><X size={20} /></button>
            </div>
            <form onSubmit={handleSaveLocation} style={{ display: 'flex', flexDirection: 'column', gap: '0.8rem' }}>
              <div style={{ display: 'flex', gap: '0.5rem' }}>
                <input placeholder="Name (e.g. Sreemangal)" required value={locForm.name} onChange={(e) => setLocForm({ ...locForm, name: e.target.value })} style={{ flex: 1, background: 'rgba(255,255,255,0.06)', border: '1px solid var(--border)', padding: '0.55rem', borderRadius: '4px', color: '#fff' }} />
                <input placeholder="Bangla Name" value={locForm.banglaName} onChange={(e) => setLocForm({ ...locForm, banglaName: e.target.value })} style={{ flex: 1, background: 'rgba(255,255,255,0.06)', border: '1px solid var(--border)', padding: '0.55rem', borderRadius: '4px', color: '#fff' }} />
              </div>
              <div style={{ display: 'flex', gap: '0.5rem' }}>
                <input placeholder="District (e.g. Moulvibazar)" required value={locForm.district} onChange={(e) => setLocForm({ ...locForm, district: e.target.value })} style={{ flex: 1, background: 'rgba(255,255,255,0.06)', border: '1px solid var(--border)', padding: '0.55rem', borderRadius: '4px', color: '#fff' }} />
                <input placeholder="Division (e.g. Sylhet)" required value={locForm.division} onChange={(e) => setLocForm({ ...locForm, division: e.target.value })} style={{ flex: 1, background: 'rgba(255,255,255,0.06)', border: '1px solid var(--border)', padding: '0.55rem', borderRadius: '4px', color: '#fff' }} />
              </div>
              <select value={locForm.type} onChange={(e) => setLocForm({ ...locForm, type: e.target.value })} style={{ background: 'rgba(255,255,255,0.06)', border: '1px solid var(--border)', padding: '0.55rem', borderRadius: '4px', color: '#fff' }}>
                <option value="TouristSpot" style={{ background: '#0f172a' }}>Tourist Spot</option>
                <option value="TransitHub" style={{ background: '#0f172a' }}>Transit Hub</option>
                <option value="District" style={{ background: '#0f172a' }}>District Capital</option>
                <option value="Upazila" style={{ background: '#0f172a' }}>Upazila</option>
              </select>
              <div style={{ display: 'flex', gap: '0.5rem' }}>
                <input placeholder="Latitude (optional)" type="number" step="any" value={locForm.latitude} onChange={(e) => setLocForm({ ...locForm, latitude: e.target.value })} style={{ flex: 1, background: 'rgba(255,255,255,0.06)', border: '1px solid var(--border)', padding: '0.55rem', borderRadius: '4px', color: '#fff' }} />
                <input placeholder="Longitude (optional)" type="number" step="any" value={locForm.longitude} onChange={(e) => setLocForm({ ...locForm, longitude: e.target.value })} style={{ flex: 1, background: 'rgba(255,255,255,0.06)', border: '1px solid var(--border)', padding: '0.55rem', borderRadius: '4px', color: '#fff' }} />
              </div>
              <input placeholder="Hero Image URL (Unsplash or direct image link)" value={locForm.heroImageUrl} onChange={(e) => setLocForm({ ...locForm, heroImageUrl: e.target.value })} style={{ background: 'rgba(255,255,255,0.06)', border: '1px solid var(--border)', padding: '0.55rem', borderRadius: '4px', color: '#fff' }} />
              <textarea placeholder="Description of the destination or hub..." rows={3} value={locForm.description} onChange={(e) => setLocForm({ ...locForm, description: e.target.value })} style={{ background: 'rgba(255,255,255,0.06)', border: '1px solid var(--border)', padding: '0.55rem', borderRadius: '4px', color: '#fff' }} />
              <div style={{ display: 'flex', gap: '1.5rem', marginTop: '0.5rem' }}>
                <label style={{ display: 'flex', alignItems: 'center', gap: '0.4rem', fontSize: '0.85rem' }}>
                  <input type="checkbox" checked={locForm.isTouristDestination} onChange={(e) => setLocForm({ ...locForm, isTouristDestination: e.target.checked })} />
                  Is Tourist Destination
                </label>
                <label style={{ display: 'flex', alignItems: 'center', gap: '0.4rem', fontSize: '0.85rem' }}>
                  <input type="checkbox" checked={locForm.isMajorHub} onChange={(e) => setLocForm({ ...locForm, isMajorHub: e.target.checked })} />
                  Is Major Transit Hub
                </label>
              </div>
              <div style={{ display: 'flex', justifyContent: 'flex-end', gap: '0.75rem', marginTop: '1rem' }}>
                <button type="button" onClick={() => setShowLocationModal(false)} style={{ background: 'rgba(255,255,255,0.06)', border: 'none', padding: '0.6rem 1.25rem', borderRadius: '4px', color: '#fff', cursor: 'pointer' }}>Cancel</button>
                <button type="submit" className="btn-primary" style={{ padding: '0.6rem 1.25rem' }}>Save Location</button>
              </div>
            </form>
          </div>
        </div>
      )}

      {/* Route Modal */}
      {showRouteModal && (
        <div style={{ position: 'fixed', inset: 0, background: 'rgba(0,0,0,0.7)', display: 'flex', alignItems: 'center', justifyContent: 'center', zIndex: 100, padding: '1rem' }}>
          <div style={{ background: '#0f172a', border: '1px solid var(--border)', borderRadius: 'var(--radius-lg)', width: '100%', maxWidth: '480px', padding: '1.5rem' }}>
            <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '1rem' }}>
              <h3 style={{ margin: 0, fontSize: '1.15rem' }}>Create Route Segment</h3>
              <button onClick={() => setShowRouteModal(false)} style={{ background: 'none', border: 'none', color: '#fff', cursor: 'pointer' }}><X size={20} /></button>
            </div>
            <form onSubmit={handleSaveRoute} style={{ display: 'flex', flexDirection: 'column', gap: '0.8rem' }}>
              <div>
                <label style={{ fontSize: '0.8rem', color: 'var(--text-muted)' }}>Origin Hub:</label>
                <select value={routeForm.originId} onChange={(e) => setRouteForm({ ...routeForm, originId: e.target.value })} style={{ width: '100%', background: 'rgba(255,255,255,0.06)', border: '1px solid var(--border)', padding: '0.55rem', borderRadius: '4px', color: '#fff', marginTop: '0.2rem' }}>
                  {locations.map((l) => (
                    <option key={l.id} value={l.id} style={{ background: '#0f172a' }}>{l.name} ({l.district})</option>
                  ))}
                </select>
              </div>
              <div>
                <label style={{ fontSize: '0.8rem', color: 'var(--text-muted)' }}>Destination Hub:</label>
                <select value={routeForm.destinationId} onChange={(e) => setRouteForm({ ...routeForm, destinationId: e.target.value })} style={{ width: '100%', background: 'rgba(255,255,255,0.06)', border: '1px solid var(--border)', padding: '0.55rem', borderRadius: '4px', color: '#fff', marginTop: '0.2rem' }}>
                  {locations.map((l) => (
                    <option key={l.id} value={l.id} style={{ background: '#0f172a' }}>{l.name} ({l.district})</option>
                  ))}
                </select>
              </div>
              <div style={{ display: 'flex', gap: '0.5rem' }}>
                <input placeholder="Distance (km)" type="number" step="any" required value={routeForm.distanceKm} onChange={(e) => setRouteForm({ ...routeForm, distanceKm: e.target.value })} style={{ flex: 1, background: 'rgba(255,255,255,0.06)', border: '1px solid var(--border)', padding: '0.55rem', borderRadius: '4px', color: '#fff' }} />
                <input placeholder="Duration (min)" type="number" required value={routeForm.avgDurationMinutes} onChange={(e) => setRouteForm({ ...routeForm, avgDurationMinutes: e.target.value })} style={{ flex: 1, background: 'rgba(255,255,255,0.06)', border: '1px solid var(--border)', padding: '0.55rem', borderRadius: '4px', color: '#fff' }} />
              </div>
              <div style={{ display: 'flex', justifyContent: 'flex-end', gap: '0.75rem', marginTop: '1rem' }}>
                <button type="button" onClick={() => setShowRouteModal(false)} style={{ background: 'rgba(255,255,255,0.06)', border: 'none', padding: '0.6rem 1.25rem', borderRadius: '4px', color: '#fff', cursor: 'pointer' }}>Cancel</button>
                <button type="submit" className="btn-primary" style={{ padding: '0.6rem 1.25rem' }}>Create Route</button>
              </div>
            </form>
          </div>
        </div>
      )}

      {/* Transport Modal */}
      {showTransportModal && (
        <div style={{ position: 'fixed', inset: 0, background: 'rgba(0,0,0,0.7)', display: 'flex', alignItems: 'center', justifyContent: 'center', zIndex: 100, padding: '1rem' }}>
          <div style={{ background: '#0f172a', border: '1px solid var(--border)', borderRadius: 'var(--radius-lg)', width: '100%', maxWidth: '480px', padding: '1.5rem' }}>
            <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '1rem' }}>
              <h3 style={{ margin: 0, fontSize: '1.15rem' }}>Add Transport Operator</h3>
              <button onClick={() => setShowTransportModal(null)} style={{ background: 'none', border: 'none', color: '#fff', cursor: 'pointer' }}><X size={20} /></button>
            </div>
            <form onSubmit={handleSaveTransport} style={{ display: 'flex', flexDirection: 'column', gap: '0.8rem' }}>
              <input placeholder="Operator Name (e.g. Green Line, Subarna Express)" required value={transportForm.operatorName} onChange={(e) => setTransportForm({ ...transportForm, operatorName: e.target.value })} style={{ background: 'rgba(255,255,255,0.06)', border: '1px solid var(--border)', padding: '0.55rem', borderRadius: '4px', color: '#fff' }} />
              <div style={{ display: 'flex', gap: '0.5rem' }}>
                <select value={transportForm.mode} onChange={(e) => setTransportForm({ ...transportForm, mode: e.target.value })} style={{ flex: 1, background: 'rgba(255,255,255,0.06)', border: '1px solid var(--border)', padding: '0.55rem', borderRadius: '4px', color: '#fff' }}>
                  <option value="Bus" style={{ background: '#0f172a' }}>Bus</option>
                  <option value="Train" style={{ background: '#0f172a' }}>Train</option>
                  <option value="Flight" style={{ background: '#0f172a' }}>Flight</option>
                  <option value="Launch" style={{ background: '#0f172a' }}>Launch</option>
                  <option value="ChanderGari" style={{ background: '#0f172a' }}>Chander Gari</option>
                  <option value="Boat" style={{ background: '#0f172a' }}>Boat</option>
                </select>
                <select value={transportForm.tier} onChange={(e) => setTransportForm({ ...transportForm, tier: e.target.value })} style={{ flex: 1, background: 'rgba(255,255,255,0.06)', border: '1px solid var(--border)', padding: '0.55rem', borderRadius: '4px', color: '#fff' }}>
                  <option value="AcBus" style={{ background: '#0f172a' }}>AC Bus</option>
                  <option value="NonAcBus" style={{ background: '#0f172a' }}>Non-AC Bus</option>
                  <option value="SleeperBus" style={{ background: '#0f172a' }}>Sleeper Coach</option>
                  <option value="ShovonChair" style={{ background: '#0f172a' }}>Shovon Chair (Train)</option>
                  <option value="Snigdha" style={{ background: '#0f172a' }}>Snigdha AC (Train)</option>
                  <option value="EconomyAir" style={{ background: '#0f172a' }}>Economy Air</option>
                  <option value="ReservedVehicle" style={{ background: '#0f172a' }}>Reserved 4x4</option>
                </select>
              </div>
              <div style={{ display: 'flex', gap: '0.5rem' }}>
                <input placeholder="Min Fare (BDT)" type="number" required value={transportForm.minCostBdt} onChange={(e) => setTransportForm({ ...transportForm, minCostBdt: e.target.value })} style={{ flex: 1, background: 'rgba(255,255,255,0.06)', border: '1px solid var(--border)', padding: '0.55rem', borderRadius: '4px', color: '#fff' }} />
                <input placeholder="Max Fare (BDT)" type="number" value={transportForm.maxCostBdt} onChange={(e) => setTransportForm({ ...transportForm, maxCostBdt: e.target.value })} style={{ flex: 1, background: 'rgba(255,255,255,0.06)', border: '1px solid var(--border)', padding: '0.55rem', borderRadius: '4px', color: '#fff' }} />
                <input placeholder="Daily Trips" type="number" value={transportForm.frequencyPerDay} onChange={(e) => setTransportForm({ ...transportForm, frequencyPerDay: e.target.value })} style={{ width: '90px', background: 'rgba(255,255,255,0.06)', border: '1px solid var(--border)', padding: '0.55rem', borderRadius: '4px', color: '#fff' }} />
              </div>
              <div style={{ display: 'flex', gap: '0.5rem' }}>
                <input placeholder="Departure Station (e.g. Sayedabad, Kamalapur)" value={transportForm.departureStation} onChange={(e) => setTransportForm({ ...transportForm, departureStation: e.target.value })} style={{ flex: 1, background: 'rgba(255,255,255,0.06)', border: '1px solid var(--border)', padding: '0.55rem', borderRadius: '4px', color: '#fff' }} />
                <input placeholder="Arrival Station" value={transportForm.arrivalStation} onChange={(e) => setTransportForm({ ...transportForm, arrivalStation: e.target.value })} style={{ flex: 1, background: 'rgba(255,255,255,0.06)', border: '1px solid var(--border)', padding: '0.55rem', borderRadius: '4px', color: '#fff' }} />
              </div>
              <input placeholder="Schedule Notes (e.g. Departs every 45 min)" value={transportForm.scheduleNotes} onChange={(e) => setTransportForm({ ...transportForm, scheduleNotes: e.target.value })} style={{ background: 'rgba(255,255,255,0.06)', border: '1px solid var(--border)', padding: '0.55rem', borderRadius: '4px', color: '#fff' }} />
              <div style={{ display: 'flex', justifyContent: 'flex-end', gap: '0.75rem', marginTop: '1rem' }}>
                <button type="button" onClick={() => setShowTransportModal(null)} style={{ background: 'rgba(255,255,255,0.06)', border: 'none', padding: '0.6rem 1.25rem', borderRadius: '4px', color: '#fff', cursor: 'pointer' }}>Cancel</button>
                <button type="submit" className="btn-primary" style={{ padding: '0.6rem 1.25rem' }}>Save Operator</button>
              </div>
            </form>
          </div>
        </div>
      )}

    </div>
  );
}
