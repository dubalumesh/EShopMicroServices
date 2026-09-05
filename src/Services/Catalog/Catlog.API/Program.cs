


var builder = WebApplication.CreateBuilder(args);
//register services

//register Carter modules from the current assembly
builder.Services.AddCarterModulesFromAssembly(
    typeof(Program).Assembly);

builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddProblemDetails();

// register MediatR services from the current assembly
builder.Services.AddMediatR((config) =>
{
    config.RegisterServicesFromAssemblies(typeof(Program).Assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

// register FluentValidation services from the current assembly
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);


// register marten to connect Postgres SQL databse
builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("CatalogAPIConnectionString")!);
}).UseLightweightSessions();

if (builder.Environment.IsDevelopment())
{
    builder.Services.InitializeMartenWith<CatalogInitialData>();
}

builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("CatalogAPIConnectionString")!);

var app = builder.Build();
//Request pipeline

app.MapCarter();
app.UseExceptionHandler();
app.UseHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var response = new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(entry => new
            {
                name = entry.Key,
                status = entry.Value.Status.ToString(),
                exception = entry.Value.Exception?.Message,
                duration = entry.Value.Duration.ToString()
            })
        };
        await context.Response.WriteAsJsonAsync(response);
    }
});

app.Run();
