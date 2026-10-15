using TrafficFineManagement.Web.Data.Seed;

namespace TrafficFineManagement.Web.Extensions
{
    public static class DatabaseSeederExtensions
    {
        public static async Task SeedDatabaseAsync(this WebApplication app)
        {
            await RoleSeeder.SeedAsync(app.Services);

            if (app.Environment.IsDevelopment())
            {
                await SuperAdminSeeder.SeedAsync(app.Services);
                await DemoDataSeeder.SeedAsync(app.Services);
            }
        }
    }
}
