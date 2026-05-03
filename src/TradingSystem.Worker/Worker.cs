using System.Runtime.CompilerServices;
using TradingSystem.Worker.MarketData;
using TradingSystem.Worker.OrderExecution;
using TradingSystem.Worker.RiskMonitor;
using TradingSystem.Worker.Metrics;

namespace TradingSystem.Worker;

public class Worker : BackgroundService
{
    private readonly FakeMarketDataProducer _marketProducer;
    private readonly RiskMonitorConsumer _riskMonitor;
    private readonly OrderExecusion _orderExecusionConsumer;
    private readonly MetricsReporter _metricsReporter;

    public Worker(
        FakeMarketDataProducer producer,
        RiskMonitorConsumer riskMonitor,
        OrderExecusion orderExecusionConsumer,
        MetricsReporter metricsReporter)
    {
        _marketProducer = producer;
        _riskMonitor = riskMonitor;
        _orderExecusionConsumer = orderExecusionConsumer;
        _metricsReporter = metricsReporter;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var producerTask = _marketProducer.ProduceAsync(stoppingToken);
        // var riskTask = _riskMonitor.ConsumeAsync(stoppingToken);
        var riskTasks = Enumerable.Range(1, 3)
            .Select(workerId => _riskMonitor.ConsumeAsync(workerId, stoppingToken))
            .ToArray();
        var executionTask = _orderExecusionConsumer.ConsumeAsync(stoppingToken);
        var metricsTask = _metricsReporter.ReportAsync(stoppingToken);

        // await Task.WhenAll(producerTask, riskTask, executionTask);
        await Task.WhenAll(
            riskTasks.Append(producerTask).Append(executionTask).Append(metricsTask)
        );
    }
}

