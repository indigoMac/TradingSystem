using TradingSystem.Application.interfaces;

namespace TradingSystem.Infrastructure.Metrics;

public sealed class TradingMetrics : ITradingMetrics
{
    private long _produced;
    private long _risked;
    private long _executed;
    private long _riskLatencyCount;
    private long _riskLatencyTotalMs;
    private long _executedLatencyCount;
    private long _executedLatencyTotalMs;
    private long _endToEndLatencyCount;
    private long _endToEndLatencyTotalMs;

    public long Produced => Interlocked.Read(ref _produced);
    public long Risked => Interlocked.Read(ref _risked);
    public long Executed => Interlocked.Read(ref _executed);
    public double AverageRiskLatencyMs => _riskLatencyCount == 0 ? 0 : (double)_riskLatencyTotalMs / _riskLatencyCount; 
    public double AverageExecutedLatencyMs => _executedLatencyCount == 0 ? 0 : (double) _executedLatencyTotalMs / _executedLatencyCount; 
    public double AverageEndToEndLatencyMs => _endToEndLatencyCount == 0 ? 0 : (double) _endToEndLatencyTotalMs / _endToEndLatencyCount;

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

    public void RecordRiskLatency(double milliseconds)
    {
        Interlocked.Increment(ref _riskLatencyCount);
        Interlocked.Add(ref _riskLatencyTotalMs, (long) milliseconds);
    }

    public void RecordExecutionLatency(double milliseconds)
    {
        Interlocked.Increment(ref _executedLatencyCount);
        Interlocked.Add(ref _executedLatencyTotalMs, (long) milliseconds);
    }

    public void RecordEndToEndLatency(double milliseconds)
    {
        Interlocked.Increment(ref _endToEndLatencyCount);
        Interlocked.Add(ref _endToEndLatencyTotalMs, (long) milliseconds);
    }
}