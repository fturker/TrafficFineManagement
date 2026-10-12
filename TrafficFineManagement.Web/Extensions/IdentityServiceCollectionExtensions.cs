using Microsoft.AspNetCore.Identity;
using TrafficFineManagement.Web.Data;
using TrafficFineManagement.Web.Models.Entities;

namespace TrafficFineManagement.Web.Extensions
{
    public static class IdentityServiceCollectionExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 4;
                options.User.RequireUniqueEmail = false;

                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;

                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan =
                    TimeSpan.FromMinutes(10);
            }).AddEntityFrameworkStores<ApplicationDbContext>();

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "TrafficFineManagement.Auth";
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.Lax;

                options.LoginPath = "/Login";
                options.AccessDeniedPath = "/AccessDenied";

                options.ExpireTimeSpan = TimeSpan.FromHours(2);
                options.SlidingExpiration = true;
            });

            return services;
        }
    }
}
