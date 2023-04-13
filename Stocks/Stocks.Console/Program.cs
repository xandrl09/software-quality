// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stocks.Console;
using Stocks.Services;

var hostBuilder = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, builder) =>
    {
        builder.SetBasePath(AppContext.BaseDirectory)
        .AddJsonFile("appsettings.json", optional: false);
    })
    .ConfigureServices(services =>
    {
        services.AddTransient<Client>();
        RegisterServices.Register(services);
    });

using IHost host = hostBuilder.Build();

var client = host.Services.GetService<Client>();
await client.RunAsync();