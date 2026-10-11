using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrafficFineManagement.Web.Models.Entities;

namespace TrafficFineManagement.Web.Data.Config
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.Property(x => x.Amount)
                .HasPrecision(18, 2);

            builder.Property(x => x.Name)
                .HasMaxLength(100);

            builder.Property(x => x.Surname)
                .HasMaxLength(100);

            builder.Property(x => x.LastSixCardNumber)
                .HasMaxLength(6);

            builder.HasOne(x => x.Fine)
                .WithOne()
                .HasForeignKey<Payment>(x => x.FineId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.User)
                .WithMany(x => x.Payments)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
