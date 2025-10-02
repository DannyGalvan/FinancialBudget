using FinancialBudget.Server.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinancialBudget.Server.Context.Config
{
    public class RequestConfig : IEntityTypeConfiguration<Request>
    {
        public void Configure(EntityTypeBuilder<Request> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Name)
                .IsRequired().HasMaxLength(100);
            builder.Property(x => x.Reason)
                .IsRequired().HasMaxLength(500);
            builder.Property(x => x.Email)
                .IsRequired().HasMaxLength(100);
            builder.Property(x => x.AuthorizedReason)
                .IsRequired().HasMaxLength(500);
            builder.Property(x => x.RejectionReason)
                .IsRequired().HasMaxLength(500);

            builder.HasOne(x => x.Origin)
                   .WithMany(o => o.Requests)
                   .HasForeignKey(x => x.OriginId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.RequestStatus)
                     .WithMany(rs => rs.Requests)
                     .HasForeignKey(x => x.RequestStatusId)
                     .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Priority)
                     .WithMany(p => p.Requests)
                     .HasForeignKey(x => x.PriorityId)
                     .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
