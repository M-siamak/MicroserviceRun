using EventBus.Messages.Common;
using MassTransit;
using MediatR;
using Microsoft.OpenApi.Models;
using Ordering.API.EventBusConsumer;
using Ordering.API.Extensions;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Persistance;
using System.Reflection;
using static Ordering.Infrastructure.Persistance.OrderSeeding;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
// Add services to the container.
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
builder.Services.AddMassTransit(config =>
{
    
    config.AddConsumer<BasketCheckoutConsumer>();
    config.AddConsumer<ProductUpdateConsumer>();
    config.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(builder.Configuration.GetValue<string>("EventBusSettings:HostAddress"));

        cfg.ReceiveEndpoint(EventBusConstants.BacketCheckoutQueue, c =>
        {
            c.ConfigureConsumer<BasketCheckoutConsumer>(ctx);
        });

        cfg.ReceiveEndpoint(EventBusConstants.ProductUpdateQueue, d =>
        {
            d.ConfigureConsumer<ProductUpdateConsumer>(ctx);
        });
    });
});




//builder.Services.AddMassTransitHostedService();
//we dont need it anymore in V8
builder.Services.AddHttpClient();
builder.Services.AddScoped<BasketCheckoutConsumer>();
builder.Services.AddScoped<ProductUpdateConsumer>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ordering.API", Version = "v1" });
});

//builder.Services.AddHealthChecks().AddDbContextCheck<OrderContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MigrateDatabase<OrderContext>((context, services)=>
{
    var logger = services.GetService<ILogger<OrderContextSeed>>();
    OrderContextSeed.SeedAsync(context, logger).Wait();

});
app.UseAuthorization();

app.MapControllers();

app.Run();
