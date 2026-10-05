import { useState, type FC } from 'react';
import type { RoutePlanDto } from '../types/travel';
import { Bus, Train, Plane, Clock, Banknote, ChevronDown, ChevronUp, ShieldCheck } from 'lucide-react';

interface RouteViewProps {
  plans: RoutePlanDto[];
  originName: string;
  destinationName: string;
}

export const RouteView: FC<RouteViewProps> = ({ plans, originName, destinationName }) => {
  const [selectedPlanIndex, setSelectedPlanIndex] = useState(0);
  const [expandedLegIndex, setExpandedLegIndex] = useState<number | null>(null);

  if (!plans || plans.length === 0) {
    return (
      <div className="glass-panel" style={{ padding: '2.5rem', textAlign: 'center', marginTop: '2rem' }}>
        <p style={{ color: 'var(--text-muted)', fontSize: '1.1rem' }}>
          No direct or multi-hop path found connecting <strong>{originName}</strong> to <strong>{destinationName}</strong> yet.
        </p>
        <p style={{ fontSize: '0.9rem', color: 'var(--text-dim)', marginTop: '0.5rem' }}>
          Try searching via a major transit hub like Dhaka or Chittagong.
        </p>
      </div>
    );
  }

  const activePlan = plans[selectedPlanIndex] || plans[0];

  const formatHours = (mins: number) => {
    const h = Math.floor(mins / 60);
    const m = mins % 60;
    if (h === 0) return `${m}m`;
    if (m === 0) return `${h}h`;
    return `${h}h ${m}m`;
  };

  const getTransportIcon = (mode: string) => {
    switch (mode?.toLowerCase()) {
      case 'train': return <Train size={18} color="#38bdf8" />;
      case 'flight': return <Plane size={18} color="#f472b6" />;
      case 'chandergari': return <ShieldCheck size={18} color="#fbbf24" />;
      default: return <Bus size={18} color="#34d399" />;
    }
  };

  return (
    <div style={{ marginTop: '2rem' }} className="animate-fade-in">
      
      {/* Route Plans Switcher Tabs */}
      <div style={{ display: 'flex', gap: '0.75rem', overflowX: 'auto', paddingBottom: '0.5rem' }}>
        {plans.map((plan, idx) => {
          const isSelected = selectedPlanIndex === idx;
          return (
            <button
              key={idx}
              onClick={() => {
                setSelectedPlanIndex(idx);
                setExpandedLegIndex(null);
              }}
              className="glass-panel"
              style={{
                flex: '1 0 220px',
                padding: '1rem',
                cursor: 'pointer',
                textAlign: 'left',
                border: isSelected ? '1px solid var(--primary-light)' : '1px solid var(--border)',
                background: isSelected ? 'rgba(5, 150, 105, 0.15)' : 'var(--bg-card)',
                transition: 'all 0.2s',
                boxShadow: isSelected ? '0 0 20px rgba(16, 185, 129, 0.2)' : 'none'
              }}
            >
              <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                <span style={{ fontSize: '0.8rem', fontWeight: 700, color: isSelected ? 'var(--primary-light)' : 'var(--text-muted)' }}>
                  {plan.planType}
                </span>
                <span style={{ fontSize: '0.7rem', padding: '0.15rem 0.4rem', borderRadius: '4px', background: 'rgba(255,255,255,0.06)' }}>
                  {plan.totalHops === 1 ? 'Direct' : `${plan.totalHops} Segments`}
                </span>
              </div>

              <div style={{ display: 'flex', gap: '1rem', marginTop: '0.6rem' }}>
                <div style={{ display: 'flex', alignItems: 'center', gap: '0.3rem', fontSize: '0.85rem' }}>
                  <Clock size={14} color="var(--text-muted)" />
                  <strong>{formatHours(plan.totalDurationMinutes)}</strong>
                </div>
                <div style={{ display: 'flex', alignItems: 'center', gap: '0.3rem', fontSize: '0.85rem' }}>
                  <Banknote size={14} color="var(--accent-amber)" />
                  <span style={{ color: 'var(--accent-amber)', fontWeight: 600 }}>৳{plan.totalMinCostBdt} - ৳{plan.totalMaxCostBdt}</span>
                </div>
              </div>
            </button>
          );
        })}
      </div>

      {/* Selected Route Detailed Itinerary */}
      <div className="glass-panel" style={{ padding: '1.75rem', marginTop: '1.25rem' }}>
        <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', borderBottom: '1px solid var(--border)', paddingBottom: '1rem' }}>
          <div>
            <h3 style={{ fontSize: '1.25rem', fontWeight: 700 }}>
              {originName} <span style={{ color: 'var(--text-muted)' }}>→</span> {destinationName}
            </h3>
            <p style={{ fontSize: '0.85rem', color: 'var(--text-muted)', marginTop: '0.2rem' }}>
              Total Journey: {formatHours(activePlan.totalDurationMinutes)} · Estimated Fare: ৳{activePlan.totalMinCostBdt} - ৳{activePlan.totalMaxCostBdt}
            </p>
          </div>
        </div>

        {/* Steps Timeline */}
        <div style={{ marginTop: '1.5rem', display: 'flex', flexDirection: 'column', gap: '1.5rem' }}>
          {activePlan.steps.map((step, sIdx) => {
            const isExpanded = expandedLegIndex === sIdx;
            const primaryMode = step.transportOptions[0]?.mode || 'Bus';

            return (
              <div
                key={step.stepNumber}
                style={{
                  background: 'rgba(15, 23, 42, 0.45)',
                  border: '1px solid var(--border)',
                  borderRadius: 'var(--radius-md)',
                  padding: '1.25rem'
                }}
              >
                {/* Leg Header */}
                <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', flexWrap: 'wrap', gap: '0.5rem' }}>
                  <div style={{ display: 'flex', alignItems: 'center', gap: '0.75rem' }}>
                    <div
                      style={{
                        width: '36px',
                        height: '36px',
                        borderRadius: '50%',
                        background: 'rgba(255,255,255,0.06)',
                        display: 'flex',
                        alignItems: 'center',
                        justifyContent: 'center'
                      }}
                    >
                      {getTransportIcon(primaryMode)}
                    </div>
                    <div>
                      <div style={{ fontSize: '0.75rem', color: 'var(--text-dim)', textTransform: 'uppercase', fontWeight: 600 }}>
                        Leg {step.stepNumber} ({step.distanceKm} km)
                      </div>
                      <div style={{ fontSize: '1.05rem', fontWeight: 600 }}>
                        {step.originName} <span style={{ color: 'var(--text-muted)' }}>→</span> {step.destinationName}
                      </div>
                    </div>
                  </div>

                  <div style={{ display: 'flex', alignItems: 'center', gap: '1.25rem' }}>
                    <div style={{ textAlign: 'right' }}>
                      <div style={{ fontSize: '0.85rem', fontWeight: 600 }}>~{formatHours(step.avgDurationMinutes)}</div>
                      <div style={{ fontSize: '0.75rem', color: 'var(--text-muted)' }}>
                        {step.transportOptions.length} transport {step.transportOptions.length === 1 ? 'option' : 'options'}
                      </div>
                    </div>
                    <button
                      onClick={() => setExpandedLegIndex(isExpanded ? null : sIdx)}
                      style={{
                        background: 'transparent',
                        border: '1px solid var(--border)',
                        color: 'var(--text-muted)',
                        borderRadius: 'var(--radius-sm)',
                        padding: '0.4rem 0.6rem',
                        cursor: 'pointer',
                        display: 'flex',
                        alignItems: 'center',
                        gap: '0.3rem',
                        fontSize: '0.8rem'
                      }}
                    >
                      {isExpanded ? 'Hide Options' : 'View Options'}
                      {isExpanded ? <ChevronUp size={14} /> : <ChevronDown size={14} />}
                    </button>
                  </div>
                </div>

                {/* Operator Options list */}
                {isExpanded && (
                  <div style={{ marginTop: '1rem', borderTop: '1px dashed var(--border)', paddingTop: '1rem', display: 'flex', flexDirection: 'column', gap: '0.75rem' }}>
                    {step.transportOptions.map((opt) => (
                      <div
                        key={opt.id}
                        style={{
                          background: 'rgba(30, 41, 59, 0.5)',
                          borderRadius: 'var(--radius-sm)',
                          padding: '0.85rem 1rem',
                          display: 'flex',
                          justifyContent: 'space-between',
                          alignItems: 'center',
                          flexWrap: 'wrap',
                          gap: '0.5rem'
                        }}
                      >
                        <div>
                          <div style={{ display: 'flex', alignItems: 'center', gap: '0.5rem' }}>
                            <span style={{ fontWeight: 600, color: '#fff' }}>{opt.operatorName}</span>
                            <span
                              style={{
                                fontSize: '0.7rem',
                                padding: '0.1rem 0.4rem',
                                borderRadius: '4px',
                                background: 'rgba(56, 189, 248, 0.15)',
                                color: '#38bdf8'
                              }}
                            >
                              {opt.tier}
                            </span>
                            <span style={{ fontSize: '0.75rem', color: 'var(--text-muted)' }}>
                              ({opt.frequencyPerDay}x daily)
                            </span>
                          </div>
                          {opt.scheduleNotes && (
                            <div style={{ fontSize: '0.78rem', color: 'var(--text-dim)', marginTop: '0.2rem' }}>
                              {opt.scheduleNotes}
                            </div>
                          )}
                        </div>

                        <div style={{ textAlign: 'right', display: 'flex', alignItems: 'center', gap: '1rem' }}>
                          <div>
                            <span style={{ fontSize: '1.05rem', fontWeight: 700, color: 'var(--accent-amber)' }}>
                              ৳{opt.minCostBdt} - ৳{opt.maxCostBdt}
                            </span>
                            <div style={{ fontSize: '0.7rem', color: 'var(--text-dim)' }}>per passenger</div>
                          </div>
                        </div>
                      </div>
                    ))}
                  </div>
                )}
              </div>
            );
          })}
        </div>
      </div>
    </div>
  );
};
