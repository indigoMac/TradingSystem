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

    public RiskMonitorConsumer(ILogger<RiskMonitorConsumer> logger, IMarketDataChannel marketDataChannel, IOrderIntentChannel orderIntentChannel)
    {
        _logger = logger;
        _marketDataChannel = marketDataChannel;
        _orderIntentChannel = orderIntentChannel;
    }

    public async Task ConsumeAsync(CancellationToken cancellationToken)
    {
        await foreach (var tick in _marketDataChannel.Reader.ReadAllAsync(cancellationToken))
        {
            await AnalyseRisk(tick, cancellationToken);
            await Task.Delay(250, cancellationToken);
        }
    }

    public async Task ProduceAsync(Tick tick, string side, int quantity, CancellationToken cancellationToken)
    {
        var orderIntent = new OrderIntent(
            ID: Guid.NewGuid(),
            Symbol: tick.Symbol,
            Price: tick.Price,
            TimeStamp: tick.TimeStamp,
            Side: side,
            Quantity: quantity
        );

        _logger.LogInformation("[RISK] {ID} {side} {symbol} {price} {quantity}",
        orderIntent.ID,
        orderIntent.Side,
        orderIntent.Symbol,
        orderIntent.Price,
        orderIntent.Quantity);

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