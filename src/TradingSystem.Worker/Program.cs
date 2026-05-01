using TradingSystem.Application.interfaces;
using TradingSystem.Infrastructure.Messaging;
using TradingSystem.Worker.MarketData;
using TradingSystem.Worker.RiskMonitor;
using TradingSystem.Worker;
using TradingSystem.Worker.OrderExecution;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<IMarketDataChannel, MarketDataChannel>();
builder.Services.AddSingleton<IOrderIntentChannel, OrderIntentChannel>();
builder.Services.AddSingleton<FakeMarketDataProducer>();
builder.Services.AddSingleton<RiskMonitorConsumer>();
builder.Services.AddSingleton<OrderExecusion>();
// builder.Services.AddSingleton<MarketDataConsumer>();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
