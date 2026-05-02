using Microsoft.Extensions.Logging;
using TradingSystem.Application.interfaces;
using TradingSystem.Infrastructure.Metrics;

namespace TradingSystem.Worker.Mertrics;

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

            await Task.Delay(1000, cancellationToken);
        }
    }
}