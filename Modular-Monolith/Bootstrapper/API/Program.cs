var builder = WebApplication.CreateBuilder(args);

// Add service to the container

var app = builder.Build();

// Configure HTTP pipeline

app.Run();
