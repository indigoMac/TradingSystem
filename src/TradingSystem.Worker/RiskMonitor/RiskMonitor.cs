using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using TradingSystem.Application.interfaces;
using TradingSystem.Domain.Entities;
using TradingSystem.Infrastructure.Messaging;

namespace TradingSystem.Worker.RiskMonitor;

public sealed class RiskMonitorConsumer
{
    private readonly ILogger<RiskMonitorConsumer> _logger;
    private readonly IMarketDataChannel _marketDataChannel;
    private readonly IOrderIntentChannel _orderIntentChannel;
    private readonly ITradingMetrics _tradingMetrics;

    public RiskMonitorConsumer(ILogger<RiskMonitorConsumer> logger,
        ITradingMetrics tradingMetrics,
        IMarketDataChannel marketDataChannel, 
        IOrderIntentChannel orderIntentChannel)
    {
        _logger = logger;
        _tradingMetrics = tradingMetrics;
        _marketDataChannel = marketDataChannel;
        _orderIntentChannel = orderIntentChannel;
    }

    public async Task ConsumeAsync(int workerId, CancellationToken cancellationToken)
    {
        await foreach (var tick in _marketDataChannel.Reader.ReadAllAsync(cancellationToken))
        {
            var riskLatencyMs = (DateTime.UtcNow - tick.TimeStamp).TotalMilliseconds;
            _tradingMetrics.RecordRiskLatency(riskLatencyMs);

            _logger.LogInformation(
                "[RISK-{WorkerId}] Processing tick {TickId} {Symbol} {Price}",
                workerId,
                tick.ID,
                tick.Symbol,
                tick.Price);
            await AnalyseRisk(tick, cancellationToken);
            await Task.Delay(1000, cancellationToken);
        }
    }

    public async Task ProduceAsync(Tick tick, string side, int quantity, CancellationToken cancellationToken)
    {
        var orderIntent = new OrderIntent(
            ID: Guid.NewGuid(),
            Symbol: tick.Symbol,
            Price: tick.Price,
            TimeStamp: DateTime.UtcNow,
            Side: side,
            Quantity: quantity,
            TickCreatedAt: tick.TimeStamp,
            SourceTickId: tick.ID
        );

        _logger.LogInformation("[RISK] {ID} {side} {symbol} {price} {quantity}",
        orderIntent.ID,
        orderIntent.Side,
        orderIntent.Symbol,
        orderIntent.Price,
        orderIntent.Quantity);

        _tradingMetrics.IncrementRisked();

        await _orderIntentChannel.Writer.WriteAsync(orderIntent, cancellationToken);

        await Task.Delay(250, cancellationToken);
    }

    private async Task<string> AnalyseRisk(Tick tick, CancellationToken cancellationToken)
    {
        if (tick.Price > 500)
        {
            await ProduceAsync(tick, "SELL", 100, cancellationToken);
            return "SELL";
        }
        await ProduceAsync(tick, "BUY", 100, cancellationToken);
        return "BUY";
    }
}