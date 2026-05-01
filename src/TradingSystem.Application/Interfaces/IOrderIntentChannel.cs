using System.Threading.Channels;
using TradingSystem.Domain.Entities;

namespace TradingSystem.Application.interfaces;

public interface IOrderIntentChannel
{
    ChannelWriter<OrderIntent> Writer { get; }
    ChannelReader<OrderIntent> Reader { get; }
}