using Microsoft.Extensions.Logging;
using TradingSystem.Application.interfaces;
using TradingSystem.Infrastructure.Metrics;

namespace TradingSystem.Worker.Metrics;

public sealed class MetricsReporter
{
    private readonly ILogger<MetricsReporter> _logger;
    private readonly ITradingMetrics _metrics;

    public MetricsReporter(ILogger<MetricsReporter> logger, ITradingMetrics metrics)
    {
        _logger = logger;
        _metrics = metrics;
    }

    public async Task ReportAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            _logger.LogInformation(
                "[METRICS] Produced={Produced} Risked={Risked} Executed={Executed}",
                _metrics.Produced,
                _metrics.Risked,
                _metrics.Executed);

            _logger.LogInformation(
                "[LATENCY METRICS] Risk={riskLatency} Executted={executedLatency} E2E={e2eLatency}",
                _metrics.AverageRiskLatencyMs,
                _metrics.AverageExecutedLatencyMs,
                _metrics.AverageEndToEndLatencyMs);

            await Task.Delay(1000, cancellationToken);
        }
    }
}