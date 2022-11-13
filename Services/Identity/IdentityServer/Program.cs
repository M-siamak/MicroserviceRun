using IdentityServer;
using IdentityServer.DataBase;
using IdentityServer.Model;
using IdentityServerHost.Quickstart.UI;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using IdentityServer4;
using Microsoft.EntityFrameworkCore.Migrations.Internal;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("Defualtconnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>().
    AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

builder.Services.AddIdentityServer(options =>
{
    options.Events.RaiseErrorEvents = true;
    options.Events.RaiseInformationEvents = true;
    options.Events.RaiseFailureEvents = true;
    options.Events.RaiseSuccessEvents = true;
    options.EmitScopesAsSpaceDelimitedStringInJwt = true;

}).AddInMemoryApiScopes(SD.ApiScopes)
  .AddInMemoryIdentityResources(SD.IdentityResources)
  .AddInMemoryClients(SD.Clients)
  .AddAspNetIdentity<ApplicationUser>()
  .AddDeveloperSigningCredential();


builder.Services.AddControllersWithViews();

//builder.Services.AddIdentityServer()
//                .AddInMemoryClients(Config.Clients)
//                .AddInMemoryApiScopes(Config.ApiScopes)
//                .AddInMemoryIdentityResources(Config.IdentityResources)
//                .AddTestUsers(TestUsers.Users)
//                .AddDeveloperSigningCredential();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseIdentityServer();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();

});

app.Run();
