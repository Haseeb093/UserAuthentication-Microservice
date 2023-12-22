using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Configuration.AddJsonFile("GateWay/Ocelot.json");
builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();
app.UseHttpsRedirection();
app.UseOcelot().Wait();

app.Run();

