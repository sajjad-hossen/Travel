import { useState, useEffect, type FC } from 'react';
import { attractionApi, feedbackApi } from '../services/api';
import type { AttractionDetailDto, AuthUser } from '../types/travel';
import { AttractionMap } from './AttractionMap';
import { ArrowLeft, Star, MapPin, Clock, Navigation2, Ticket } from 'lucide-react';

interface AttractionDetailProps {
  attractionId: string;
  onBack: () => void;
  currentUser: AuthUser | null;
  onRequireLogin: () => void;
}

export const AttractionDetail: FC<AttractionDetailProps> = ({ attractionId, onBack, currentUser, onRequireLogin }) => {
  const [data, setData] = useState<AttractionDetailDto | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [activeImage, setActiveImage] = useState<string | undefined>(undefined);

  const [reviewRating, setReviewRating] = useState(5);
  const [reviewComment, setReviewComment] = useState('');
  const [submitting, setSubmitting] = useState(false);
  const [submitError, setSubmitError] = useState('');

  const load = async () => {
    setLoading(true);
    setError(null);
    try {
      const detail = await attractionApi.getById(attractionId);
      setData(detail);
      setActiveImage(detail.imageUrl || detail.galleryImages[0]);
    } catch (err: any) {
      setError(err.message || 'Failed to load attraction');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    load();
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }, [attractionId]);

  const handleSubmitReview = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!currentUser) {
      onRequireLogin();
      return;
    }
    setSubmitting(true);
    setSubmitError('');
    try {
      await feedbackApi.create({ attractionId, rating: reviewRating, comment: reviewComment.trim() });
      setReviewComment('');
      setReviewRating(5);
      const refreshed = await attractionApi.getById(attractionId);
      setData(refreshed);
    } catch (err: any) {
      setSubmitError(err.message || 'Failed to submit review');
    } finally {
      setSubmitting(false);
    }
  };

  if (loading) {
    return (
      <main style={{ flex: 1, maxWidth: '1000px', width: '100%', margin: '0 auto', padding: '2rem 1.5rem', color: 'var(--text-muted)' }}>
        Loading attraction...
      </main>
    );
  }

  if (error || !data) {
    return (
      <main style={{ flex: 1, maxWidth: '1000px', width: '100%', margin: '0 auto', padding: '2rem 1.5rem' }}>
        <button onClick={onBack} style={{ background: 'none', border: 'none', color: 'var(--primary-light)', cursor: 'pointer', display: 'flex', alignItems: 'center', gap: '0.4rem', marginBottom: '1rem' }}>
          <ArrowLeft size={16} /> Back
        </button>
        <div style={{ color: '#fca5a5' }}>{error || 'Attraction not found.'}</div>
      </main>
    );
  }

  const gallery = [data.imageUrl, ...data.galleryImages].filter((u): u is string => !!u);

  return (
    <main style={{ flex: 1, maxWidth: '1000px', width: '100%', margin: '0 auto', padding: '2rem 1.5rem' }} className="animate-fade-in">
      <button onClick={onBack} style={{ background: 'none', border: 'none', color: 'var(--primary-light)', cursor: 'pointer', display: 'flex', alignItems: 'center', gap: '0.4rem', marginBottom: '1.25rem', fontWeight: 600 }}>
        <ArrowLeft size={16} /> Back to {data.locationName}
      </button>

      {/* Cover photo */}
      {activeImage && (
        <div
          style={{
            height: '340px',
            borderRadius: 'var(--radius-lg)',
            overflow: 'hidden',
            backgroundImage: `url(${activeImage})`,
            backgroundSize: 'cover',
            backgroundPosition: 'center',
            border: '1px solid var(--border)'
          }}
        />
      )}

      {/* Gallery thumbnails */}
      {gallery.length > 1 && (
        <div style={{ display: 'flex', gap: '0.6rem', marginTop: '0.75rem', overflowX: 'auto' }}>
          {gallery.map((url, i) => (
            <img
              key={i}
              src={url}
              onClick={() => setActiveImage(url)}
              style={{
                width: '90px',
                height: '64px',
                objectFit: 'cover',
                borderRadius: '6px',
                cursor: 'pointer',
                border: activeImage === url ? '2px solid var(--primary-light)' : '2px solid transparent',
                flexShrink: 0
              }}
            />
          ))}
        </div>
      )}

      {/* Title + rating */}
      <div style={{ marginTop: '1.5rem', display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start', flexWrap: 'wrap', gap: '0.75rem' }}>
        <div>
          <h1 style={{ fontSize: '2rem', fontWeight: 800, color: 'var(--text-heading)', margin: 0 }}>{data.name}</h1>
          {data.banglaName && <div style={{ color: 'var(--text-muted)', fontSize: '1rem' }}>{data.banglaName}</div>}
          {data.category && (
            <span style={{ display: 'inline-block', marginTop: '0.5rem', fontSize: '0.75rem', background: 'rgba(16,185,129,0.15)', color: 'var(--primary-light)', padding: '0.2rem 0.6rem', borderRadius: 'var(--radius-full)', fontWeight: 600 }}>
              {data.category}
            </span>
          )}
        </div>
        <div style={{ textAlign: 'right' }}>
          <div style={{ display: 'flex', alignItems: 'center', gap: '0.4rem', justifyContent: 'flex-end' }}>
            <Star size={20} color="#f59e0b" fill="#f59e0b" />
            <span style={{ fontSize: '1.3rem', fontWeight: 800, color: '#f59e0b' }}>
              {data.reviewCount > 0 ? data.averageRating : '—'}
            </span>
          </div>
          <div style={{ fontSize: '0.8rem', color: 'var(--text-dim)' }}>{data.reviewCount} review{data.reviewCount === 1 ? '' : 's'}</div>
        </div>
      </div>

      {data.description && (
        <p style={{ color: 'var(--text-muted)', marginTop: '0.75rem', lineHeight: 1.6 }}>{data.description}</p>
      )}

      {/* Travel info cards */}
      <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fit, minmax(200px, 1fr))', gap: '1rem', marginTop: '1.5rem' }}>
        {data.distanceFromTownKm != null && (
          <div className="glass-panel" style={{ padding: '1rem', display: 'flex', gap: '0.75rem', alignItems: 'center' }}>
            <MapPin size={22} color="var(--primary-light)" />
            <div>
              <div style={{ fontSize: '0.75rem', color: 'var(--text-dim)' }}>Distance from {data.locationName}</div>
              <div style={{ fontWeight: 700, color: 'var(--text-heading)' }}>{data.distanceFromTownKm} km</div>
            </div>
          </div>
        )}
        {data.travelTimeMinutes != null && (
          <div className="glass-panel" style={{ padding: '1rem', display: 'flex', gap: '0.75rem', alignItems: 'center' }}>
            <Clock size={22} color="var(--primary-light)" />
            <div>
              <div style={{ fontSize: '0.75rem', color: 'var(--text-dim)' }}>Travel Time</div>
              <div style={{ fontWeight: 700, color: 'var(--text-heading)' }}>
                {Math.floor(data.travelTimeMinutes / 60) > 0 ? `${Math.floor(data.travelTimeMinutes / 60)}h ` : ''}{data.travelTimeMinutes % 60}m
              </div>
            </div>
          </div>
        )}
        {data.entryFeeBdt > 0 && (
          <div className="glass-panel" style={{ padding: '1rem', display: 'flex', gap: '0.75rem', alignItems: 'center' }}>
            <Ticket size={22} color="var(--primary-light)" />
            <div>
              <div style={{ fontSize: '0.75rem', color: 'var(--text-dim)' }}>Entry Fee</div>
              <div style={{ fontWeight: 700, color: 'var(--text-heading)' }}>৳{data.entryFeeBdt}</div>
            </div>
          </div>
        )}
      </div>

      {data.howToReach && (
        <div className="glass-panel" style={{ padding: '1.1rem', marginTop: '1rem', display: 'flex', gap: '0.75rem' }}>
          <Navigation2 size={20} color="var(--accent-amber)" style={{ flexShrink: 0, marginTop: '0.1rem' }} />
          <div>
            <div style={{ fontSize: '0.8rem', fontWeight: 700, color: 'var(--accent-amber)', textTransform: 'uppercase', marginBottom: '0.3rem' }}>How to Reach</div>
            <div style={{ color: 'var(--text-muted)', lineHeight: 1.5 }}>{data.howToReach}</div>
          </div>
        </div>
      )}

      {/* Map */}
      {data.latitude != null && data.longitude != null && (
        <div style={{ marginTop: '1.5rem' }}>
          <h3 style={{ color: 'var(--text-heading)', fontSize: '1.05rem', marginBottom: '0.6rem' }}>Location Map</h3>
          <AttractionMap
            attractionLat={data.latitude}
            attractionLng={data.longitude}
            attractionName={data.name}
            townLat={data.locationLatitude}
            townLng={data.locationLongitude}
            townName={data.locationName}
          />
        </div>
      )}

      {/* Reviews */}
      <div style={{ marginTop: '2.5rem' }}>
        <h3 style={{ color: 'var(--text-heading)', fontSize: '1.2rem', marginBottom: '1rem' }}>Reviews ({data.reviewCount})</h3>

        {/* Submit review form */}
        <div className="glass-panel" style={{ padding: '1.25rem', marginBottom: '1.25rem' }}>
          {currentUser ? (
            <form onSubmit={handleSubmitReview} style={{ display: 'flex', flexDirection: 'column', gap: '0.75rem' }}>
              <div style={{ display: 'flex', alignItems: 'center', gap: '0.3rem' }}>
                {[1, 2, 3, 4, 5].map((n) => (
                  <Star
                    key={n}
                    size={24}
                    color="#f59e0b"
                    fill={n <= reviewRating ? '#f59e0b' : 'none'}
                    style={{ cursor: 'pointer' }}
                    onClick={() => setReviewRating(n)}
                  />
                ))}
              </div>
              <textarea
                placeholder="Share your experience..."
                required
                rows={3}
                value={reviewComment}
                onChange={(e) => setReviewComment(e.target.value)}
                style={{ background: 'var(--bg-input)', border: '1px solid var(--border)', padding: '0.6rem', borderRadius: '4px', color: 'var(--text-main)', fontSize: '0.9rem' }}
              />
              {submitError && <div style={{ color: '#fca5a5', fontSize: '0.85rem' }}>{submitError}</div>}
              <button type="submit" disabled={submitting} className="btn-primary" style={{ alignSelf: 'flex-start', padding: '0.5rem 1.25rem', opacity: submitting ? 0.7 : 1 }}>
                {submitting ? 'Submitting...' : 'Submit Review'}
              </button>
            </form>
          ) : (
            <div style={{ textAlign: 'center', padding: '0.5rem' }}>
              <p style={{ color: 'var(--text-muted)', marginBottom: '0.75rem' }}>Sign in to leave a rating and review.</p>
              <button onClick={onRequireLogin} className="btn-primary" style={{ padding: '0.5rem 1.25rem' }}>Sign In</button>
            </div>
          )}
        </div>

        {/* Review list */}
        <div style={{ display: 'flex', flexDirection: 'column', gap: '1rem' }}>
          {data.reviews.length === 0 && (
            <div style={{ color: 'var(--text-dim)', fontSize: '0.9rem' }}>No reviews yet. Be the first to share your experience!</div>
          )}
          {data.reviews.map((r) => (
            <div key={r.id} className="glass-panel" style={{ padding: '1rem' }}>
              <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                <span style={{ fontWeight: 700, color: 'var(--text-heading)' }}>{r.userName}</span>
                <div style={{ display: 'flex', alignItems: 'center', gap: '0.2rem' }}>
                  {[1, 2, 3, 4, 5].map((n) => (
                    <Star key={n} size={14} color="#f59e0b" fill={n <= r.rating ? '#f59e0b' : 'none'} />
                  ))}
                </div>
              </div>
              <p style={{ color: 'var(--text-muted)', marginTop: '0.4rem', lineHeight: 1.5 }}>{r.comment}</p>
              <div style={{ fontSize: '0.75rem', color: 'var(--text-dim)', marginTop: '0.4rem' }}>
                {new Date(r.createdAt).toLocaleDateString()}
              </div>
            </div>
          ))}
        </div>
      </div>
    </main>
  );
};
