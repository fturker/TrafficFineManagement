using Microsoft.AspNetCore.Identity;
using TrafficFineManagement.Web.Consts;
using TrafficFineManagement.Web.Models.Entities;

namespace TrafficFineManagement.Web.Data.Seed
{
    public static class SuperAdminSeeder
    {
       public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            await using var scope = serviceProvider.CreateAsyncScope();
            var manager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            if ((await manager.GetUsersInRoleAsync(RoleName.SuperAdmin)).Any())
                return;

            await using var transaction = await context.Database.BeginTransactionAsync();
            var password = "12345";

            var user = new User
            {
                UserName = "superadmin",
                Name = "Super",
                Surname = "Admin"
            };

            CheckResult(await manager.CreateAsync(user, password));
            CheckResult(await manager.AddToRoleAsync(user, RoleName.SuperAdmin));

            await transaction.CommitAsync();
        }

        private static void CheckResult(IdentityResult result)
        {
            if (!result.Succeeded)
                throw new InvalidOperationException(
                    string.Join("; ", result.Errors.Select(x => x.Description)));
        }
    }
}
