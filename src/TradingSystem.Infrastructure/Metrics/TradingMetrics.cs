using TradingSystem.Application.interfaces;

namespace TradingSystem.Infrastructure.Metrics;

public sealed class TradingMetrics : ITradingMetrics
{
    private long _produced;
    private long _risked;
    private long _executed;

    public long Produced => Interlocked.Read(ref _produced);
    public long Risked => Interlocked.Read(ref _risked);
    public long Executed => Interlocked.Read(ref _executed);

    public void IncrementProduced()
    {
        Interlocked.Increment(ref _produced);
    }

    public void IncrementRisked()
    {
        Interlocked.Increment(ref _risked);
    }

    public void IncrementExecuted()
    {
        Interlocked.Increment(ref _executed);
    }
}