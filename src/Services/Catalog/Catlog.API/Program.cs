


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

var app = builder.Build();
//Request pipeline

app.MapCarter();
app.UseExceptionHandler();


app.Run();
