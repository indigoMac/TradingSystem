using TradingSystem.Application.interfaces;
using TradingSystem.Domain.Entities;

namespace TradingSystem.Worker.MarketData;

public sealed class FakeMarketDataProducer
{
    private readonly IMarketDataChannel _marketDataChannel;
    private readonly string[] _symbols = ["AAPL", "MSFT", "NVDA"];
    private readonly Random _random = new();

    public FakeMarketDataProducer(IMarketDataChannel marketDataChannel)
    {
        _marketDataChannel = marketDataChannel;
    }

    public async Task ProduceAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var symbol = _symbols[_random.Next(_symbols.Length)];
            var price = Math.Round((decimal)(_random.NextDouble() * 1000), 2);

            var tick = new Tick(
                ID: Guid.NewGuid(),
                Symbol: symbol,
                Price: price,
                TimeStamp: DateTime.UtcNow
            );

            await _marketDataChannel.Writer.WriteAsync(tick, cancellationToken);

            Console.WriteLine($"[PRODUCED] TickID: {tick.ID} - {tick.Symbol} ${tick.Price}");
            await Task.Delay(250, cancellationToken);
        }
    }
}