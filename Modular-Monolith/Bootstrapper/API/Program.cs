using Carter;
using Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add service to the container
builder.Services.AddCarterWithAssemblies(typeof(CatalogModule).Assembly);

builder.Services
    .AddCatalogModule(builder.Configuration)
    .AddBasketModule(builder.Configuration)
    .AddOrderModule(builder.Configuration);


builder.Services.AddOpenApi();

var app = builder.Build();

app.MapCarter();

// Configure HTTP pipeline
app
    .UseCatalogModule()
    .UseBasketModule()
    .UseOrderModule();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "My API V1");
    });
}


app.Run();
