using TradingSystem.Worker.RiskMonitor;
using TradingSystem.Domain.Entities;
using TradingSystem.Application.interfaces;

namespace TradingSystem.Worker.OrderExecution;

public sealed class OrderExecusion
{
    private readonly IOrderIntentChannel _orderIntentChannel;

    public OrderExecusion(IOrderIntentChannel orderIntentChannel)
    {
        _orderIntentChannel = orderIntentChannel;
    }

    public async Task ConsumeAsync(CancellationToken cancellationToken)
    {
        await foreach (var OrderIntent in _orderIntentChannel.Reader.ReadAllAsync(cancellationToken))
        {
            Console.WriteLine($"[EXECUTED] {OrderIntent.ID}: A {OrderIntent.Side} Order Executed for {OrderIntent.Quantity} of {OrderIntent.Symbol} at {OrderIntent.Price} on {OrderIntent.TimeStamp}");
            await Task.Delay(250, cancellationToken);
        }
    }
}