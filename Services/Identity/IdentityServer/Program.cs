using IdentityServer;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddIdentityServer()
                .AddInMemoryClients(new List<Client>())
                .AddInMemoryApiResources(new List<ApiResource>())
                .AddInMemoryApiScopes(new List<ApiScope>())
                .AddInMemoryIdentityResources(new List<IdentityResource>())
                .AddTestUsers(new List<TestUser>())
                .AddDeveloperSigningCredential();

var app = builder.Build();

app.UseIdentityServer();
app.MapGet("/", () => "Hello World!");

app.Run();
