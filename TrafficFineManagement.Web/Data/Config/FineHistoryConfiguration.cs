using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrafficFineManagement.Web.Models.Entities;

namespace TrafficFineManagement.Web.Data.Config
{
    public class FineHistoryConfigure : IEntityTypeConfiguration<FineHistory>
    {
        public void Configure(EntityTypeBuilder<FineHistory> builder)
        {
            builder.Property(x => x.Description)
                .HasMaxLength(1000);

            builder.HasOne(x => x.Fine)
                .WithMany(x => x.Histories)
                .HasForeignKey(x => x.FineId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}