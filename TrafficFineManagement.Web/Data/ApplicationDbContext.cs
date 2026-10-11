using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TrafficFineManagement.Web.Models.Entities;

namespace TrafficFineManagement.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Car> Cars { get; set; } = null!;
        public DbSet<Fine> Fines { get; set; } = null!;
        public DbSet<Payment> Payments { get; set; } = null!;
        public DbSet<FineHistory> FineHistories { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
}
