using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);


IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddJsonFile($"ocelot.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true);
            })
            .ConfigureLogging((hostingContext, loggingbuilder) =>
            {
                loggingbuilder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                loggingbuilder.AddConsole();
                loggingbuilder.AddDebug();
            });


builder.Services.AddOcelot()
    .AddCacheManager(setting => setting.WithDictionaryHandle()); 
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.UseOcelot();
app.Run();
