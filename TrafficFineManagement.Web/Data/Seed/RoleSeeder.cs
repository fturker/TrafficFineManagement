using Microsoft.AspNetCore.Identity;
using TrafficFineManagement.Web.Consts;

namespace TrafficFineManagement.Web.Data.Seed
{
    public static class RoleSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            await using var scope = serviceProvider.CreateAsyncScope();

            var roleManager = scope.ServiceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();

            string[] roleNames =
            {
                RoleName.Citizen,
                RoleName.Officer,
                RoleName.Admin,
                RoleName.Finance,
                RoleName.SuperAdmin
            };

            foreach (var roleName in roleNames)
            {
                if (await roleManager.RoleExistsAsync(roleName))
                {
                    continue;
                }

                var result = await roleManager.CreateAsync(
                    new IdentityRole(roleName));

                if (!result.Succeeded)
                {
                    var errors = string.Join(
                        "; ",
                        result.Errors.Select(error => error.Description));

                    throw new InvalidOperationException(
                        $"Failed to create role '{roleName}': {errors}");
                }
            }
        }
    }
}