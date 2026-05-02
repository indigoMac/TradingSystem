using System.Diagnostics;
using TradingSystem.Application.interfaces;
using TradingSystem.Domain.Entities;

namespace TradingSystem.Worker.MarketData;

public sealed class FakeMarketDataProducer
{
    private readonly IMarketDataChannel _marketDataChannel;
    private readonly ILogger<FakeMarketDataProducer> _logger;
    private readonly ITradingMetrics _tradingMetrics;
    private readonly string[] _symbols = ["AAPL", "MSFT", "NVDA"];
    private readonly Random _random = new();

    public FakeMarketDataProducer(ILogger<FakeMarketDataProducer> logger, 
        ITradingMetrics tradingMetrics,
        IMarketDataChannel marketDataChannel)
    {
        _logger = logger;
        _tradingMetrics = tradingMetrics;
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

            // _logger.LogInformation("[PRODUCER] Attempting to write tick {TickId}", tick.ID);
            // var start = Stopwatch.GetTimestamp();

            await _marketDataChannel.Writer.WriteAsync(tick, cancellationToken);

            _tradingMetrics.IncrementProduced();

            // var elapsed = Stopwatch.GetElapsedTime(start);
            // _logger.LogInformation(
            //     "[PRODUCER] Wrote tick {TickId}. WriteWaitMs={WriteWaitMs}",
            //     tick.ID,
            //     elapsed.TotalMilliseconds);

            _logger.LogInformation("[PRODUCED] {TickID}: {symbol} {price}",
            tick.ID,
            tick.Symbol,
            tick.Price);
            await Task.Delay(10, cancellationToken);
        }
    }
}