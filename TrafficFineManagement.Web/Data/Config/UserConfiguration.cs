using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrafficFineManagement.Web.Models.Entities;

namespace TrafficFineManagement.Web.Data.Config
{
    public class UserConfigure : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(x => x.Name)
                .HasMaxLength(40);

            builder.Property(x => x.Surname)
                .HasMaxLength(40);

            builder.Property(x => x.IdentityNumber)
                .HasMaxLength(11);
            builder.HasIndex(x => x.IdentityNumber)
                .IsUnique();

            builder.Property(x => x.Address)
                .HasMaxLength(250);

            builder.Property(x => x.CompanyName)
                .HasMaxLength(200);

            builder.Property(x => x.TaxNumber)
                .HasMaxLength(10);
        }
    }
}
