using System.Runtime.CompilerServices;
using TradingSystem.Worker.MarketData;
using TradingSystem.Worker.OrderExecution;
using TradingSystem.Worker.RiskMonitor;

namespace TradingSystem.Worker;

public class Worker : BackgroundService
{
    private readonly FakeMarketDataProducer _marketProducer;
    private readonly RiskMonitorConsumer _riskMonitor;
    private readonly OrderExecusion _orderExecusionConsumer;

    public Worker(
        FakeMarketDataProducer producer,
        RiskMonitorConsumer riskMonitor,
        OrderExecusion orderExecusionConsumer)
    {
        _marketProducer = producer;
        _riskMonitor = riskMonitor;
        _orderExecusionConsumer = orderExecusionConsumer;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var producerTask = _marketProducer.ProduceAsync(stoppingToken);
        var consumerTask = _riskMonitor.ConsumeAsync(stoppingToken);
        var executionTask = _orderExecusionConsumer.ConsumeAsync(stoppingToken);

        await Task.WhenAll(producerTask, consumerTask, executionTask);
    }
}

