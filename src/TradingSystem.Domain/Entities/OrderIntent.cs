namespace TradingSystem.Domain.Entities;

public sealed record OrderIntent(
    Guid ID,
    string Symbol,
    decimal Price,
    DateTime TimeStamp,
    string Side,
    int Quantity,
    DateTime TickCreatedAt,
    Guid SourceTickId
);
