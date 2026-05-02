
namespace TradingSystem.Application.interfaces;

public interface ITradingMetrics
{
    void IncrementProduced();
    void IncrementRisked();
    void IncrementExecuted();

    long Produced { get; }
    long Risked { get; }
    long Executed { get; }
}