using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TrafficFineManagement.Web.Consts;
using TrafficFineManagement.Web.Models.Entities;
using TrafficFineManagement.Web.Models.Enums;

namespace TrafficFineManagement.Web.Data.Seed
{
    public static class DemoDataSeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            await using var scope = services.CreateAsyncScope();

            var context = scope.ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

            var userManager = scope.ServiceProvider
                .GetRequiredService<UserManager<User>>();

            await CreateUserAsync(
                userManager,
                "admin",
                "Demo",
                "Yönetici",
                RoleName.Admin);

            await CreateUserAsync(
                userManager,
                "financeadmin",
                "Demo",
                "Finans",
                RoleName.Finance);

            var officer = await CreateUserAsync(
                userManager,
                "officer",
                "Demo",
                "Memur",
                RoleName.Officer);

            var citizen = await CreateUserAsync(
                userManager,
                "11111111110",
                "Ali",
                "Yılmaz",
                RoleName.Citizen,
                "11111111110");

            const string plate = "34DM001";

            var car = await context.Cars
                .FirstOrDefaultAsync(car => car.Plate == plate);

            if (car is null)
            {
                car = new Car
                {
                    Plate = plate,
                    Brand = "Ford",
                    Model = "Focus",
                    CarType = Enum.GetValues<CarType>()[0],
                    OwnerId = citizen.Id,
                    User = citizen,
                    CreatedAt = DateTime.UtcNow,
                    LastUpdatedAt = DateTime.UtcNow
                };

                context.Cars.Add(car);
                await context.SaveChangesAsync();
            }
            else if (car.OwnerId != citizen.Id)
            {
                throw new InvalidOperationException(
                    $"Araç başka bir kullanıcıya tanımlı.");
            }

            const string description = "Demo: Hız sınırı aşım cezası..";

            var fineExists = await context.Fines.AnyAsync(fine =>
                fine.CarId == car.Id &&
                fine.CreatedByUserId == officer.Id &&
                fine.Description == description);

            if (fineExists)
                return;

            var now = DateTime.UtcNow;

            var fine = new Fine
            {
                CarId = car.Id,
                Car = car,
                CreatedByUserId = officer.Id,
                CreatedByUser = officer,
                Amount = 1500m,
                Description = description,
                FineDate = now,
                Status = FineStatus.New,
                IsPaid = false,
                CreatedAt = now,
                LastUpdatedAt = now
            };

            fine.Histories.Add(new FineHistory
            {
                UserId = officer.Id,
                User = officer,
                ActionType = FineActionType.Created,
                Description = "Fine created.",
                PreviousStatus = null,
                NewStatus = FineStatus.New,
                CreatedAt = now
            });

            context.Fines.Add(fine);
            await context.SaveChangesAsync();
        }

        private static async Task<User> CreateUserAsync(
            UserManager<User> userManager,
            string userName,
            string name,
            string surname,
            string role,
            string? identityNumber = null)
        {
            var user = await userManager.FindByNameAsync(userName);

            if (user is null)
            {
                user = new User
                {
                    UserName = userName,
                    Name = name,
                    Surname = surname,
                    IdentityNumber = identityNumber,
                    AccountType = AccountType.Individual,
                    CreatedAt = DateTime.UtcNow,
                    LastUpdatedAt = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(user, "12345");

                EnsureSucceeded(result);
            }

            if (!await userManager.IsInRoleAsync(user, role))
            {
                var result = await userManager.AddToRoleAsync(user, role);

                EnsureSucceeded(result);
            }

            return user;
        }

        private static void EnsureSucceeded(IdentityResult result)
        {
            if (!result.Succeeded)
            {
                throw new InvalidOperationException(
                    string.Join("; ",
                        result.Errors.Select(error => error.Description)));
            }
        }
    }
}
