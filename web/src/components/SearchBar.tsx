import { useState, useEffect, useRef, type FC, type FormEvent } from 'react';
import { Search, MapPin, ArrowRightLeft, Sparkles } from 'lucide-react';
import type { LocationDto } from '../types/travel';
import { travelApi } from '../services/api';

interface SearchBarProps {
  onSearch: (from: string, to: string, pref: string) => void;
  initialFrom?: string;
  initialTo?: string;
  loading?: boolean;
}

export const SearchBar: FC<SearchBarProps> = ({
  onSearch,
  initialFrom = 'Dhaka',
  initialTo = 'Bandarban',
  loading = false
}) => {
  const [fromQuery, setFromQuery] = useState(initialFrom);
  const [toQuery, setToQuery] = useState(initialTo);
  const [preference, setPreference] = useState<'recommended' | 'fastest' | 'cheapest'>('recommended');

  const [fromSuggestions, setFromSuggestions] = useState<LocationDto[]>([]);
  const [toSuggestions, setToSuggestions] = useState<LocationDto[]>([]);
  const [activeDropdown, setActiveDropdown] = useState<'from' | 'to' | null>(null);

  const containerRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    const handleClickOutside = (e: MouseEvent) => {
      if (containerRef.current && !containerRef.current.contains(e.target as Node)) {
        setActiveDropdown(null);
      }
    };
    document.addEventListener('mousedown', handleClickOutside);
    return () => document.removeEventListener('mousedown', handleClickOutside);
  }, []);

  const handleFetchSuggestions = async (val: string, field: 'from' | 'to') => {
    try {
      const results = await travelApi.searchLocations(val);
      if (field === 'from') setFromSuggestions(results);
      else setToSuggestions(results);
    } catch {
      // ignore
    }
  };

  const handleSwap = () => {
    setFromQuery(toQuery);
    setToQuery(fromQuery);
  };

  const handleSubmit = (e: FormEvent) => {
    e.preventDefault();
    if (!fromQuery.trim() || !toQuery.trim()) return;
    setActiveDropdown(null);
    onSearch(fromQuery.trim(), toQuery.trim(), preference);
  };

  return (
    <div ref={containerRef} style={{ width: '100%', maxWidth: '860px', margin: '0 auto' }}>
      <form
        onSubmit={handleSubmit}
        className="glass-panel"
        style={{
          padding: '1.25rem',
          boxShadow: '0 20px 40px -15px rgba(0,0,0,0.7)',
          border: '1px solid rgba(255,255,255,0.12)'
        }}
      >
        <div style={{ display: 'flex', flexDirection: 'row', gap: '0.75rem', alignItems: 'center', flexWrap: 'wrap' }}>
          
          {/* Origin Input */}
          <div style={{ flex: '1 1 240px', position: 'relative' }}>
            <label style={{ fontSize: '0.75rem', color: 'var(--text-muted)', textTransform: 'uppercase', letterSpacing: '0.05em', fontWeight: 600 }}>
              From (Origin)
            </label>
            <div style={{ display: 'flex', alignItems: 'center', background: 'rgba(15,23,42,0.6)', border: '1px solid var(--border)', borderRadius: 'var(--radius-sm)', padding: '0.6rem 0.8rem', marginTop: '0.25rem' }}>
              <MapPin size={18} color="var(--primary-light)" style={{ marginRight: '0.5rem', flexShrink: 0 }} />
              <input
                type="text"
                placeholder="e.g. Dhaka, Rajshahi, Sylhet..."
                value={fromQuery}
                onFocus={() => {
                  setActiveDropdown('from');
                  handleFetchSuggestions(fromQuery, 'from');
                }}
                onChange={(e) => {
                  setFromQuery(e.target.value);
                  handleFetchSuggestions(e.target.value, 'from');
                }}
                style={{
                  background: 'transparent',
                  border: 'none',
                  outline: 'none',
                  color: '#fff',
                  width: '100%',
                  fontSize: '0.95rem',
                  fontWeight: 500
                }}
              />
            </div>

            {/* Suggestions Dropdown */}
            {activeDropdown === 'from' && fromSuggestions.length > 0 && (
              <div
                className="glass-panel"
                style={{
                  position: 'absolute',
                  top: '105%',
                  left: 0,
                  right: 0,
                  zIndex: 40,
                  maxHeight: '220px',
                  overflowY: 'auto',
                  padding: '0.4rem',
                  background: '#0f172a'
                }}
              >
                {fromSuggestions.map((loc) => (
                  <div
                    key={loc.id}
                    onClick={() => {
                      setFromQuery(loc.name);
                      setActiveDropdown(null);
                    }}
                    style={{
                      padding: '0.5rem 0.75rem',
                      borderRadius: 'var(--radius-sm)',
                      cursor: 'pointer',
                      fontSize: '0.9rem',
                      display: 'flex',
                      justifyContent: 'space-between',
                      alignItems: 'center',
                      transition: 'background 0.15s'
                    }}
                    onMouseEnter={(e) => (e.currentTarget.style.background = 'rgba(255,255,255,0.06)')}
                    onMouseLeave={(e) => (e.currentTarget.style.background = 'transparent')}
                  >
                    <span>
                      <strong>{loc.name}</strong> {loc.banglaName ? `(${loc.banglaName})` : ''}
                    </span>
                    <span style={{ fontSize: '0.75rem', color: 'var(--text-muted)' }}>{loc.district}</span>
                  </div>
                ))}
              </div>
            )}
          </div>

          {/* Swap Button */}
          <button
            type="button"
            onClick={handleSwap}
            title="Swap Origin and Destination"
            style={{
              background: 'rgba(255,255,255,0.06)',
              border: '1px solid var(--border)',
              borderRadius: '50%',
              width: '40px',
              height: '40px',
              display: 'flex',
              alignItems: 'center',
              justifyContent: 'center',
              cursor: 'pointer',
              color: 'var(--text-muted)',
              marginTop: '1.2rem',
              alignSelf: 'center',
              transition: 'all 0.2s'
            }}
            onMouseEnter={(e) => {
              e.currentTarget.style.color = '#fff';
              e.currentTarget.style.borderColor = 'var(--primary)';
              e.currentTarget.style.transform = 'rotate(180deg)';
            }}
            onMouseLeave={(e) => {
              e.currentTarget.style.color = 'var(--text-muted)';
              e.currentTarget.style.borderColor = 'var(--border)';
              e.currentTarget.style.transform = 'rotate(0deg)';
            }}
          >
            <ArrowRightLeft size={16} />
          </button>

          {/* Destination Input */}
          <div style={{ flex: '1 1 240px', position: 'relative' }}>
            <label style={{ fontSize: '0.75rem', color: 'var(--text-muted)', textTransform: 'uppercase', letterSpacing: '0.05em', fontWeight: 600 }}>
              To (Destination Hub)
            </label>
            <div style={{ display: 'flex', alignItems: 'center', background: 'rgba(15,23,42,0.6)', border: '1px solid var(--border)', borderRadius: 'var(--radius-sm)', padding: '0.6rem 0.8rem', marginTop: '0.25rem' }}>
              <MapPin size={18} color="var(--accent-amber)" style={{ marginRight: '0.5rem', flexShrink: 0 }} />
              <input
                type="text"
                placeholder="e.g. Cox's Bazar, Bandarban, Rangamati..."
                value={toQuery}
                onFocus={() => {
                  setActiveDropdown('to');
                  handleFetchSuggestions(toQuery, 'to');
                }}
                onChange={(e) => {
                  setToQuery(e.target.value);
                  handleFetchSuggestions(e.target.value, 'to');
                }}
                style={{
                  background: 'transparent',
                  border: 'none',
                  outline: 'none',
                  color: '#fff',
                  width: '100%',
                  fontSize: '0.95rem',
                  fontWeight: 500
                }}
              />
            </div>

            {/* Suggestions Dropdown */}
            {activeDropdown === 'to' && toSuggestions.length > 0 && (
              <div
                className="glass-panel"
                style={{
                  position: 'absolute',
                  top: '105%',
                  left: 0,
                  right: 0,
                  zIndex: 40,
                  maxHeight: '220px',
                  overflowY: 'auto',
                  padding: '0.4rem',
                  background: '#0f172a'
                }}
              >
                {toSuggestions.map((loc) => (
                  <div
                    key={loc.id}
                    onClick={() => {
                      setToQuery(loc.name);
                      setActiveDropdown(null);
                    }}
                    style={{
                      padding: '0.5rem 0.75rem',
                      borderRadius: 'var(--radius-sm)',
                      cursor: 'pointer',
                      fontSize: '0.9rem',
                      display: 'flex',
                      justifyContent: 'space-between',
                      alignItems: 'center'
                    }}
                    onMouseEnter={(e) => (e.currentTarget.style.background = 'rgba(255,255,255,0.06)')}
                    onMouseLeave={(e) => (e.currentTarget.style.background = 'transparent')}
                  >
                    <span>
                      <strong>{loc.name}</strong> {loc.banglaName ? `(${loc.banglaName})` : ''}
                    </span>
                    <span style={{ fontSize: '0.75rem', color: 'var(--text-muted)' }}>{loc.district}</span>
                  </div>
                ))}
              </div>
            )}
          </div>

          {/* Search Button */}
          <div style={{ alignSelf: 'flex-end', marginTop: '0.25rem' }}>
            <button
              type="submit"
              disabled={loading}
              style={{
                background: 'linear-gradient(135deg, #059669 0%, #10b981 100%)',
                color: '#fff',
                border: 'none',
                borderRadius: 'var(--radius-sm)',
                padding: '0.65rem 1.6rem',
                fontSize: '0.95rem',
                fontWeight: 600,
                cursor: 'pointer',
                display: 'flex',
                alignItems: 'center',
                gap: '0.5rem',
                boxShadow: '0 4px 15px rgba(16, 185, 129, 0.35)',
                transition: 'transform 0.15s, opacity 0.15s',
                opacity: loading ? 0.7 : 1
              }}
              onMouseEnter={(e) => (e.currentTarget.style.transform = 'translateY(-1px)')}
              onMouseLeave={(e) => (e.currentTarget.style.transform = 'translateY(0)')}
            >
              <Search size={18} />
              {loading ? 'Finding routes...' : 'Search Plan'}
            </button>
          </div>
        </div>

        {/* Filter Pills */}
        <div style={{ display: 'flex', gap: '0.5rem', marginTop: '1rem', alignItems: 'center' }}>
          <span style={{ fontSize: '0.75rem', color: 'var(--text-dim)', marginRight: '0.25rem' }}>Preference:</span>
          {(['recommended', 'fastest', 'cheapest'] as const).map((pref) => (
            <button
              key={pref}
              type="button"
              onClick={() => {
                setPreference(pref);
                if (fromQuery && toQuery) onSearch(fromQuery, toQuery, pref);
              }}
              style={{
                background: preference === pref ? 'rgba(16,185,129,0.18)' : 'rgba(255,255,255,0.04)',
                color: preference === pref ? 'var(--primary-light)' : 'var(--text-muted)',
                border: `1px solid ${preference === pref ? 'var(--primary)' : 'var(--border)'}`,
                borderRadius: 'var(--radius-full)',
                padding: '0.25rem 0.75rem',
                fontSize: '0.75rem',
                fontWeight: 600,
                cursor: 'pointer',
                textTransform: 'capitalize',
                transition: 'all 0.15s'
              }}
            >
              {pref === 'recommended' && <Sparkles size={12} style={{ display: 'inline', marginRight: '4px' }} />}
              {pref}
            </button>
          ))}
        </div>
      </form>
    </div>
  );
};
