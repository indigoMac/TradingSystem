
namespace TradingSystem.Application.interfaces;

public interface ITradingMetrics
{
    void IncrementProduced();
    void IncrementRisked();
    void IncrementExecuted();

    void RecordRiskLatency(double milliseconds);
    void RecordExecutionLatency(double milliseconds);
    void RecordEndToEndLatency(double milliseconds);

    long Produced { get; }
    long Risked { get; }
    long Executed { get; }
    double AverageRiskLatencyMs { get; }
    double AverageExecutedLatencyMs { get; }
    double AverageEndToEndLatencyMs { get; }
}