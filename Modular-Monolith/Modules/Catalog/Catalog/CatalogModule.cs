using Catalog.Data;
using Catalog.Data.Seed;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Data;
using Shared.Data.Seed;

namespace Catalog
{
    public static class CatalogModule
    {
        public static IServiceCollection AddCatalogModule(this IServiceCollection services, IConfiguration configuration)
        {

            var connectionStrings = configuration.GetConnectionString("Database");

            services.AddDbContext<CatalogDbContext>(options => options.UseNpgsql(connectionStrings));

            services.AddScoped<IDataSeeder, CatalogDataSeeder>();

            return services;
        }

        public static IApplicationBuilder UseCatalogModule(this IApplicationBuilder app)
        {
            // Apply migration on application startup

            app.UseMigration<CatalogDbContext>();

            return app;
        }
    }
}
