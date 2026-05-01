using System.Threading.Channels;
using TradingSystem.Domain.Entities;

namespace TradingSystem.Application.interfaces;

public interface IMarketDataChannel
{
    ChannelWriter<Tick> Writer { get; }
    ChannelReader<Tick> Reader { get; }
}