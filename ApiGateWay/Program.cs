using ApiGateWay.Config;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddJwtAuthentication();

builder.Configuration.AddJsonFile("GateWay/UserAuthentication.json");

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseOcelot().Wait();

app.UseAuthorization();

app.Run();

