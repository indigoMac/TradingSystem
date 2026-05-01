using System.Threading.Channels;
using TradingSystem.Application.interfaces;
using TradingSystem.Domain.Entities;

namespace TradingSystem.Infrastructure.Messaging;

public sealed class MarketDataChannel : IMarketDataChannel
{
    private readonly Channel<Tick> _channel;

    public MarketDataChannel()
    {
        _channel = Channel.CreateBounded<Tick>(new BoundedChannelOptions(100)
        {
            FullMode = BoundedChannelFullMode.Wait
        });
    }
    public ChannelWriter<Tick> Writer => _channel.Writer;
    public ChannelReader<Tick> Reader => _channel.Reader;
}