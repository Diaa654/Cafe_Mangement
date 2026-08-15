using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Cafe_Mangement.Extentions
{
    public static class WebApplicationRegistration
    {
        public static async Task<WebApplication> MigrateDatabaseAsync(this WebApplication app)
        {
            await using var scope = app.Services.CreateAsyncScope();
            var DbContext = scope.ServiceProvider.GetRequiredService<CafeDbContext>();
            var pendingMigrations = await DbContext.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
                await DbContext.Database.MigrateAsync();
            return app;
        }
        public static async Task<WebApplication> SeedDataAsync(this WebApplication app)
        {
            await using var scope = app.Services.CreateAsyncScope();
            var dataInitializer = scope.ServiceProvider.GetRequiredService<IDataInitializer>();
            await dataInitializer.InitializeDataAsync();
            return app;
        }
    }
}
