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
    private readonly IMarketDataChannel _marketDataChannel;
    private readonly IOrderIntentChannel _orderIntentChannel;
    // private String RiskDecision = "";

    public RiskMonitorConsumer(IMarketDataChannel marketDataChannel, IOrderIntentChannel orderIntentChannel)
    {
        _marketDataChannel = marketDataChannel;
        _orderIntentChannel = orderIntentChannel;
    }

    public async Task ConsumeAsync(CancellationToken cancellationToken)
    {
        await foreach (var tick in _marketDataChannel.Reader.ReadAllAsync(cancellationToken))
        {
            var RiskDecision = await AnalyseRisk(tick, cancellationToken);
            Console.WriteLine($"Tick: {tick.Symbol} Price: {tick.Price} Time: {tick.TimeStamp} Order: {RiskDecision}");
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

        await _orderIntentChannel.Writer.WriteAsync(orderIntent, cancellationToken);

        Console.WriteLine($"{orderIntent.ID}: {orderIntent.Side} {orderIntent.Quantity} or {orderIntent.Symbol} at {orderIntent.Price} per share at time {orderIntent.TimeStamp}");
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