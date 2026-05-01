using TradingSystem.Worker.RiskMonitor;
using TradingSystem.Domain.Entities;
using TradingSystem.Application.interfaces;
using System.Collections.Concurrent;

namespace TradingSystem.Worker.OrderExecution;

public sealed class OrderExecusion
{
    private readonly IOrderIntentChannel _orderIntentChannel;
    private readonly ILogger<OrderExecusion> _logger;

    public OrderExecusion(ILogger<OrderExecusion> logger, IOrderIntentChannel orderIntentChannel)
    {
        _logger = logger;
        _orderIntentChannel = orderIntentChannel;
    }

    public async Task ConsumeAsync(CancellationToken cancellationToken)
    {
        await foreach (var orderIntent in _orderIntentChannel.Reader.ReadAllAsync(cancellationToken))
        {
            _logger.LogInformation("[EXECUTED] {ID} {Side} {Symbol} {Price} {Quantity} {Time}",
            orderIntent.ID,
            orderIntent.Side,
            orderIntent.Symbol,
            orderIntent.Price,
            orderIntent.Quantity,
            orderIntent.TimeStamp);
            await Task.Delay(250, cancellationToken);
        }
    }
}