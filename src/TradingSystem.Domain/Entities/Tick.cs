namespace TradingSystem.Domain.Entities;

public sealed record Tick(
    Guid ID,
    string Symbol,
    decimal Price,
    DateTime TimeStamp
);