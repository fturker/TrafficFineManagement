using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrafficFineManagement.Web.Models.Entities;

namespace TrafficFineManagement.Web.Data.Config
{
    public class CarConfiguration : IEntityTypeConfiguration<Car>
    {
        public void Configure(EntityTypeBuilder<Car> builder)
        {
            builder.Property(x => x.Plate).IsRequired()
                .HasMaxLength(9);
            builder.HasIndex(x => x.Plate).IsUnique();

            builder.Property(x => x.Brand).HasMaxLength(20);
            builder.Property(x => x.Model).HasMaxLength(20);

            builder.HasOne(x => x.User)
                .WithMany(x => x.Cars)
                .HasForeignKey(x => x.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
} 
