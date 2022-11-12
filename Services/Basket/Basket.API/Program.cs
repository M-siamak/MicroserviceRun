using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Basket.API.Services;
using Discount.Grpc.Protos;
using EventBus.Messages.Common;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "https://localhost:5005";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false
        };
    });


builder.Services.AddAuthorization(authorizationOptions =>
{
    authorizationOptions.AddPolicy(
    name: "MustOwnBasket",
     configurePolicy: policyBuilder =>
     {
         policyBuilder.RequireAuthenticatedUser();
         policyBuilder.AddRequirements(new MustOwnBasketRequirement());
     });
});

builder.Services.AddScoped<IAuthorizationHandler, MustOwnBasketHandler>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetValue<string>("CacheSettings:ConnectionString");
});


builder.Services.AddScoped<IBasketRepository, BasketRepository>();

var version = builder.Configuration.GetValue<string>("GrpcSettings:DiscountUrl");

builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(o => o.Address = new Uri(builder.Configuration.GetValue<string>("GrpcSettings:DiscountUrl")));

builder.Services.AddScoped<DiscountGrpcService>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddMassTransit(config =>
{
    config.UsingRabbitMq(configure:
        (ctx, cfg) =>
        {
            cfg.Host(builder.Configuration.GetValue<string>("EventBusSettings:HostAddress"));
        });
});
//builder.Services.AddMassTransitHostedService();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Basket.API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Basket.API v1"));
}

app.UseAuthorization();

app.MapControllers();

app.Run();
