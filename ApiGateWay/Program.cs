using ApiGateWay.Config;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("GateWay/Ocelot.json");

// Add services to the container.

builder.Services.AddJwtAuthentication();

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseOcelot().Wait();

app.UseAuthorization();

app.Run();

