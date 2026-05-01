using TradingSystem.Application.interfaces;

namespace TradingSystem.Worker.MarketData;

public sealed class MarketDataConsumer
{
    private readonly IMarketDataChannel _marketDataChannel;

    public MarketDataConsumer(IMarketDataChannel marketDataChannel)
    {
        _marketDataChannel = marketDataChannel;
    }

    public async Task ConsumeAsync(CancellationToken cancellationToken)
    {
        await foreach (var tick in _marketDataChannel.Reader.ReadAllAsync(cancellationToken))
        {
            Console.WriteLine($"Consumed Tick: {tick.Symbol} at price ${tick.Price} at {tick.TimeStamp:HH:mm:ss.fff}");

            await Task.Delay(500, cancellationToken);   
        }
    }
}