import { useState, type FC } from 'react';
import type { DestinationDetailDto } from '../types/travel';
import { ShieldAlert, Compass, Bed, AlertCircle, Phone, Star, MapPin, Info, ExternalLink } from 'lucide-react';

interface DestinationGuideProps {
  guide: DestinationDetailDto;
  onSelectAttraction?: (id: string) => void;
}

export const DestinationGuide: FC<DestinationGuideProps> = ({ guide, onSelectAttraction }) => {
  const [activeTab, setActiveTab] = useState<'tips' | 'attractions' | 'stays'>('tips');

  const { location, attractions, accommodations, advisories } = guide;

  return (
    <div style={{ marginTop: '2.5rem' }} className="animate-fade-in">
      {/* Destination Hero Banner */}
      <div
        style={{
          position: 'relative',
          borderRadius: 'var(--radius-lg)',
          overflow: 'hidden',
          minHeight: '260px',
          display: 'flex',
          flexDirection: 'column',
          justifyContent: 'flex-end',
          padding: '2rem',
          backgroundImage: `linear-gradient(to top, rgba(15, 23, 42, 0.95) 10%, rgba(15, 23, 42, 0.4) 60%, rgba(15,23,42,0.1)), url(${location.heroImageUrl})`,
          backgroundSize: 'cover',
          backgroundPosition: 'center',
          boxShadow: '0 20px 40px -15px rgba(0,0,0,0.8)',
          border: '1px solid var(--border)'
        }}
      >
        <div style={{ display: 'flex', alignItems: 'center', gap: '0.6rem' }}>
          <span
            style={{
              background: 'var(--primary)',
              color: '#fff',
              fontSize: '0.75rem',
              fontWeight: 700,
              padding: '0.2rem 0.6rem',
              borderRadius: 'var(--radius-full)',
              textTransform: 'uppercase'
            }}
          >
            {location.district} District
          </span>
          {location.banglaName && (
            <span style={{ color: 'var(--text-muted)', fontSize: '1.1rem', fontWeight: 600 }}>
              {location.banglaName}
            </span>
          )}
        </div>
        <h2 style={{ fontSize: '2.2rem', fontWeight: 800, color: '#fff', marginTop: '0.3rem' }}>
          {location.name}
        </h2>
        <p style={{ color: '#cbd5e1', maxWidth: '750px', fontSize: '0.95rem', marginTop: '0.5rem', lineHeight: 1.6 }}>
          {location.description}
        </p>
      </div>

      {/* Guide Navigation Tabs */}
      <div style={{ display: 'flex', gap: '1rem', marginTop: '1.5rem', borderBottom: '1px solid var(--border)', paddingBottom: '0.75rem' }}>
        <button
          onClick={() => setActiveTab('tips')}
          style={{
            background: 'transparent',
            border: 'none',
            color: activeTab === 'tips' ? 'var(--primary-light)' : 'var(--text-muted)',
            fontSize: '1rem',
            fontWeight: 700,
            cursor: 'pointer',
            display: 'flex',
            alignItems: 'center',
            gap: '0.5rem',
            paddingBottom: '0.5rem',
            borderBottom: activeTab === 'tips' ? '2px solid var(--primary-light)' : '2px solid transparent'
          }}
        >
          <ShieldAlert size={18} />
          Hill-Tracts & Local Tips ({advisories.length})
        </button>

        <button
          onClick={() => setActiveTab('attractions')}
          style={{
            background: 'transparent',
            border: 'none',
            color: activeTab === 'attractions' ? 'var(--primary-light)' : 'var(--text-muted)',
            fontSize: '1rem',
            fontWeight: 700,
            cursor: 'pointer',
            display: 'flex',
            alignItems: 'center',
            gap: '0.5rem',
            paddingBottom: '0.5rem',
            borderBottom: activeTab === 'attractions' ? '2px solid var(--primary-light)' : '2px solid transparent'
          }}
        >
          <Compass size={18} />
          Attractions & Sightseeing ({attractions.length})
        </button>

        <button
          onClick={() => setActiveTab('stays')}
          style={{
            background: 'transparent',
            border: 'none',
            color: activeTab === 'stays' ? 'var(--primary-light)' : 'var(--text-muted)',
            fontSize: '1rem',
            fontWeight: 700,
            cursor: 'pointer',
            display: 'flex',
            alignItems: 'center',
            gap: '0.5rem',
            paddingBottom: '0.5rem',
            borderBottom: activeTab === 'stays' ? '2px solid var(--primary-light)' : '2px solid transparent'
          }}
        >
          <Bed size={18} />
          Where to Stay ({accommodations.length})
        </button>
      </div>

      {/* TAB CONTENT: ADVISORIES */}
      {activeTab === 'tips' && (
        <div style={{ marginTop: '1.25rem', display: 'grid', gridTemplateColumns: 'repeat(auto-fit, minmax(320px, 1fr))', gap: '1.25rem' }}>
          {advisories.map((adv) => (
            <div
              key={adv.id}
              className="glass-panel"
              style={{
                padding: '1.25rem',
                borderLeft: adv.isMandatory ? '4px solid var(--accent-rose)' : '4px solid var(--primary)',
                background: adv.isMandatory ? 'rgba(244, 63, 94, 0.06)' : 'var(--bg-card)'
              }}
            >
              <div style={{ display: 'flex', alignItems: 'center', gap: '0.5rem', marginBottom: '0.5rem' }}>
                {adv.isMandatory ? (
                  <AlertCircle size={18} color="var(--accent-rose)" />
                ) : (
                  <Info size={18} color="var(--primary-light)" />
                )}
                <span
                  style={{
                    fontSize: '0.75rem',
                    fontWeight: 700,
                    textTransform: 'uppercase',
                    color: adv.isMandatory ? 'var(--accent-rose)' : 'var(--primary-light)'
                  }}
                >
                  {adv.category} {adv.isMandatory && '• MANDATORY'}
                </span>
              </div>
              <h4 style={{ fontSize: '1.05rem', fontWeight: 700, color: 'var(--text-heading)' }}>{adv.title}</h4>
              <p style={{ fontSize: '0.88rem', color: 'var(--text-muted)', marginTop: '0.4rem', lineHeight: 1.5 }}>
                {adv.content}
              </p>
            </div>
          ))}
        </div>
      )}

      {/* TAB CONTENT: ATTRACTIONS */}
      {activeTab === 'attractions' && (
        <div style={{ marginTop: '1.25rem', display: 'grid', gridTemplateColumns: 'repeat(auto-fill, minmax(280px, 1fr))', gap: '1.25rem' }}>
          {attractions.map((spot) => (
            <div
              key={spot.id}
              className="glass-panel"
              onClick={() => onSelectAttraction?.(spot.id)}
              style={{ overflow: 'hidden', display: 'flex', flexDirection: 'column', cursor: onSelectAttraction ? 'pointer' : 'default' }}
            >
              <div
                style={{
                  height: '160px',
                  backgroundImage: `url(${spot.imageUrl})`,
                  backgroundSize: 'cover',
                  backgroundPosition: 'center',
                  position: 'relative'
                }}
              >
                {spot.category && (
                  <span
                    style={{
                      position: 'absolute',
                      top: '10px',
                      left: '10px',
                      background: 'rgba(15, 23, 42, 0.8)',
                      backdropFilter: 'blur(8px)',
                      color: '#fff',
                      fontSize: '0.7rem',
                      fontWeight: 600,
                      padding: '0.2rem 0.5rem',
                      borderRadius: '4px'
                    }}
                  >
                    {spot.category}
                  </span>
                )}
              </div>
              <div style={{ padding: '1.2rem', flex: 1, display: 'flex', flexDirection: 'column' }}>
                <h4 style={{ fontSize: '1.1rem', fontWeight: 700, color: 'var(--text-heading)' }}>{spot.name}</h4>
                {spot.banglaName && (
                  <div style={{ fontSize: '0.85rem', color: 'var(--text-muted)' }}>{spot.banglaName}</div>
                )}
                <p style={{ fontSize: '0.85rem', color: 'var(--text-muted)', marginTop: '0.5rem', flex: 1, lineHeight: 1.5 }}>
                  {spot.description}
                </p>
                {spot.reviewCount > 0 && (
                  <div style={{ display: 'flex', alignItems: 'center', gap: '0.3rem', marginTop: '0.5rem' }}>
                    <Star size={14} color="#f59e0b" fill="#f59e0b" />
                    <span style={{ fontSize: '0.85rem', fontWeight: 700, color: '#f59e0b' }}>{spot.averageRating}</span>
                    <span style={{ fontSize: '0.75rem', color: 'var(--text-dim)' }}>({spot.reviewCount} reviews)</span>
                  </div>
                )}
                {spot.bestTimeToVisit && (
                  <div style={{ fontSize: '0.78rem', color: 'var(--accent-amber)', marginTop: '0.75rem', fontWeight: 500 }}>
                    Best Time: {spot.bestTimeToVisit}
                  </div>
                )}
              </div>
            </div>
          ))}
        </div>
      )}

      {/* TAB CONTENT: STAYS */}
      {activeTab === 'stays' && (
        <div style={{ marginTop: '1.25rem', display: 'grid', gridTemplateColumns: 'repeat(auto-fill, minmax(300px, 1fr))', gap: '1.25rem' }}>
          {accommodations.map((stay) => (
            <div key={stay.id} className="glass-panel" style={{ overflow: 'hidden', display: 'flex', flexDirection: 'column' }}>
              {stay.imageUrl ? (
                <div
                  style={{
                    height: '170px',
                    width: '100%',
                    position: 'relative',
                    overflow: 'hidden',
                    background: '#0f172a'
                  }}
                >
                  <img
                    src={stay.imageUrl}
                    alt={stay.name}
                    referrerPolicy="no-referrer"
                    crossOrigin="anonymous"
                    style={{
                      width: '100%',
                      height: '100%',
                      objectFit: 'cover',
                      display: 'block'
                    }}
                    onError={(e) => {
                      // Fallback if image still fails to load
                      const target = e.currentTarget;
                      target.style.display = 'none';
                      if (target.parentElement) {
                        target.parentElement.innerHTML = `
                          <div style="height: 100%; display: flex; align-items: center; justify-content: center; background: rgba(30, 41, 59, 0.9);">
                            <span style="font-size: 0.8rem; color: #94a3b8;">🏨 ${stay.name}</span>
                          </div>
                        `;
                      }
                    }}
                  />
                  <span
                    style={{
                      position: 'absolute',
                      top: '10px',
                      left: '10px',
                      background: 'rgba(15, 23, 42, 0.85)',
                      backdropFilter: 'blur(8px)',
                      color: '#fff',
                      fontSize: '0.7rem',
                      fontWeight: 600,
                      padding: '0.2rem 0.5rem',
                      borderRadius: '4px',
                      zIndex: 2
                    }}
                  >
                    {stay.budgetLevel}
                  </span>
                </div>
              ) : (
                <div
                  style={{
                    height: '110px',
                    background: 'var(--bg-subtle)',
                    display: 'flex',
                    alignItems: 'center',
                    justifyContent: 'center',
                    position: 'relative',
                    borderBottom: '1px solid var(--border)'
                  }}
                >
                  <Bed size={32} color="var(--primary-light)" style={{ opacity: 0.6 }} />
                  <span
                    style={{
                      position: 'absolute',
                      top: '10px',
                      left: '10px',
                      background: 'rgba(15, 23, 42, 0.85)',
                      backdropFilter: 'blur(8px)',
                      color: '#fff',
                      fontSize: '0.7rem',
                      fontWeight: 600,
                      padding: '0.2rem 0.5rem',
                      borderRadius: '4px'
                    }}
                  >
                    {stay.budgetLevel}
                  </span>
                </div>
              )}

              <div style={{ padding: '1.25rem', flex: 1, display: 'flex', flexDirection: 'column' }}>
                <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start', gap: '0.5rem' }}>
                  <div>
                    <h4 style={{ fontSize: '1.1rem', fontWeight: 700, color: 'var(--text-heading)', margin: 0 }}>{stay.name}</h4>
                    <div style={{ display: 'flex', alignItems: 'center', gap: '0.4rem', marginTop: '0.25rem' }}>
                      <Star size={14} color="#f59e0b" fill="#f59e0b" />
                      <span style={{ fontSize: '0.85rem', fontWeight: 700, color: '#f59e0b' }}>{stay.rating}</span>
                      <span style={{ fontSize: '0.75rem', color: 'var(--text-dim)' }}>• {stay.budgetLevel}</span>
                    </div>
                  </div>
                  <div style={{ textAlign: 'right' }}>
                    <div style={{ fontSize: '0.95rem', fontWeight: 700, color: 'var(--primary-light)' }}>
                      {stay.approxPriceRange}
                    </div>
                  </div>
                </div>

                {stay.highlightFeature && (
                  <div style={{ fontSize: '0.82rem', color: '#93c5fd', marginTop: '0.75rem', background: 'rgba(59, 130, 246, 0.1)', padding: '0.4rem 0.6rem', borderRadius: '4px' }}>
                    ✨ {stay.highlightFeature}
                  </div>
                )}

                {stay.address && (
                  <div style={{ marginTop: '0.75rem', fontSize: '0.82rem', color: 'var(--text-muted)', display: 'flex', alignItems: 'center', gap: '0.4rem' }}>
                    <MapPin size={14} /> {stay.address}
                  </div>
                )}

                {stay.contactPhone && (
                  <div style={{ marginTop: '0.4rem', fontSize: '0.82rem', color: 'var(--text-muted)', display: 'flex', alignItems: 'center', gap: '0.4rem' }}>
                    <Phone size={14} /> {stay.contactPhone}
                  </div>
                )}

                {stay.bookingUrl && (
                  <div style={{ marginTop: '1rem', paddingTop: '0.85rem', borderTop: '1px solid var(--border)' }}>
                    <a
                      href={stay.bookingUrl}
                      target="_blank"
                      rel="noopener noreferrer"
                      className="btn-book-stay"
                    >
                      <span>Book or View Hotel</span>
                      <ExternalLink size={15} style={{ opacity: 0.9 }} />
                    </a>
                  </div>
                )}
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};
