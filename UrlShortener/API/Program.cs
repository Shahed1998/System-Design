using API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(opt => {
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Catch All API
app.MapFallback(async (ApplicationDbContext db, HttpContext ctx) => 
{
    var shortUrl = ctx.Request.Path.ToUriComponent().Trim().TrimStart('/');

    var urlMapper = await db.UrlMappers.FirstOrDefaultAsync(x => x.ShortenedUrl == shortUrl);

    if(urlMapper != null)
    {
        return Results.Redirect(urlMapper!.ActualUrl!);
    }

    return Results.BadRequest("Unable to find API");
 
});

app.Run();
