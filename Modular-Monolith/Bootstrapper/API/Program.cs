using Carter;
using Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add service to the container
builder.Services.AddCarterWithAssemblies(typeof(CatalogModule).Assembly);

builder.Services
    .AddCatalogModule(builder.Configuration)
    .AddBasketModule(builder.Configuration)
    .AddOrderModule(builder.Configuration);

var app = builder.Build();

app.MapCarter();

// Configure HTTP pipeline
app
    .UseCatalogModule()
    .UseBasketModule()
    .UseOrderModule();

app.Run();
