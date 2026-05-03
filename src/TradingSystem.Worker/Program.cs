using Serilog;
using TradingSystem.Application.interfaces;
using TradingSystem.Infrastructure.Messaging;
using TradingSystem.Worker.MarketData;
using TradingSystem.Worker.RiskMonitor;
using TradingSystem.Worker;
using TradingSystem.Worker.OrderExecution;
using TradingSystem.Infrastructure.Metrics;
using TradingSystem.Worker.Metrics;

var builder = Host.CreateApplicationBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Services.AddSerilog();
Log.Information("Starting Logging...");

builder.Services.AddSingleton<IMarketDataChannel, MarketDataChannel>();
builder.Services.AddSingleton<IOrderIntentChannel, OrderIntentChannel>();
builder.Services.AddSingleton<FakeMarketDataProducer>();
builder.Services.AddSingleton<RiskMonitorConsumer>();
builder.Services.AddSingleton<OrderExecusion>();
builder.Services.AddSingleton<ITradingMetrics, TradingMetrics>();
builder.Services.AddSingleton<MetricsReporter>();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
