import { useState, useEffect } from 'react';
import { SearchBar } from './components/SearchBar';
import { RouteView } from './components/RouteView';
import { DestinationGuide } from './components/DestinationGuide';
import { AdminPanel } from './components/AdminPanel';
import { travelApi } from './services/api';
import type { LocationDto, RouteSearchResultDto, DestinationDetailDto } from './types/travel';
import { Compass, Sparkles, Navigation, Settings } from 'lucide-react';

export function App() {
  const [isAdminView, setIsAdminView] = useState(false);
  const [isAuthenticated, setIsAuthenticated] = useState(() => {
    return localStorage.getItem('travelbd_admin_token') === 'travelbd-admin-secret-2026';
  });
  const [showAuthModal, setShowAuthModal] = useState(false);
  const [secretInput, setSecretInput] = useState('');
  const [authError, setAuthError] = useState('');

  const [destinations, setDestinations] = useState<LocationDto[]>([]);
  const [searchResult, setSearchResult] = useState<RouteSearchResultDto | null>(null);
  const [destinationGuide, setDestinationGuide] = useState<DestinationDetailDto | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  // Check URL query or hash for hidden admin route (e.g. /?admin=1 or #admin)
  useEffect(() => {
    const urlParams = new URLSearchParams(window.location.search);
    const hasAdminParam = urlParams.has('admin') || window.location.hash === '#admin';

    if (hasAdminParam) {
      if (isAuthenticated) {
        setIsAdminView(true);
      } else {
        setShowAuthModal(true);
      }
    }

    // Secret shortcut listener: Ctrl + Shift + A
    const handleKeyDown = (e: KeyboardEvent) => {
      if (e.ctrlKey && e.shiftKey && (e.key === 'A' || e.key === 'a')) {
        e.preventDefault();
        if (isAuthenticated) {
          setIsAdminView(prev => !prev);
        } else {
          setShowAuthModal(true);
        }
      }
    };

    window.addEventListener('keydown', handleKeyDown);
    return () => window.removeEventListener('keydown', handleKeyDown);
  }, [isAuthenticated]);

  // Load popular hubs initially and run default plan
  useEffect(() => {
    async function init() {
      try {
        const dests = await travelApi.getDestinations();
        setDestinations(dests);
        // Initial search from Dhaka to Bandarban
        handleSearch('Dhaka', 'Bandarban', 'recommended');
      } catch (err: any) {
        console.error(err);
      }
    }
    init();
  }, []);

  const handleSearch = async (from: string, to: string, pref: string) => {
    setLoading(true);
    setError(null);
    try {
      const result = await travelApi.searchRoutes(from, to, pref);
      setSearchResult(result);

      // Load destination guide
      if (result.destination?.slug) {
        try {
          const guide = await travelApi.getDestinationDetails(result.destination.slug);
          setDestinationGuide(guide);
        } catch {
          setDestinationGuide(null);
        }
      }
    } catch (err: any) {
      setError(err.message || 'Error occurred while calculating route');
      setSearchResult(null);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div style={{ minHeight: '100vh', display: 'flex', flexDirection: 'column' }}>
      
      {/* Top Navbar */}
      <header className="glass-nav" style={{ position: 'sticky', top: 0, zIndex: 50, padding: '0.9rem 2rem' }}>
        <div style={{ maxWidth: '1200px', margin: '0 auto', display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
          <div style={{ display: 'flex', alignItems: 'center', gap: '0.6rem' }}>
            <div
              onClick={() => {
                if (isAuthenticated) {
                  setIsAdminView(prev => !prev);
                } else {
                  setShowAuthModal(true);
                }
              }}
              title="GhurboBD"
              style={{
                width: '36px',
                height: '36px',
                borderRadius: 'var(--radius-sm)',
                background: 'linear-gradient(135deg, #059669 0%, #10b981 100%)',
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'center',
                boxShadow: '0 0 15px rgba(16, 185, 129, 0.4)',
                cursor: 'pointer'
              }}
            >
              <Compass size={22} color="#fff" />
            </div>
            <div>
              <span style={{ fontSize: '1.25rem', fontWeight: 800, letterSpacing: '-0.02em', color: '#fff' }}>
                Ghurbo<span style={{ color: 'var(--primary-light)' }}>BD</span>
              </span>
              <span style={{ fontSize: '0.7rem', color: 'var(--text-muted)', display: 'block', lineHeight: 1 }}>
                Smart Bangladesh Transit & Trip Planner
              </span>
            </div>
          </div>

          <div style={{ display: 'flex', gap: '0.75rem', alignItems: 'center' }}>
            {/* Admin toggle: Only shown if already logged in as admin */}
            {isAuthenticated && (
              <button
                onClick={() => setIsAdminView(!isAdminView)}
                style={{
                  display: 'flex',
                  alignItems: 'center',
                  gap: '0.4rem',
                  fontSize: '0.8rem',
                  padding: '0.35rem 0.85rem',
                  borderRadius: 'var(--radius-full)',
                  background: isAdminView ? 'var(--primary)' : 'rgba(255, 255, 255, 0.08)',
                  color: '#fff',
                  border: '1px solid var(--border)',
                  fontWeight: 600,
                  cursor: 'pointer'
                }}
              >
                <Settings size={14} /> {isAdminView ? 'Live App' : 'Operations'}
              </button>
            )}

            <span
              style={{
                fontSize: '0.75rem',
                padding: '0.25rem 0.65rem',
                borderRadius: 'var(--radius-full)',
                background: 'rgba(56, 189, 248, 0.12)',
                color: '#38bdf8',
                border: '1px solid rgba(56, 189, 248, 0.25)',
                fontWeight: 600
              }}
            >
              Southeastern Corridor Active
            </span>
          </div>
        </div>
      </header>

      {/* Admin Panel View */}
      {isAdminView ? (
        <AdminPanel onExit={() => setIsAdminView(false)} />
      ) : (
        /* Main Content Area */
        <main style={{ flex: 1, maxWidth: '1200px', width: '100%', margin: '0 auto', padding: '2rem 1.5rem' }}>
        
        {/* Hero Title */}
        <div style={{ textAlign: 'center', marginBottom: '2rem' }}>
          <div style={{ display: 'inline-flex', alignItems: 'center', gap: '0.4rem', color: 'var(--primary-light)', fontSize: '0.85rem', fontWeight: 600, marginBottom: '0.5rem' }}>
            <Sparkles size={16} /> Intelligent Multi-hop Transit Engine
          </div>
          <h1 style={{ fontSize: '2.5rem', fontWeight: 800, color: '#fff', letterSpacing: '-0.03em' }}>
            Plan Any Route to Bangladesh’s Best Escapes
          </h1>
          <p style={{ color: 'var(--text-muted)', fontSize: '1.05rem', maxWidth: '620px', margin: '0.5rem auto 0' }}>
            Compare Bus, Intercity Train, and Chander Gari fares, travel times, and army permit protocols.
          </p>
        </div>

        {/* Search Bar Component */}
        <SearchBar
          onSearch={(from, to, pref) => handleSearch(from, to, pref)}
          loading={loading}
        />

        {/* Quick Tourist Hub Selector Chips */}
        <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center', gap: '0.6rem', marginTop: '1.25rem', flexWrap: 'wrap' }}>
          <span style={{ fontSize: '0.75rem', color: 'var(--text-dim)', fontWeight: 600 }}>POPULAR HUBS:</span>
          {destinations.slice(0, 5).map((hub) => (
            <button
              key={hub.id}
              onClick={() => handleSearch('Dhaka', hub.name, 'recommended')}
              style={{
                background: 'rgba(255,255,255,0.04)',
                border: '1px solid var(--border)',
                borderRadius: 'var(--radius-full)',
                padding: '0.3rem 0.8rem',
                color: 'var(--text-muted)',
                fontSize: '0.8rem',
                cursor: 'pointer',
                display: 'flex',
                alignItems: 'center',
                gap: '0.3rem',
                transition: 'all 0.15s'
              }}
              onMouseEnter={(e) => {
                e.currentTarget.style.color = '#fff';
                e.currentTarget.style.borderColor = 'var(--primary-light)';
              }}
              onMouseLeave={(e) => {
                e.currentTarget.style.color = 'var(--text-muted)';
                e.currentTarget.style.borderColor = 'var(--border)';
              }}
            >
              <Navigation size={12} color="var(--primary-light)" />
              {hub.name}
            </button>
          ))}
        </div>

        {/* Error Notification */}
        {error && (
          <div
            className="glass-panel"
            style={{
              padding: '1rem',
              marginTop: '1.5rem',
              borderColor: 'var(--accent-rose)',
              background: 'rgba(244, 63, 94, 0.1)',
              color: '#fca5a5',
              textAlign: 'center'
            }}
          >
            {error}
          </div>
        )}

        {/* Search Results Route Itinerary View */}
        {searchResult && (
          <RouteView
            plans={searchResult.plans}
            originName={searchResult.origin?.name || 'Origin'}
            destinationName={searchResult.destination?.name || 'Destination'}
          />
        )}

        {/* Destination Guide (Attractions, Stays, Hill-Tract Advisories) */}
        {destinationGuide && (
          <DestinationGuide guide={destinationGuide} />
        )}
      </main>
      )}

      {/* Footer */}
      <footer style={{ borderTop: '1px solid var(--border)', padding: '1.5rem 2rem', textAlign: 'center', color: 'var(--text-dim)', fontSize: '0.85rem' }}>
        GhurboBD • Mini-Rome2rio for Bangladesh • Chittagong, Cox's Bazar, Bandarban & Rangamati
      </footer>

      {/* Secret Admin Authentication Modal */}
      {showAuthModal && (
        <div style={{ position: 'fixed', inset: 0, background: 'rgba(0,0,0,0.85)', backdropFilter: 'blur(8px)', display: 'flex', alignItems: 'center', justifyContent: 'center', zIndex: 1000, padding: '1rem' }}>
          <div style={{ background: '#0f172a', border: '1px solid var(--border)', borderRadius: 'var(--radius-lg)', width: '100%', maxWidth: '400px', padding: '1.75rem', boxShadow: '0 20px 25px -5px rgba(0, 0, 0, 0.5)' }}>
            <h3 style={{ margin: '0 0 0.5rem', fontSize: '1.2rem', color: '#fff', fontWeight: 700 }}>Management Authorization</h3>
            <p style={{ margin: '0 0 1.25rem', fontSize: '0.85rem', color: 'var(--text-muted)' }}>Enter your master deployment key to unlock system operations.</p>
            
            <form onSubmit={(e) => {
              e.preventDefault();
              if (secretInput === 'travelbd-admin-secret-2026') {
                localStorage.setItem('travelbd_admin_token', secretInput);
                setIsAuthenticated(true);
                setIsAdminView(true);
                setShowAuthModal(false);
                setSecretInput('');
                setAuthError('');
              } else {
                setAuthError('Invalid deployment key.');
              }
            }}>
              <input
                type="password"
                placeholder="Secret Access Key"
                autoFocus
                required
                value={secretInput}
                onChange={(e) => setSecretInput(e.target.value)}
                style={{ width: '100%', background: 'rgba(255,255,255,0.06)', border: authError ? '1px solid #f43f5e' : '1px solid var(--border)', padding: '0.65rem 0.85rem', borderRadius: 'var(--radius-md)', color: '#fff', fontSize: '0.9rem', marginBottom: '0.75rem' }}
              />

              {authError && (
                <div style={{ color: '#f87171', fontSize: '0.8rem', marginBottom: '0.75rem' }}>
                  {authError}
                </div>
              )}

              <div style={{ display: 'flex', justifyContent: 'flex-end', gap: '0.6rem' }}>
                <button
                  type="button"
                  onClick={() => { setShowAuthModal(false); setAuthError(''); setSecretInput(''); }}
                  style={{ background: 'rgba(255,255,255,0.06)', border: 'none', padding: '0.55rem 1rem', borderRadius: '4px', color: '#fff', cursor: 'pointer', fontSize: '0.85rem' }}
                >
                  Cancel
                </button>
                <button
                  type="submit"
                  className="btn-primary"
                  style={{ padding: '0.55rem 1.25rem', fontSize: '0.85rem' }}
                >
                  Authorize
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
}

export default App;
