using Catlog.API.Extensions;
using Catlog.API.Products.CreateProduct;

var builder = WebApplication.CreateBuilder(args);
//register services

builder.Services.AddCarterModulesFromAssembly(
    typeof(Program).Assembly);


builder.Services.AddMediatR((config) =>
{
    config.RegisterServicesFromAssemblies(typeof(Program).Assembly);
});

// register marten to connect Postgres SQL databse
builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("CatalogAPIConnectionString")!);
}).UseLightweightSessions();

var app = builder.Build();
//Request pipeline

app.MapCarter();

app.Run();
