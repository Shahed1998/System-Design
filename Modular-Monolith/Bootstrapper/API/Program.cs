var builder = WebApplication.CreateBuilder(args);

// Add service to the container
builder.Services
    .AddCatalogModule(builder.Configuration)
    .AddBasketModule(builder.Configuration)
    .AddOrderModule(builder.Configuration);

var app = builder.Build();

// Configure HTTP pipeline
app
    .UseCatalogModule()
    .UseBasketModule()
    .UseOrderModule();

app.Run();
