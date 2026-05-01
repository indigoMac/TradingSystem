using System.Security.Authentication.ExtendedProtection;
using System.Threading.Channels;
using TradingSystem.Application.interfaces;
using TradingSystem.Domain.Entities;

namespace TradingSystem.Infrastructure.Messaging;

public sealed class OrderIntentChannel : IOrderIntentChannel
{
    private readonly Channel<OrderIntent> _channel;

    public OrderIntentChannel()
    {
        _channel = Channel.CreateBounded<OrderIntent>( new BoundedChannelOptions(100)
        {
            FullMode = BoundedChannelFullMode.Wait
        });
    }

    public ChannelWriter<OrderIntent> Writer => _channel.Writer;

    public ChannelReader<OrderIntent> Reader => _channel.Reader;
}