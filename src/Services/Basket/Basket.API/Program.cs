using BuildingBlocks.Extensions;
using HealthChecks.UI.Client;
using JasperFx;

var builder = WebApplication.CreateBuilder(args);
// register application services
var assembly = typeof(Program).Assembly;
builder.Services.AddCarterModulesFromAssembly(assembly);

builder.Services.AddMediatR((config) =>
{
    config.RegisterServicesFromAssemblies(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

// register Database and caching services
builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("BasketAPIConnectionString")!);
    options.AutoCreateSchemaObjects = AutoCreate.CreateOrUpdate;

}).UseLightweightSessions();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("RedisConnectionString");
});
// register repository and decorate it with caching
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.Decorate<IBasketRepository, CacheBasketRepository>();

builder.Services.AddGrpcClient<Discount.Grpc.DiscountProtoService.DiscountProtoServiceClient>(options =>
{
    options.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]!);
});

//register  cross cutting concerns
builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddValidatorsFromAssembly(assembly);
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
