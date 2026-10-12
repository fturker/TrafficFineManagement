using TrafficFineManagement.Web.Data.Repositories.Implementations;
using TrafficFineManagement.Web.Data.Repositories.Interfaces;
using TrafficFineManagement.Web.Rules.Implementations;
using TrafficFineManagement.Web.Rules.Interfaces;
using TrafficFineManagement.Web.Services.Implementations;
using TrafficFineManagement.Web.Services.Interfaces;

namespace TrafficFineManagement.Web.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddScoped<ICarRepository, CarRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IFineRepository, FineRepository>();
            services.AddScoped<IFineHistoryRepository, FineHistoryRepository>();

            services.AddScoped<ICarService, CarService>();
            services.AddScoped<IFineService, FineService>();
            services.AddScoped<IFineHistoryService, FineHistoryService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserBusinessRules, UserBusinessRules>();

            return services;
        }
    }
}
