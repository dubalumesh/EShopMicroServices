using BuildingBlocks.Extensions;
using HealthChecks.UI.Client;
using JasperFx;

var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly;

// register services and configure the application

//register Carter modules from the current assembly
builder.Services.AddCarterModulesFromAssembly(assembly);

builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddProblemDetails();

// register FluentValidation services from the current assembly
builder.Services.AddValidatorsFromAssembly(assembly);

// register MediatR services from the current assembly
builder.Services.AddMediatR((config) =>
{
    config.RegisterServicesFromAssemblies(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("BasketAPIConnectionString")!);
    options.AutoCreateSchemaObjects = AutoCreate.CreateOrUpdate;

}).UseLightweightSessions();

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.Decorate<IBasketRepository, CacheBasketRepository>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("RedisConnectionString");
});

builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("BasketAPIConnectionString")!)
    .AddRedis(builder.Configuration.GetConnectionString("RedisConnectionString")!);

var app = builder.Build();

// configure the HTTP request pipeline

app.MapCarter();
app.UseExceptionHandler();
app.UseHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();
