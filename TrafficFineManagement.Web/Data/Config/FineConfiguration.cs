using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrafficFineManagement.Web.Models.Entities;

namespace TrafficFineManagement.Web.Data.Config
{
    public class FineConfigure : IEntityTypeConfiguration<Fine>
    {
        public void Configure(EntityTypeBuilder<Fine> builder)
        {
            builder.Property(x => x.Amount).HasPrecision(8, 2);

            builder.Property(x => x.Description).HasMaxLength(1000);

            builder.HasOne(x => x.Car)
                .WithMany(x => x.Fines)
                .HasForeignKey(x => x.CarId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.CreatedByUser).WithMany()
                .HasForeignKey(x => x.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
