using Microsoft.EntityFrameworkCore;
using TrafficFineManagement.Web.Data;
using TrafficFineManagement.Web.Options;

namespace TrafficFineManagement.Web.Extensions
{
    public static class DatabaseServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabaseContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                var connectionStringOptions = configuration.GetSection(ConnectionStringOptions.ConnectionString).Get<ConnectionStringOptions>();

                if (string.IsNullOrWhiteSpace(connectionStringOptions?.SQLServer))
                {
                    throw new InvalidOperationException(
                        "The Server connection string cannot be null or empty.");
                }

                options.UseSqlServer(connectionStringOptions.SQLServer);
            });

            return services;
        }
    }
}
