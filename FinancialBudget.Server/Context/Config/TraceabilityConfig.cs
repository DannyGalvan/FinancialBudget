using FinancialBudget.Server.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinancialBudget.Server.Context.Config
{
    public class TraceabilityConfig : IEntityTypeConfiguration<Traceability>
    {
        public void Configure(EntityTypeBuilder<Traceability> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Comments)
                .IsRequired()
                .HasMaxLength(500);

            builder.HasOne(x => x.Request)
                .WithMany(x => x.Traceabilities)
                .HasForeignKey(x => x.RequestId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.RequestStatus)
                .WithMany(x => x.Traceabilities)
                .HasForeignKey(x => x.RequestStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.User)
                .WithMany(x => x.Traceabilities)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
